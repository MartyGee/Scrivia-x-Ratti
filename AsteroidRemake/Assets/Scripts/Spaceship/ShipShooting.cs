using UnityEngine;

public class SpaceshipShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject bulletPrefab;
    public Transform[] guns;
    public float fireRate = 0.2f;
    public float bulletSpeed = 10f;
    public bool alternateFire = false; //Toggle for alternating or simultaneous firing

    [Header("Dependencies")]
    [SerializeField] private ShipController movementController;

    private float nextFireTime;
    private int currentGunIndex = 0; //Tracks current gun in alternating mode

    void Awake()
    {
        if (movementController == null)
            movementController = GetComponent<ShipController>();

        if (movementController == null)
            Debug.LogError("Missing ShipController reference on " + gameObject.name);
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
        if (movementController == null || guns == null || guns.Length == 0) return;

        Vector2 shipVelocity = movementController.GetCurrentVelocity();

        if (alternateFire)
        {
            Transform selectedGun = guns[currentGunIndex];
            if (selectedGun != null)
                SpawnBullet(selectedGun, shipVelocity);

            currentGunIndex = (currentGunIndex + 1) % guns.Length;
        }
        else
        {
            foreach (Transform gun in guns)
            {
                if (gun != null)
                    SpawnBullet(gun, shipVelocity);
            }
        }
    }

    void SpawnBullet(Transform gun, Vector2 inheritedVelocity)
    {
        GameObject bullet = Instantiate(bulletPrefab, gun.position, gun.rotation);
        ApplyBulletPhysics(bullet, gun.up, inheritedVelocity);
    }

    void ApplyBulletPhysics(GameObject bullet, Vector2 direction, Vector2 inheritedVelocity)
    {
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb == null)
            bulletRb = bullet.AddComponent<Rigidbody2D>();

        bulletRb.gravityScale = 0;
        bulletRb.velocity = direction * bulletSpeed + inheritedVelocity;
        bulletRb.angularVelocity = 0f; 

        Destroy(bullet, 2f);
    }
}
