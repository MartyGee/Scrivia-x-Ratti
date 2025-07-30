using UnityEngine;

public class ShipCollision : MonoBehaviour
{
    public int health = 3;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            health--;
            Debug.Log("Ship hit! Health: " + health);
            if (health <= 0)
            {
                Debug.Log("Ship destroyed!");
                Destroy(gameObject);
            }
        }
    }
}
