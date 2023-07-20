using UnityEngine;

[System.Serializable]
public class Item 
{

    public ItemType type;
    public ItemRank rank;
    public ItemQuality quality;
    public Sprite itemSprite;
    public string itemName;
    public int attack;
    public float rate;
    public float moveSpeed;
    public string itemDesc;
   
    public Item(ItemType type, ItemRank rank, ItemQuality quality, Sprite itemSprite, string itemName, int attack, float rate, float moveSpeed, string itemDesc)
    {
        this.type = type;
        this.rank = rank;
        this.quality = quality;
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
        quality = ItemQuality.Default;
        itemSprite = null;
        itemName = "";
        attack = 0;
        rate = 0f;
        moveSpeed = 0f;
        itemDesc = "";
    }
}
