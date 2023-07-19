using UnityEngine;

[System.Serializable]
public class Item 
{

    public ItemType type;
    public ItemRank rank;
    public Sprite itemSprite;
    public string itemName;
    public float attack;
    public float rate;
    public float moveSpeed;
    public string itemDesc;

    public Item(ItemType type, ItemRank rank, Sprite itemSprite, string itemName, float attack, float rate, float moveSpeed, string itemDesc)
    {
        this.type = type;
        this.rank = rank;
        this.itemSprite = itemSprite;
        this.itemName = itemName;
        this.attack = attack;
        this.rate = rate;
        this.moveSpeed = moveSpeed;
        this.itemDesc = itemDesc;
    }

    public void reset()
    {
        type = ItemType.Default; // ItemType과 ItemRank는 각자 기본 값을 설정해주어야 합니다.
        rank = ItemRank.Default;
        itemSprite = null;
        itemName = "";
        attack = 0f;
        rate = 0f;
        moveSpeed = 0f;
        itemDesc = "";
    }
}
