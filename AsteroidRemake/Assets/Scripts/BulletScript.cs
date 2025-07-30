using UnityEngine;

public class BulletScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            Debug.Log("Bullet hit an asteroid!");
            Destroy(other.gameObject);  // Destroy the asteroid
            Destroy(gameObject);        // Destroy the bullet
        }
    }
}
