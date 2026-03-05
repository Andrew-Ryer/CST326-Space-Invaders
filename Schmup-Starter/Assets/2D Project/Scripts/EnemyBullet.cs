using UnityEngine;

// Set speed and direction of Enemy bullet (RED)
public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;

    private void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * speed;
        Destroy(gameObject, 6f);
    }
}
