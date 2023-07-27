using UnityEngine;
using UnityEngine.UI;

public class ItemPreview : MonoBehaviour
{
    public Text itemInfomation;

    string itemNameColor;
    string itemRankColor;
    string itemQuality;
    private void Awake()
    {
        itemInfomation = GetComponentInChildren<Text>();

    }

    public void ItemInfoSet(Item item)
    {
        if(item.type == ItemType.Staff)
        {
            switch (item.quality)
            {
                case ItemQuality.Low:
                    itemNameColor = "green";
                    break;
                case ItemQuality.Normal:
                    itemNameColor = "blue";
                    break;
                case ItemQuality.High:
                    itemNameColor = "red";
                    break;
            }

            switch (item.rank)
            {
                case ItemRank.Common:
                    itemRankColor = "black";
                    break;
                case ItemRank.Rare:
                    itemRankColor = "blue";
                    break;
                case ItemRank.Epic:
                    itemRankColor = "purple";
                    break;
                case ItemRank.Legendary:
                    itemRankColor = "yellow";
                    break;
            }


            itemInfomation.text = string.Format("<color={0}>{1}</color> (<color={2}>{3}</color>)\n공격력 + {4}\n공격속도 + {5}%\n\n{6}", itemNameColor, item.itemName, itemRankColor, item.rank, item.attack, item.rate * 100, item.itemDesc);

        }
        else if(item.type == ItemType.Book)
        {
            switch (item.itemAttribute)
            {
                case ItemAttribute.Non:
                    itemNameColor = "white";
                    break;
                case ItemAttribute.Fire:
                    itemNameColor = "red";
                    break;
                case ItemAttribute.Water:
                    itemNameColor = "blue";
                    break;
                case ItemAttribute.Eeath:
                    itemNameColor = "brown";
                    break;
                case ItemAttribute.Grass:
                    itemNameColor = "green";
                    break;
                case ItemAttribute.Dark:
                    itemNameColor = "black";
                    break;
                case ItemAttribute.Holy:
                    itemNameColor = "yellow";
                    break;
            }

            switch (item.quality)
            {
                case ItemQuality.Low:
                    itemQuality = "하급";
                    break;
                case ItemQuality.Normal:
                    itemQuality = "중급";
                    break;
                case ItemQuality.High:
                    itemQuality = "상급";
                    break;
            }

            itemInfomation.text = string.Format("<color={0}>{1}</color> ({2})\n\n", itemNameColor, item.bookName, itemQuality);

            for(int i = 0; i < item.skillDesc.Length; i++)
            {
                itemInfomation.text += string.Format("{0}\n" , item.skillDesc[i]);

            }

        }
    }
}
