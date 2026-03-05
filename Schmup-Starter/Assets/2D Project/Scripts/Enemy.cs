using UnityEngine;

// Logic to kill enemy with bullet and tally score
public class Enemy : MonoBehaviour
{
    // C# Special Event call
    public delegate void EnemyDiedFunc(int points);

    public static event EnemyDiedFunc onEnemyDied;
    
    public int score = 10;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ouch!");
        
        // todo - destroy the bullet
        if (collision.gameObject.layer == LayerMask.NameToLayer("bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
            // ? means "if null don't do following"
            onEnemyDied?.Invoke(score);
        }
        // todo - trigger death animation
    }
}
