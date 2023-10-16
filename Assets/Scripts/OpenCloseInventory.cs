using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseInventory : MonoBehaviour
{
    public GameObject inventoryWindow;
    public void OpenClose()
    {
        inventoryWindow.active = !inventoryWindow.active;
        inventoryWindow.GetComponent<InventoryUIScript>().RefreshUI();
    }
}
