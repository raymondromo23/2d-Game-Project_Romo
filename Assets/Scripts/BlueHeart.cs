using UnityEngine;

public class BlueHeart : MonoBehaviour, ICollectible      
{
    public void Collect()
    {
        int currentCount = PlayerPrefs.GetInt("HeartCount", 0);

        UnityEngine.PlayerPrefs.SetInt("HeartCount",
        UnityEngine.PlayerPrefs.GetInt("HeartCount", 0) + 1);
        UnityEngine.PlayerPrefs.Save();

        Debug.Log("Player has collected a blue heart!");
        Destroy(gameObject);
    }
}
