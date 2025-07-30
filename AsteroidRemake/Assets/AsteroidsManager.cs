using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Settings")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay = 1.5f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float minSpin = -50f;
    [SerializeField] private float maxSpin = 50f;
    [SerializeField] private string destructionTag = "PlayerWeapon";

    [Header("Effects")]
    [SerializeField] private GameObject destructionEffect;
    [SerializeField] private float effectDuration = 1f;
    [SerializeField] private GameObject spaceship;

    [Header("Collision Settings")]
    [SerializeField] private string asteroidTag = "Asteroid";

    private List<GameObject> activeAsteroids = new List<GameObject>();

    private void Start()
    {
        if (!ValidateComponents()) return;

        // Configure all collision systems
        SetupSpaceshipCollision();
        TagAllAsteroids();

        StartCoroutine(SpawnAsteroids());
    }

    private bool ValidateComponents()
    {
        if (!asteroidPrefab)
        {
            Debug.LogError("Asteroid prefab not assigned!");
            return false;
        }

        if (!asteroidPrefab.TryGetComponent<Rigidbody2D>(out _))
        {
            Debug.LogError("Asteroid prefab needs a Rigidbody2D!");
            return false;
        }

        if (!spawnPoint)
        {
            Debug.LogError("Spawn point not assigned!");
            return false;
        }

        if (!spaceship)
        {
            Debug.LogError("Spaceship reference not assigned!");
            return false;
        }

        return true;
    }

    private void TagAllAsteroids()
    {
        asteroidPrefab.tag = asteroidTag;
    }

    private void SetupSpaceshipCollision()
    {
        // Ensure spaceship has required components
        if (!spaceship.TryGetComponent<Collider2D>(out var shipCollider))
        {
            shipCollider = spaceship.AddComponent<BoxCollider2D>();
        }
        shipCollider.isTrigger = true;

        if (!spaceship.TryGetComponent<SpaceshipCollisionHandler>(out var handler))
        {
            handler = spaceship.AddComponent<SpaceshipCollisionHandler>();
        }
        handler.Initialize(this, destructionEffect, effectDuration);
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
    {
        GameObject asteroid = Instantiate(
            asteroidPrefab,
            spawnPoint.position,
            Quaternion.Euler(0, 0, Random.Range(0f, 360f))
        );

        SetupAsteroidPhysics(asteroid);
        AddCollisionHandler(asteroid);
        activeAsteroids.Add(asteroid);
    }

    private void SetupAsteroidPhysics(GameObject asteroid)
    {
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        Vector2 direction = Random.insideUnitCircle.normalized;
        rb.velocity = direction * Random.Range(minSpeed, maxSpeed);
        rb.angularVelocity = Random.Range(minSpin, maxSpin);
    }

    private void AddCollisionHandler(GameObject asteroid)
    {
        if (!asteroid.TryGetComponent<Collider2D>(out var collider))
        {
            collider = asteroid.AddComponent<CircleCollider2D>();
        }
        collider.isTrigger = true;

        AsteroidCollisionHandler handler = asteroid.AddComponent<AsteroidCollisionHandler>();
        handler.Initialize(this, destructionTag, destructionEffect, effectDuration);
    }

    public void DestroyAsteroid(GameObject asteroid, GameObject effectPrefab)
    {
        if (activeAsteroids.Contains(asteroid))
        {
            if (effectPrefab != null)
            {
                GameObject effect = Instantiate(effectPrefab, asteroid.transform.position, Quaternion.identity);
                Destroy(effect, effectDuration);
            }

            activeAsteroids.Remove(asteroid);
            Destroy(asteroid);
        }
    }
}

public class AsteroidCollisionHandler : MonoBehaviour
{
    private AsteroidSpawner spawner;
    private string destroyTag;
    private GameObject destructionEffect;
    private float effectDuration;

    public void Initialize(AsteroidSpawner spawnerRef, string tag, GameObject effect, float duration)
    {
        spawner = spawnerRef;
        destroyTag = tag;
        destructionEffect = effect;
        effectDuration = duration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(destroyTag))
        {
            spawner.DestroyAsteroid(gameObject, destructionEffect);
        }
    }
}

public class SpaceshipCollisionHandler : MonoBehaviour
{
    private AsteroidSpawner spawner;
    private GameObject destructionEffect;
    private float effectDuration;

    public void Initialize(AsteroidSpawner spawnerRef, GameObject effect, float duration)
    {
        spawner = spawnerRef;
        destructionEffect = effect;
        effectDuration = duration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            DestroySpaceship();
        }
    }

    private void DestroySpaceship()
    {
        if (destructionEffect != null)
        {
            GameObject effect = Instantiate(destructionEffect, transform.position, Quaternion.identity);
            Destroy(effect, effectDuration);
        }

        Debug.Log("Spaceship destroyed!");
        Destroy(gameObject);

        // Add any additional game over logic here
        // Example: FindObjectOfType<GameManager>().GameOver();
    }
}