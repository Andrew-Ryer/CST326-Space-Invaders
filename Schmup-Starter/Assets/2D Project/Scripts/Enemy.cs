using System.Collections;
using UnityEngine;

// Logic to kill enemy with bullet and tally score
public class Enemy : MonoBehaviour
{
    // C# Special Event call
    public delegate void EnemyDiedFunc(int points);

    public static event EnemyDiedFunc onEnemyDied;
    
    private Animator animator;
    
    public int score = 10;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ouch!");
        
        // todo - destroy the bullet
        if (collision.gameObject.layer == LayerMask.NameToLayer("bullet"))
        {
            // Death animation plays here
            Destroy(collision.gameObject);
            animator.SetTrigger("isDead");
            //audioSource.PlayOneShot(deathSound);
            //Destroy(gameObject, 1f);
            onEnemyDied?.Invoke(score);
            StartCoroutine(ResetAfterDelay(1f));
        }
    }
    
    private IEnumerator ResetAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
