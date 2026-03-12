using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Player keyboard movement (Clamped to edges)
// Hold starting position for a reset
// Track offset for shooting bullets
// Die (reset) if hit by enemy bullet
public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootOffsetTransform;
    
    public float speed = 10f;
    
    public float minX = -40f;
    public float maxX = 40f;
    
    public static event Action onPlayerDied;
    private Animator animator;
    private Vector3 startPosition;
    
    
    
    public AudioClip shotSound;
    public AudioClip deathSound;
    
    AudioSource audioSource;

    void Start()
    {
        // Sound
        audioSource = GetComponent<AudioSource>();
        
        // Animation
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }
    
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Play shot audio
            audioSource.PlayOneShot(shotSound);
            GameObject shot = Instantiate(bulletPrefab, shootOffsetTransform.position, Quaternion.identity);
            
            Debug.Log("Bang!");

            // todo - destroy the bullet after x seconds
            Destroy(shot, 6f);
            // todo - trigger shoot animation
            GetComponent<Animator>().SetTrigger("Shot Trigger");

        }
        
        // Move right/left axis
        if (Keyboard.current == null) return;

        float dir = 0f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            dir += 1f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            dir -= 1f;

        if (dir != 0f)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x + dir * speed * Time.deltaTime, minX, maxX);
            transform.position = pos;
        }

      
    }
    
    public void ResetPlayer()
    {
        SceneManager.LoadScene("2D Project/Scenes/Credits");
        transform.position = startPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        // Play death audio
        audioSource.PlayOneShot(deathSound);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ebullet")) 
        { 
            Destroy(other.gameObject);
            
            onPlayerDied?.Invoke();
            animator.SetTrigger("isDead");
            StartCoroutine(ResetAfterDelay(2f));

        }
        
    }
    
    // Trying to delay ResetPlayer So Death animation can Play
    private IEnumerator ResetAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetPlayer();
    }

}
