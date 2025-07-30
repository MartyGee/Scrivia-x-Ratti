using UnityEngine;

public class SpaceshipShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject bulletPrefab;
    public Transform[] guns;
    public float fireRate = 0.2f;
    public float bulletSpeed = 10f;

    [Header("Dependencies")]
    [SerializeField] private SpaceshipMovement movementController; // Riferimento rinominato

    private float nextFireTime;

    void Awake()
    {
        // Risoluzione automatica se il riferimento non è assegnato
        if (movementController == null)
            movementController = GetComponent<SpaceshipMovement>();

        if (movementController == null)
            Debug.LogError("SpaceshipMovement component missing!");
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            FireBullets();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireBullets()
    {
        if (movementController == null) return;

        Vector2 shipVelocity = movementController.GetCurrentVelocity();

        foreach (Transform gun in guns)
        {
            if (gun == null) continue;

            GameObject bullet = Instantiate(
                bulletPrefab,
                gun.position,
                gun.rotation
            );

            ConfigureBulletPhysics(bullet, gun.up, shipVelocity);
        }
    }

    void ConfigureBulletPhysics(GameObject bullet, Vector3 direction, Vector2 inheritedVelocity)
    {
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb == null) bulletRb = bullet.AddComponent<Rigidbody2D>();

        bulletRb.linearVelocity = (Vector2)direction * bulletSpeed + inheritedVelocity;
        bulletRb.angularVelocity = Random.Range(-90f, 90f);
        Destroy(bullet, 2f);
    }
}