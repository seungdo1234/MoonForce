using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public ItemInfo itemInfo;

    public Image rewardImage;


    private ItemType type;
    private ItemRank rank;
    private Sprite itemSprite;
    private string itemName;
    private string desc;
    private float attack;
    private float rate;
    private float moveSpeed;

    public void ItemCreate(int itemRank)
    {


        type = ItemType.Staff;

        if (itemRank == 0)
        {
            rank = ItemRank.Common;
            itemSprite = itemInfo.commonStaffImage[Random.Range(0, itemInfo.commonStaffImage.Length)];
            rewardImage.sprite = itemSprite;
            int rand = Random.Range(0, itemInfo.commonItemName.Length);
            itemName = itemInfo.commonItemName[rand];
            desc = itemInfo.commonItemDesc[rand];
            attack = 3.5f;
            rate = 0.15f;
            moveSpeed = 0;
        }

        ItemDatabase.instance.Get(type, rank, itemSprite, itemName, attack, rate, moveSpeed, desc);
    }
}


