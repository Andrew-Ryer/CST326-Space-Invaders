using System.Collections;
using UnityEngine;

// Animate the sprite with discrete steps (Used with Movement Part)
public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites = new Sprite[0];
    public float animationTime = 1f;
    public int score = 10;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    private Animator animator;
    public AudioClip deathSound;
    AudioSource audioSource;
    
    public delegate void EnemyDiedFunc(float points);
    public static event EnemyDiedFunc onEnemyDied;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriteRenderer.sprite = animationSprites[0];
    }
    
    // Uses Discrete Steps
    // Uses Discrete Steps
    public void StepAnimation()
    {
        // Fire exactly once per movement tick (EnemyArray.Step())
        if (animator != null)
        {
            animator.SetTrigger("Step");
        }
    }

    // Called by EnemyArray when this invader is chosen to shoot.
    public void ShootAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet layer should be named "bullet"
        if (other.gameObject.layer == LayerMask.NameToLayer("bullet"))
        {
            // Death Audio Plays here
            Destroy(other.gameObject);
            animator.SetTrigger("isDead");
            audioSource.PlayOneShot(deathSound);
            //Destroy(gameObject, 1f);
            onEnemyDied?.Invoke(score);
            StartCoroutine(ResetAfterDelay(1f));
        }
    }
    
    // Trying to delay Destroy(object) So Death animation can Play
    private IEnumerator ResetAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
