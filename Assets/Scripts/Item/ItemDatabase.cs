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
    public void GetStaff(ItemType type, ItemRank rank, ItemQuality quality, Sprite itemSprite ,  string itemName, int attack, float rate, float moveSpeed, string itemDesc /*int[] skillNum, string[] skillDesc*/)
    {
        //Item newItem = new Item(type, rank, quality ,itemSprite, itemName, attack, rate, moveSpeed, itemDesc, skillNum,skillDesc);
        Item newItem = new Item();
        newItem.Staff(type, rank, quality, itemSprite, itemName, attack, rate, moveSpeed, itemDesc);
        itemDB.Add(newItem);
    }
    public void GetBook(ItemType type, ItemAttribute itemAttribute, ItemQuality quality , Sprite itemSprite, string bookName,int[] skillNum, string[] skillDesc)
    {
        Item newItem = new Item();
        newItem.Book(type, itemAttribute, quality, itemSprite, bookName, skillNum, skillDesc);
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