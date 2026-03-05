using UnityEngine;

// Animate the sprite with discrete steps (Used with Movement Part)
public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites = new Sprite[0];
    public float animationTime = 1f;
    public int score = 10;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;
    
    public delegate void EnemyDiedFunc(float points);
    public static event EnemyDiedFunc onEnemyDied;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animationSprites[0];
    }

    // Just Repeats animation
    // private void Start()
    // {
    //     // Another way to animate
    //     InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    // }
    
    // Uses Discrete Steps
    public void StepAnimation()
    {
        AnimateSprite();
    }

    private void AnimateSprite()
    {
        animationFrame++;

        // Loop back to the start if the animation frame exceeds the length
        if (animationFrame >= animationSprites.Length) {
            animationFrame = 0;
        }

        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet layer should be named "bullet"
        if (other.gameObject.layer == LayerMask.NameToLayer("bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            onEnemyDied?.Invoke(score);
        }
    }
}
