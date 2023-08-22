using UnityEngine;
using UnityEngine.UI;

public class ItemPreview : MonoBehaviour
{
    public Text[] itemInfomation;

    string itemNameColor;
    string itemRankColor;
    string itemQuality;
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


            itemInfomation[0].text = string.Format("<color={0}>{1}</color>", itemNameColor, item.itemName);
            itemInfomation[1].text = string.Format("(<color={0}>{1}</color>)", itemRankColor, item.rank);
            itemInfomation[2].text = string.Format("공격력 + {0}\n공격속도 + {1}%\n\n{2}", item.attack, item.rate * 100, item.itemDesc);

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

            itemInfomation[2].text = null;

            for (int i = 0; i < item.aditionalAbility.Length;i++)
            {
                switch (item.aditionalAbility[i])
                {
                    case 0:
                        itemInfomation[2].text += "마법 피해량 증가\n";
                        break;
                    case 1:

                        if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime == 0)
                        {
                            itemInfomation[2].text += "마법 공격 속도 증가\n";
                        }
                        else
                        {
                            itemInfomation[2].text += "마법 쿨타임 감소\n";
                        }
                            break;
                    case 2:
                        if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCountIncrease)
                        {
                            itemInfomation[2].text += "마법 출력 갯수 증가\n";
                        }
                        else
                        {
                            itemInfomation[2].text += "마법 크기 증가\n";
                        }
                        break;
                }
            }


            itemInfomation[0].text = string.Format("<color=black>{0}</color>", item.bookName);
            itemInfomation[1].text = itemQuality;



            /*
                        for(int i = 0; i < item..Length; i++)
                        {
                            itemInfomation.text += string.Format("{0}\n" , item.skillDesc[i]);

                        }
            */

        }
    }
}
