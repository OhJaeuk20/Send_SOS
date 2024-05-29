using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;

    // 생성자
    public Item(int id, string itemName, string description, Sprite icon)
    {
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        this.icon = icon;
    }
}