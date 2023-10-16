using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{
    public Inventory inventory;
    public GameObject cellPrefab;
    public Transform gridContainer;
    public TMP_Text nameItem;
    public TMP_Text canDropLabel;
    public List<GameObject> itemCells = new List<GameObject>();
    public GameObject confirmationDialog; // Ссылка на диалоговое окно подтверждения


    private int selectedItemIndex = -1; // Индекс выбранной ячейки

    public void RefreshUI()
    {

        foreach (var cell in itemCells)
        {
            Destroy(cell);
        }
        itemCells.Clear();

        for (int i = 0; i < inventory.items.Count; i++)
        {
            int itemIndex = i; // сохранить индекс предмета для обработки нажатия

            GameObject cell = Instantiate(cellPrefab, gridContainer);
            itemCells.Add(cell);
            cell.GetComponent<Image>().sprite = inventory.items[i].item.icon;
            if (inventory.items[i].quantity > 1) cell.transform.GetChild(0).GetComponent<TMP_Text>().text = inventory.items[i].quantity.ToString();
            else cell.transform.GetChild(0).GetComponent<TMP_Text>().text = "";

            EventTrigger trigger = cell.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
            entry.callback.AddListener((data) => { OnCellClick(itemIndex); });
            trigger.triggers.Add(entry);
        }
    }

    private void OnCellClick(int itemIndex)
    {
        selectedItemIndex = itemIndex;
        nameItem.text = inventory.items[selectedItemIndex].item.name.ToString();

        // диалоговое окно с кнопками "Подтвердить" и "Отменить"
        confirmationDialog.SetActive(true);
        Debug.Log(inventory.items[selectedItemIndex].item.canDrop);
        canDropLabel.enabled = inventory.items[selectedItemIndex].item.canDrop;
    }
    public void ConfirmDelete()
    {
        if (selectedItemIndex >= 0)
        {
            Item item = inventory.items[selectedItemIndex].item;
            int quantity = inventory.items[selectedItemIndex].quantity;
            if (inventory.RemoveItem(item, quantity))
            {
                Destroy(itemCells[selectedItemIndex]);
                itemCells.RemoveAt(selectedItemIndex);
            }
            CancelDelete();
            RefreshUI();
        }
    }
    public void CancelDelete()
    {
        confirmationDialog.SetActive(false);
        selectedItemIndex = -1;
    }

}
