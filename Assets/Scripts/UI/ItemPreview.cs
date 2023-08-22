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
            itemInfomation[2].text = string.Format("���ݷ� + {0}\n���ݼӵ� + {1}%\n\n{2}", item.attack, item.rate * 100, item.itemDesc);

        }
        else if(item.type == ItemType.Book)
        {
            switch (item.quality)
            {
                case ItemQuality.Low:
                    itemQuality = "�ʱ�";
                    break;
                case ItemQuality.Normal:
                    itemQuality = "�߱�";
                    break;
                case ItemQuality.High:
                    itemQuality = "���";
                    break;
            }

            itemInfomation[2].text = null;

            for (int i = 0; i < item.aditionalAbility.Length;i++)
            {
                switch (item.aditionalAbility[i])
                {
                    case 0:
                        itemInfomation[2].text += "���� ���ط� ����\n";
                        break;
                    case 1:

                        if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime == 0)
                        {
                            itemInfomation[2].text += "���� ���� �ӵ� ����\n";
                        }
                        else
                        {
                            itemInfomation[2].text += "���� ��Ÿ�� ����\n";
                        }
                            break;
                    case 2:
                        if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCountIncrease)
                        {
                            itemInfomation[2].text += "���� ��� ���� ����\n";
                        }
                        else
                        {
                            itemInfomation[2].text += "���� ũ�� ����\n";
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
