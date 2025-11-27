using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TMP_Text inventoryText;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);

            if (isOpen)
                RefreshInventory();
        }
    }

    void RefreshInventory()
    {
        inventoryText.text = "Inventory:\n";

        foreach (string item in InventorySystem.Instance.items)
        {
            inventoryText.text += "- " + item + "\n";
        }
    }
}
