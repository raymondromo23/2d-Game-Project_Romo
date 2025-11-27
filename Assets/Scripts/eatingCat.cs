using UnityEngine;

public class eatingCat : MonoBehaviour, ICollectible
{
    public void Collect()
    {




        int currentCount = PlayerPrefs.GetInt("EatCatCount", 0);
        UnityEngine.PlayerPrefs.SetInt("EatCatCount",
        UnityEngine.PlayerPrefs.GetInt("EatCatCount", 0) + 1);
        UnityEngine.PlayerPrefs.Save();


        Debug.Log("YOU ATE A CATTTT!???");
        Destroy(gameObject);
    }
}
