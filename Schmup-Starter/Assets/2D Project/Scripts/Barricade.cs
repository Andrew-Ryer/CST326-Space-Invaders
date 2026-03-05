using UnityEngine;

// Barricade takes 3 bullet shots (player or enemy) then is destroyed
public class Barricade : MonoBehaviour
{
    public int health = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        int bulletLayer = LayerMask.NameToLayer("bullet");
        int enemyBulletLayer = LayerMask.NameToLayer("Ebullet");

        if (other.gameObject.layer != bulletLayer && other.gameObject.layer != enemyBulletLayer)
        {
            return;
        }

        Destroy(other.gameObject);

        health--;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
