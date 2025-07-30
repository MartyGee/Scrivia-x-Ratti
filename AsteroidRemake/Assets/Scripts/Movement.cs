using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceshipMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float rotateSpeed = 3f;
    public float thrustPower = 0.5f;
    public float maxSpeed = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ConfigurePhysics();
    }

    void Update()
    {
        HandleRotation();
        HandleThrust();
    }

    void ConfigurePhysics()
    {
        rb.gravityScale = 0;
        rb.linearDamping = 0;
        rb.angularDamping = 0;
    }

    void HandleRotation()
    {
        float rotation = Input.GetAxis("Horizontal") * rotateSpeed;
        transform.Rotate(0, 0, -rotation);
    }

    void HandleThrust()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            Vector2 thrustDirection = transform.up;
            rb.AddForce(thrustDirection * thrustPower, ForceMode2D.Impulse);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    // Metodo aggiunto per permettere l'accesso alla velocit�
    public Vector2 GetCurrentVelocity()
    {
        return rb.linearVelocity;
    }
}