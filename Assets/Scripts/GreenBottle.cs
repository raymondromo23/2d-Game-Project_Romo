using UnityEngine;

public class GreenBottle : MonoBehaviour, ICollectible
{
    public void Collect()
    {
        // Get the current saved count (default is 0)
        int currentCount = PlayerPrefs.GetInt("GreenBottleCount", 0);

        UnityEngine.PlayerPrefs.SetInt("GreenBottleCount",
        UnityEngine.PlayerPrefs.GetInt("GreenBottleCount", 0) + 1);
        UnityEngine.PlayerPrefs.Save();


        Debug.Log("Player has collected a green bottle!");
        Destroy(gameObject);
    }
}
