using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        // todo - get and cache animator
    }
    
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
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
        transform.position = startPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ebullet")) 
        { 
            Destroy(other.gameObject);
            ResetPlayer();
            onPlayerDied?.Invoke(); 
        }
        
    }

}
