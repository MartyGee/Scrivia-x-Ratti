using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float spawnInterval = 2f;
    public float asteroidSpeedMin = 1f;
    public float asteroidSpeedMax = 4f;
    public float spawnMargin = 1f;              // Margin outside camera to spawn asteroids
    public float minScale = 0.5f;                // Minimum asteroid scale
    public float maxScale = 2f;                  // Maximum asteroid scale
    public float destroyAfterSeconds = 30f;     // How long before asteroid gets destroyed

    private Camera mainCamera;
    private Transform player;

    void Start()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        InvokeRepeating(nameof(SpawnAsteroid), 1f, spawnInterval);
    }

    void SpawnAsteroid()
    {
        if (asteroidPrefab == null || player == null) return;

        Vector2 spawnPos = GetSpawnPositionOutsideCamera();

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        // Random scale between minScale and maxScale
        float randomScale = Random.Range(minScale, maxScale);
        asteroid.transform.localScale = Vector3.one * randomScale;

        Vector2 moveDir;

        if (Random.value < 0.7f) // 70% chance to aim near player
        {
            Vector2 directionToPlayer = ((Vector2)player.position - spawnPos).normalized;
            Vector2 randomOffset = Random.insideUnitCircle * 0.3f;
            moveDir = (directionToPlayer + randomOffset).normalized;
        }
        else // 30% chance to move randomly
        {
            moveDir = Random.insideUnitCircle.normalized;
        }

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speed = Random.Range(asteroidSpeedMin, asteroidSpeedMax);
            rb.velocity = moveDir * speed;
            rb.angularVelocity = Random.Range(-30f, 30f);
        }

        Destroy(asteroid, destroyAfterSeconds);
    }

    Vector2 GetSpawnPositionOutsideCamera()
    {
        // Get camera bounds in world units
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Pick a random side of the screen to spawn from: 0=top,1=right,2=bottom,3=left
        int side = Random.Range(0, 4);

        Vector2 spawnPos = Vector2.zero;

        switch (side)
        {
            case 0: // top
                spawnPos = new Vector2(
                    Random.Range(player.position.x - camWidth / 2, player.position.x + camWidth / 2),
                    player.position.y + camHeight / 2 + spawnMargin);
                break;

            case 1: // right
                spawnPos = new Vector2(
                    player.position.x + camWidth / 2 + spawnMargin,
                    Random.Range(player.position.y - camHeight / 2, player.position.y + camHeight / 2));
                break;

            case 2: // bottom
                spawnPos = new Vector2(
                    Random.Range(player.position.x - camWidth / 2, player.position.x + camWidth / 2),
                    player.position.y - camHeight / 2 - spawnMargin);
                break;

            case 3: // left
                spawnPos = new Vector2(
                    player.position.x - camWidth / 2 - spawnMargin,
                    Random.Range(player.position.y - camHeight / 2, player.position.y + camHeight / 2));
                break;
        }

        return spawnPos;
    }
}
