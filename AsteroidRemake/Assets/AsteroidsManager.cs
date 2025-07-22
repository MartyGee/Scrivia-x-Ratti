using UnityEngine;

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

    private void Start()
    {
        // Safety checks
        if (!asteroidPrefab)
        {
            Debug.LogError("Asteroid prefab not assigned!");
            return;
        }

        if (!asteroidPrefab.GetComponent<Rigidbody2D>())
        {
            Debug.LogError("Asteroid prefab needs a Rigidbody2D!");
            return;
        }

        if (!spawnPoint)
        {
            Debug.LogError("Spawn point not assigned!");
            return;
        }

        StartCoroutine(SpawnAsteroids());
    }

    private System.Collections.IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnAsteroid()
    {
        // Create asteroid with random rotation
        GameObject asteroid = Instantiate(
            asteroidPrefab,
            spawnPoint.position,
            Quaternion.Euler(0, 0, Random.Range(0f, 360f))
        );

        // Configure physics
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Disable gravity
        rb.linearDamping = 0f; // Remove any drag

        // Set constant velocity (straight line movement)
        Vector2 direction = Random.insideUnitCircle.normalized;
        float speed = Random.Range(minSpeed, maxSpeed);
        rb.linearVelocity = direction * speed;

        // Add random spin
        float spin = Random.Range(minSpin, maxSpin);
        rb.angularVelocity = spin;
    }
}