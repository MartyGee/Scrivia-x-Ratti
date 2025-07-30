using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    public float thrustForce = 5f;
    public float rotationSpeed = 200f;
    public float maxSpeed = 5f;
    public int health = 3;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 0;
        rb.angularDrag = 0;
    }

    void FixedUpdate()
    {
        // Rotation input (A/D or Left/Right Arrows)
        float rotationInput = -Input.GetAxis("Horizontal");
        rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.fixedDeltaTime);

        // Thrust input (W or Up Arrow)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Vector2 force = transform.up * thrustForce;
            rb.AddForce(force);
        }

        // Clamp max speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public Vector2 GetCurrentVelocity()
    {
        return rb.velocity;
    }

    void LateUpdate()
    {
        WrapAroundScreen();
    }

    void WrapAroundScreen()
    {
        Vector3 newPosition = transform.position;
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        if (transform.position.x > screenBounds.x)
            newPosition.x = -screenBounds.x;
        else if (transform.position.x < -screenBounds.x)
            newPosition.x = screenBounds.x;

        if (transform.position.y > screenBounds.y)
            newPosition.y = -screenBounds.y;
        else if (transform.position.y < -screenBounds.y)
            newPosition.y = screenBounds.y;

        transform.position = newPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            health--;
            Debug.Log("Ship hit an asteroid! Health: " + health);
            if (health <= 0)
            {
                Debug.Log("Ship destroyed!");
                Destroy(gameObject);
                // Optionally trigger game over logic here
            }
        }
    }
}
