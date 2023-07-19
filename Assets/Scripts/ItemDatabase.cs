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
    public void Get(ItemType type, ItemRank rank, Sprite itemSprite, string itemName, float attack, float rate, float moveSpeed, string itemDesc)
    {
        Item newItem = new Item(type, rank, itemSprite, itemName, attack, rate, moveSpeed, itemDesc);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < itemDB.Count; i++)
            {
                Debug.Log(itemDB[i].itemSprite.name);
            }
        }
    }
}
