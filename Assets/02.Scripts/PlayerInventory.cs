using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> collectedItems = new List<Item>();
    private ItemManager itemManager;

    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
    }

    public void AddItem(int itemId)
    {
        Item itemToAdd = itemManager.GetItemById(itemId);
        if (itemToAdd != null)
        {
            collectedItems.Add(itemToAdd);
            Debug.Log("아이템 추가됨: " + itemToAdd.itemName);
        }
        else
        {
            Debug.LogWarning("아이템을 찾을 수 없음. ID: " + itemId);
        }
    }
}