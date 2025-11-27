using UnityEngine;

public class InventoryItem : MonoBehaviour, ICollectible
{
    public string itemName;

    public void Collect()
    {
        InventorySystem.Instance.AddItem(itemName);
        Destroy(gameObject);
    }
}
