using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> collectedItems = new List<Item>();
    private ItemManager itemManager;
    public Transform inventoryPanel; // InventoryPanel을 참조합니다.
    public GameObject slotPrefab; // SlotPrefab을 참조합니다.

    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        UpdateInventoryUI(); // 초기화 시 UI를 업데이트합니다.
    }

    public void AddItem(int itemId)
    {
        Item itemToAdd = itemManager.GetItemById(itemId);
        if (itemToAdd != null)
        {
            collectedItems.Add(itemToAdd);
            Debug.Log("아이템 추가됨: " + itemToAdd.itemName);
            UpdateInventoryUI(); // 아이템 추가 시 UI를 업데이트합니다.
        }
        else
        {
            Debug.LogWarning("아이템을 찾을 수 없음. ID: " + itemId);
        }
    }

    // 인벤토리 UI를 업데이트하는 메서드
    private void UpdateInventoryUI()
    {
        // 기존 슬롯 제거
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // 아이템 리스트를 기반으로 슬롯 생성
        foreach (Item item in collectedItems)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryPanel);
            Image slotImage = slot.GetComponent<Image>();
            slotImage.sprite = item.icon;
            slotImage.color = Color.white; // 이미지가 보이도록 색상을 설정합니다.
        }
    }
}
