using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int itemId;

    private AudioSource sound;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemId);
                sound.Play();
                Destroy(gameObject); // 아이템 오브젝트 제거
            }
        }
    }
}