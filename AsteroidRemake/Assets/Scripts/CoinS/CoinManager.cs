using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public TMP_Text coinText; // Drag your TMP UI Text here in the Inspector
    private int coinCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("CoinManager initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        coinCount++;
        Debug.Log("CoinManager: Coin added. Total = " + coinCount);

        if (coinText != null)
        {
            coinText.SetText("Coins: " + coinCount);
        }
        else
        {
            Debug.LogError("CoinManager: coinText not assigned!");
        }
    }
}