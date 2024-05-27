using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int itemId;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemId);
                Destroy(gameObject); // 아이템 오브젝트 제거
            }
        }
    }
}