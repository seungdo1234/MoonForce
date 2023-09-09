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


    public void ItemCreate(int itemRank) // 랜덤 아이템 생성
    {
       
        type = (ItemType)Random.Range(1, 3);


        rank = (ItemRank)itemRank + 1;

        SetRandomValue(GameManager.instance.itemQualityPercent);
 
        if (type == ItemType.Staff)
        {
            itemAttribute = (ItemAttribute)Random.Range(1, System.Enum.GetValues(typeof(ItemAttribute)).Length - 1);

            itemName = itemInfo.staffNames[(int)itemAttribute - 1];
            desc = itemInfo.staffDescs[(int)itemAttribute - 1];
            moveSpeed = 0;

            switch  (rank)
            {
                case ItemRank.Common:
                    itemSprite = itemInfo.commonStaffImgaes[(int)itemAttribute - 1];
                    break;
                case ItemRank.Rare:
                    itemSprite = itemInfo.rareStaffImgaes[(int)itemAttribute - 1];
                    break;
                case ItemRank.Epic:
                    itemSprite = itemInfo.epicStaffImgaes[(int)itemAttribute - 1];
                    break;
                case ItemRank.Legendary:
                    itemSprite = itemInfo.legendaryStaffImgaes[(int)itemAttribute - 2];
                    itemName = itemInfo.legendaryStaffNames[(int)itemAttribute - 2];
                    desc = itemInfo.legendaryStaffDescs[(int)itemAttribute - 2];
                    break;
            }
            rewardImage.sprite = itemSprite;

            ItemDatabase.instance.GetStaff(type, rank, quality, itemSprite, itemAttribute, itemName, attack, rate, moveSpeed, desc);
        }
        else
        {
            BookCreate();
        }
       
    }

    private void BookCreate() // 마법 책 생성
    {
        // 스킬 번호
        skillNumber = Random.Range(1, GameManager.instance.magicManager.magicInfo.Length);

        // 품질 별로 부여되는 마법의 갯수가 다름
        aditionalAbility = new int[(int)quality];
        
        for(int i = 0; i< aditionalAbility.Length; i++)
        {
            int rand = Random.Range(0, itemInfo.aditionalAbility.Length);
            
            aditionalAbility[i]  = rand;
        }


        itemName = GameManager.instance.magicManager.magicInfo[skillNumber].magicName;
        itemSprite = itemInfo.magicBookSprite;

      
        rewardImage.sprite = itemSprite; // 보상 창에 이미지 띄움

        ItemDatabase.instance.GetBook(type, quality, itemSprite, itemName, skillNumber, aditionalAbility);
    }
    private void SetRandomValue(int[] percent) // 랜덤 값
    {
        // 랜덤 값을 구해 정해져있는 확률대로 상자 등장
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
    private void SetStaffStat() // Quality에 따라 Attack값과 , Rate 값 대입
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


