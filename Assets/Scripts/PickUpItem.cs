using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Item item; // �������
    public int itemQuantity = 1; // ���������� ���������

	private void Start()
	{
        
	}
	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();

            if (inventory != null)
            {
                inventory.AddItem(item, itemQuantity);
                Destroy(gameObject);
            }
        }
    }
}
