using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public new string name;
    public bool canDrop;
    public Sprite icon;
    public GameObject itemPrefab; // Префаб предмета для создания в мире
}
