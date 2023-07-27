using UnityEngine;

[System.Serializable]
public class Item 
{
    [Header("BaseInfo")]
    public ItemType type;
    public ItemRank rank;
    public ItemQuality quality;
    public Sprite itemSprite;
    public ItemAttribute itemAttribute;
    [Header("Staff")]
    public string itemName;
    public int attack;
    public float rate;
    public float moveSpeed;
    public string itemDesc;
    [Header("Book")]
    public string bookName;
    public int[] skillNum;
    public string[] skillDesc;    


    public void Staff(ItemType type, ItemRank rank, ItemQuality quality, Sprite itemSprite, string itemName, int attack, float rate, float moveSpeed, string itemDesc)
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
    public void Book(ItemType type, ItemAttribute itemAttribute, ItemQuality quality , Sprite itemSprite ,string bookName ,int[] skillNum, string[] skillDesc)
    {
        this.type = type;
        this.itemAttribute = itemAttribute;
        this.quality = quality;
        this.itemSprite = itemSprite;
        this.bookName = bookName;
        this.skillNum = skillNum;
        this.skillDesc = skillDesc;
    }
    public void reset()
    {
        type = ItemType.Default; // ItemType과 ItemRank는 각자 기본 값을 설정해주어야 합니다.
        rank = ItemRank.Default;
        quality = ItemQuality.Default;
        itemAttribute = ItemAttribute.Default;
        itemSprite = null;
        itemName = "";
        attack = 0;
        rate = 0f;
        moveSpeed = 0f;
        itemDesc = "";
        skillNum = null;
        skillDesc = null;
    }
}
