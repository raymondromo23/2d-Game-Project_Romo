using UnityEngine;

public class Gem : MonoBehaviour, ICollectible
{
    public void Collect()
    {

        int currentCount = PlayerPrefs.GetInt("GemCount", 0);

        UnityEngine.PlayerPrefs.SetInt("GemCount",
        UnityEngine.PlayerPrefs.GetInt("GemCount", 0) + 1);
        UnityEngine.PlayerPrefs.Save();

        Debug.Log("Player has collected a gem!");
        Destroy(gameObject);
    }
}
