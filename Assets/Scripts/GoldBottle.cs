using UnityEngine;

public class GoldBottle : MonoBehaviour, ICollectible
{
    public void Collect()
    {
        int currentCount = PlayerPrefs.GetInt("GreenBottleCount", 0);

        UnityEngine.PlayerPrefs.SetInt("GreenBottleCount",
        UnityEngine.PlayerPrefs.GetInt("GreenBottleCount", 0) + 1);
        UnityEngine.PlayerPrefs.Save();
        Debug.Log("Player has collected a Golden Bottle!");
        Destroy(gameObject);
    }
}
