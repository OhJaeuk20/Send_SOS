using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> itemDatabase = new List<Item>();

    void Start()
    {
        // 임시 아이템 데이터베이스 초기화 (나중에 실제 데이터로 대체)
        itemDatabase.Add(new Item(1, "십자 드라이버", "누군가 캠핑을 즐기고 두고 간 십자 드라이버", Resources.Load<Sprite>("Images/Item_Driver")));
        itemDatabase.Add(new Item(2, "포크", "왜 가정집에 안테나가...?", null));
        itemDatabase.Add(new Item(3, "배터리", "장난감에서 뜯어낸 찌릿한 무언가", null));
        itemDatabase.Add(new Item(4, "무전기", "군인들이 떨어뜨린 지구 통신장치", null));
        itemDatabase.Add(new Item(5, "고유 아이템 1", "설명 1", null));
        // 필요한 만큼 추가...
    }

    public Item GetItemById(int id)
    {
        return itemDatabase.Find(item => item.id == id);
    }
}