using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;         // Coin prefab to spawn
    public BoxCollider2D spawnArea;       // Area to spawn coins in
    public int maxCoinsOnScene = 10;      // Max number of coins allowed at once
    public float spawnInterval = 2f;      // Time between spawn checks

    private List<GameObject> coins = new List<GameObject>();

    void Start()
    {
        InvokeRepeating("TrySpawnCoin", 0f, spawnInterval);
    }

    void TrySpawnCoin()
    {
        // Clean up list in case some coins were destroyed
        coins.RemoveAll(item => item == null);

        if (coins.Count < maxCoinsOnScene)
        {
            Vector2 spawnPosition = GetRandomPosition();
            GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            coins.Add(newCoin);

            Debug.Log("Spawned coin. Total coins on scene: " + coins.Count);
        }
        else
        {
            Debug.Log("Coin limit reached. No new coins spawned.");
        }
    }

    Vector2 GetRandomPosition()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }
}