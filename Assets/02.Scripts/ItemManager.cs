using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> itemDatabase = new List<Item>();

    void Start()
    {
        // 임시 아이템 데이터베이스 초기화 (나중에 실제 데이터로 대체)
        itemDatabase.Add(new Item(1, "고유 아이템 1", "설명 1", null));
        itemDatabase.Add(new Item(2, "고유 아이템 2", "설명 2", null));
        itemDatabase.Add(new Item(3, "고유 아이템 1", "설명 1", null));
        itemDatabase.Add(new Item(4, "고유 아이템 2", "설명 2", null));
        itemDatabase.Add(new Item(5, "고유 아이템 1", "설명 1", null));
        // 필요한 만큼 추가...
    }

    public Item GetItemById(int id)
    {
        return itemDatabase.Find(item => item.id == id);
    }
}