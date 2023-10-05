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
    private int skillNumber ;
    private int[] aditionalAbility  ;
    private int createType;
    private void Start()
    {

    }


    // createType은 아이템을 생성하는 형태 (0번 스태프, 1번 마법책)
    public void ItemCreate(int itemRank , int createType) // 랜덤 아이템 생성
    {
        this.createType = createType;

       if(itemRank > -1)
        {
            switch (createType) 
            {
                // 스태프
                case 0:
                case 2:
                    rank = (ItemRank)itemRank + 1;
                    type = ItemType.Staff;
                    if (rank == ItemRank.Legendary) // 레전드리는 무조건 품질 상
                    {
                        quality = ItemQuality.High;
                        SetStaffStat();
                    }
                    else
                    {
                        SetRandomValue(GameManager.instance.itemQualityPercent);
                    }
                    break;
                // 마법책
                case 1:
                case 3:
                    quality = (ItemQuality)itemRank + 1;
                    type = ItemType.Book;
                    break;
            }
        }
       else // 처음 게임 시작할 때 무기 추가 
        {
            type = ItemType.Staff;
            rank = ItemRank.Common;
            quality = ItemQuality.Low;
            SetStaffStat();
        }

 
        if (type == ItemType.Staff ) // 스태프 생성
        {
            StaffCreate();
        }
        else // 마법책 생성
        {
            BookCreate();
        }
       
    }
    public void StaffCreate()
    {
        int skillNum = -1;

        int rankNum = this.rank == ItemRank.Legendary ? 0 : -1;

       // itemAttribute = (ItemAttribute)Random.Range(1, System.Enum.GetValues(typeof(ItemAttribute)).Length + rankNum);
       if(rank == ItemRank.Legendary)
        {
            itemAttribute = (ItemAttribute)7;
        }
        else
        {
            itemAttribute = (ItemAttribute)Random.Range(1, System.Enum.GetValues(typeof(ItemAttribute)).Length + rankNum);
        }

  
        if(rank != ItemRank.Legendary)
        {
            itemName = itemInfo.staffNames[(int)itemAttribute - 1];
            desc = itemInfo.staffDescs[(int)itemAttribute - 1];
            moveSpeed = 0;
        }

        switch (rank)
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
                itemSprite = itemInfo.legendaryStaffImgaes[(int)itemAttribute - 1];
                itemName = itemInfo.legendaryStaffNames[(int)itemAttribute - 1];
                desc = itemInfo.legendaryStaffDescs[(int)itemAttribute - 1];
                skillNum = (int)itemAttribute - 1;
                break;
        }
        rewardImage.sprite = itemSprite;

        if(createType == 0)
        {
            ItemDatabase.instance.GetStaff(type, rank, quality, itemSprite, itemAttribute, itemName, attack, rate, moveSpeed, desc, skillNum);
        }
        else
        {
            Item item = new Item();
            item.Staff(type, rank, quality, itemSprite, itemAttribute, itemName, attack, rate, moveSpeed, desc, skillNum);
            GameManager.instance.shop.StaffCreate(item);
        }
    }
    private void BookCreate() // 마법 책 생성
    {
        // 스킬 번호
        skillNumber = Random.Range(7, GameManager.instance.magicManager.magicInfo.Length);

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
        if (createType == 1)
        {
            ItemDatabase.instance.GetBook(type, quality, itemSprite, itemName, skillNumber, aditionalAbility);
        }
        else
        {
            Item item = new Item();
            item.Book(type, quality, itemSprite, itemName, skillNumber, aditionalAbility);
            GameManager.instance.shop.SkillBookCreate(item);
        }
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


