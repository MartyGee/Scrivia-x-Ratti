using UnityEngine;

public class CoinScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("2D coin collected!");

            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoin();
            }
            else
            {
                Debug.LogError("CoinScript: CoinManager.Instance is null!");
            }

            Destroy(gameObject);
        }
    }
}