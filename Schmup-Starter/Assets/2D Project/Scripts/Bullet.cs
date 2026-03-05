using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

// Set speed and direction of player bullet (GREEN)
public class Bullet : MonoBehaviour
{
    public float speed = 5;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * speed;
        Debug.Log("Wwweeeeee");
    }
}
