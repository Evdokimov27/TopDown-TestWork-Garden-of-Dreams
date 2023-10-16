using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int quantity;

    public InventoryItem(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
[System.Serializable]
public class InventoryData
{
    public List<InventoryItem> itemsData = new List<InventoryItem>();
}

public class Inventory : MonoBehaviour
{
    // Сохранить инвентарь в файл
    string fullPath = "inventory.json";
    bool Save = true;
    public List<InventoryItem> items = new List<InventoryItem>();

    public void Update()
    {
        if (Save)
        {
            StartCoroutine(SaveInventory("inventory.json"));
            Save = false;
        }
    }
	public IEnumerator SaveInventory(string fileName)
    {
        Debug.Log("Save");

        InventoryData data = new InventoryData
        {
            itemsData = items
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(fileName, json);
        yield return new WaitForSeconds(30f);
        Save = true;
    }
	public void Start()
	{
        LoadInventory(fullPath);
    }

    // Загрузить инвентарь из файла
    public void LoadInventory(string fileName)
    {
        if (File.Exists(fileName))
        {
            Debug.Log("load");

            string json = File.ReadAllText(fileName);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            items = data.itemsData;
        }
    }
    public void AddItem(Item item, int quantity)
    {
        // Сохранение инвентаря в файл
        InventoryItem existingItem = items.Find(i => i.item == item);

        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            items.Add(new InventoryItem(item, quantity));
        }
        SaveInventory(fullPath);
    }

    public bool RemoveItem(Item item, int quantity)
    {
        bool drop = true;
        InventoryItem existingItem = items.Find(i => i.item == item);
        SaveInventory(fullPath);
        if (existingItem != null && item.canDrop)
        {
            existingItem.quantity -= quantity;

            if (existingItem.quantity <= 0)
            {
                items.Remove(existingItem);
                drop = true;
            }
        }
        else drop = false;
        return drop;
    }
    public int GetItemCount(Item item)
    {
        InventoryItem existingItem = items.Find(i => i.item == item);
        return existingItem != null ? existingItem.quantity : 0;
    }
    public void DecreaseItemQuantity(Item item, int amount)
    {
        InventoryItem inventoryItem = items.Find(x => x.item == item);

        if (inventoryItem != null)
        {
            inventoryItem.quantity -= amount;

            // Если количество стало меньше или равно нулю, удалите предмет из инвентаря
            if (inventoryItem.quantity <= 0)
            {
                items.Remove(inventoryItem);
            }
        }
    }
    public void DropItem(Item item, int quantity)
    {
        InventoryItem existingItem = items.Find(i => i.item == item);

        if (existingItem != null && item.canDrop) // Проверка флага canDrop
        {
            existingItem.quantity -= quantity;

            if (existingItem.quantity <= 0)
            {
                items.Remove(existingItem);
            }
        }
    }
}

