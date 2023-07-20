using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    
    [SerializeField]
    private List<Item> itemDB = new List<Item>();
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }
    public void Get(ItemType type, ItemRank rank, ItemQuality quality, Sprite itemSprite ,  string itemName, int attack, float rate, float moveSpeed, string itemDesc)
    {
        Item newItem = new Item(type, rank, quality ,itemSprite, itemName, attack, rate, moveSpeed, itemDesc);
        itemDB.Add(newItem);
    }

    public Item Set(int itemNum)
    {
        return itemDB[itemNum];
    }

    public int itemCount()
    {
        return itemDB.Count;
    }

}
