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
    // ��������� ��������� � ����
    string fullPath = "inventory.json";
    bool Save = true;
    bool Load = false;
    public List<InventoryItem> items = new List<InventoryItem>();
    private float timerDuration = 30f; // ������������ ������� � ��������
    private float timer;

    public void Start()
    {
        LoadInventory(fullPath);
        Load = true;
    }
    public void Update()
    {
        Debug.Log(timer);
        if (Save && Load)
        {
            SaveInventory("inventory.json");
            Save = false;
        }
        if (timer <= 0f)
        {
            Save = true;
            StartTimer();
        }
        else timer -= Time.deltaTime;
    }
	public void SaveInventory(string fileName)
    {
        Debug.Log("Save");
        InventoryData data = new InventoryData
        {
            itemsData = items
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(fileName, json);
    }

    private void StartTimer()
    {
        timer = timerDuration;
    }

    // ��������� ��������� �� �����
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
        // ���������� ��������� � ����
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

            // ���� ���������� ����� ������ ��� ����� ����, ������� ������� �� ���������
            if (inventoryItem.quantity <= 0)
            {
                items.Remove(inventoryItem);
            }
        }
    }
    public void DropItem(Item item, int quantity)
    {
        InventoryItem existingItem = items.Find(i => i.item == item);

        if (existingItem != null && item.canDrop) // �������� ����� canDrop
        {
            existingItem.quantity -= quantity;

            if (existingItem.quantity <= 0)
            {
                items.Remove(existingItem);
            }
        }
    }
}

