using UnityEngine;
using UnityEngine.UI;

public enum ItemPreviewType { Inventory , EnchantCheck}
public class ItemPreview : MonoBehaviour
{
    public ItemPreviewType type;
    public Text[] itemInfomation;

    string itemNameColor;
    string itemRankColor;
    string itemQuality;
    string itemAttributeColor;
    string itemAttribute;
    private void Awake()
    {
        itemInfomation = GetComponentsInChildren<Text>();

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

            switch (item.itemAttribute)
            {
                case ItemAttribute.Non:
                    itemAttributeColor = "white";
                    itemAttribute = "무 속성";
                    break;
                case ItemAttribute.Fire:
                    itemAttributeColor = "orange";
                    itemAttribute = "불 속성";
                    break;
                case ItemAttribute.Water:
                    itemAttributeColor = "blue";
                    itemAttribute = "수 속성";
                    break;
                case ItemAttribute.Eeath:
                    itemAttributeColor = "brown";
                    itemAttribute = "땅 속성";
                    break;
                case ItemAttribute.Grass:
                    itemAttributeColor = "green";
                    itemAttribute = "풀 속성";
                    break;
                case ItemAttribute.Dark:
                    itemAttributeColor = "black";
                    itemAttribute = "어둠 속성";
                    break;
                case ItemAttribute.Holy:
                    itemAttributeColor = "yellow";
                    itemAttribute = "빛 속성";
                    break;

            }
            itemInfomation[0].text = string.Format("<color={0}>{1}</color>", itemNameColor, item.itemName);
            itemInfomation[1].text = string.Format("(<color={0}>{1}</color>)", itemRankColor, item.rank);
            itemInfomation[2].text = string.Format("<color={0}>{1}</color>",itemAttributeColor,itemAttribute);
            if(type == ItemPreviewType.Inventory)
            {
                itemInfomation[3].text = string.Format("공격력 + {0}\n공격속도 + {1}%\n{2}", item.attack, Mathf.Floor(item.rate * 100), item.itemDesc);
            }
            else
            {
                itemInfomation[3].text = string.Format("공격력 + {0}\n공격속도 + {1}%", item.attack, Mathf.Floor(item.rate * 100));
            }

        }
        else if(item.type == ItemType.Book)
        {
            switch (item.quality)
            {
                case ItemQuality.Low:
                    itemQuality = "초급";
                    break;
                case ItemQuality.Normal:
                    itemQuality = "중급";
                    break;
                case ItemQuality.High:
                    itemQuality = "상급";
                    break;
            }

            itemInfomation[3].text = null;
            itemInfomation[2].text = null;

            for (int i = 0; i < item.aditionalAbility.Length;i++)
            {
                if (i > 0)
                {
                    itemInfomation[3].text +="\n";
                }
                switch (item.aditionalAbility[i])
                {
                    case 0:
                        itemInfomation[3].text += "마법 피해량 증가";
                        break;
                    case 1:

                        if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime == 0)
                        {
                            itemInfomation[3].text += "마법 공격 속도 증가";
                        }
                        else
                        {
                            itemInfomation[3].text += "마법 쿨타임 감소";
                        }
                            break;
                    case 2:
                        if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCountIncrease)
                        {
                            itemInfomation[3].text += "마법 출력 갯수 증가";
                        }
                        else
                        {
                            itemInfomation[3].text += "마법 크기 증가";
                        }
                        break;
                }
            }


            itemInfomation[0].text = string.Format("<color=black>{0}</color>", item.bookName);
            itemInfomation[1].text = itemQuality;

        }
    }
}
