using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RewardManager : MonoBehaviour
{
    public ItemInfo itemInfo;

    public Image rewardImage;





    private ItemType type;
    private ItemRank rank;
    private ItemQuality quality;
    private Sprite itemSprite;
    private ItemAttribute itemAttribute;
    private string itemName;
    private string desc;
    private int attack;
    private float rate;
    private float moveSpeed;
    private int rand;
    private int skillNumber ;
    private int[] aditionalAbility  ;
    private void Start()
    {

    }


    public void ItemCreate(int itemRank) // ���� ������ ����
    {

        type = (ItemType)Random.Range(1, 3);


        rank = (ItemRank)itemRank + 1;

        SetRandomValue(GameManager.instance.itemQualityPercent);

        if (type == ItemType.Staff)
        {
            itemAttribute = (ItemAttribute)Random.Range(1, System.Enum.GetValues(typeof(ItemAttribute)).Length - 1);
            switch  (rank)
            {
                case ItemRank.Common:
                    itemSprite = itemInfo.commonImage[Random.Range(0, itemInfo.commonImage.Length)];
                    rewardImage.sprite = itemSprite;
                    rand = Random.Range(0, itemInfo.commonItemName.Length);
                    itemName = itemInfo.commonItemName[rand];
                    desc = itemInfo.commonItemDesc[rand];
                    moveSpeed = 0;
                    break;
                case ItemRank.Rare:
                    itemSprite = itemInfo.rareImage[Random.Range(0, itemInfo.rareImage.Length)];
                    rewardImage.sprite = itemSprite;
                    rand = Random.Range(0, itemInfo.rareItemName.Length);
                    itemName = itemInfo.rareItemName[rand];
                    desc = itemInfo.rareItemDesc[rand];
                    moveSpeed = 0;
                    break;
                case ItemRank.Epic:
                    itemSprite = itemInfo.epicImage[Random.Range(0, itemInfo.epicImage.Length)];
                    rewardImage.sprite = itemSprite;
                    rand = Random.Range(0, itemInfo.epicItemName.Length);
                    itemName = itemInfo.epicItemName[rand];
                    desc = itemInfo.epicItemDesc[rand];
                    moveSpeed = 0;
                    break;
                case ItemRank.Legendary:
                    break;
            }

            ItemDatabase.instance.GetStaff(type, rank, quality, itemSprite, itemAttribute, itemName, attack, rate, moveSpeed, desc);
        }
        else
        {
            BookCreate();
        }
    }

    private void BookCreate() // ���� å ����
    {
        // ��ų ��ȣ
        skillNumber = Random.Range(1, GameManager.instance.magicManager.magicInfo.Length);

        // ǰ�� ���� �ο��Ǵ� ������ ������ �ٸ�
        aditionalAbility = new int[(int)quality];
        
        for(int i = 0; i< aditionalAbility.Length; i++)
        {
            int rand = Random.Range(0, itemInfo.aditionalAbility.Length);
            
            aditionalAbility[i]  = rand;
        }


        itemName = GameManager.instance.magicManager.magicInfo[skillNumber].magicName;
        itemSprite = itemInfo.magicBookSprite;

      
        rewardImage.sprite = itemSprite; // ���� â�� �̹��� ���

        ItemDatabase.instance.GetBook(type, quality, itemSprite, itemName, skillNumber, aditionalAbility);
    }
    private void SetRandomValue(int[] percent) // ���� ��
    {
        // ���� ���� ���� �������ִ� Ȯ����� ���� ����
        int random = Random.Range(1, 101);
        int percentSum = 0;

        for (int i = 0; i < percent.Length; i++)
        {
            percentSum += percent[i];

            if (random <= percentSum)
            {
                quality = (ItemQuality)i + 1;

                if(type == ItemType.Staff)
                {
                    SetStaffStat();
                }
                break;
            }
        }

    }
    private void SetStaffStat() // Quality�� ���� Attack���� , Rate �� ����
    {
        if (quality == ItemQuality.Low)
        {
            attack = itemInfo.baseAttack[(int)rank - 1];
            rate = itemInfo.baseRate[(int)rank - 1];
        }
        else if (quality == ItemQuality.Normal)
        {
            attack = itemInfo.baseAttack[(int)rank - 1] + itemInfo.increaseAttack[(int)rank - 1];
            rate = itemInfo.baseRate[(int)rank - 1] + itemInfo.increaseRate[(int)rank - 1];
        }
        else if (quality == ItemQuality.High)
        {
            attack = itemInfo.baseAttack[(int)rank - 1] + itemInfo.increaseAttack[(int)rank - 1] * 2;
            rate = itemInfo.baseRate[(int)rank - 1] + itemInfo.increaseRate[(int)rank - 1] * 2;
        }
    }

    private void SetBookStat()
    {

    }
}


