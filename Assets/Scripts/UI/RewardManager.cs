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
    private int[] bookMagicNum ;
    private string[] bookMagicDesc  ;
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
  
            switch (rank)
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

            ItemDatabase.instance.GetStaff(type, rank, quality, itemSprite, itemName, attack, rate, moveSpeed, desc);
        }
        else
        {
            BookCreate();
        }
    }

    private void BookCreate() // 마법 책 생성
    {

        // 품질 별로 부여되는 마법의 갯수가 다름
        bookMagicDesc = new string[(int)quality];
        bookMagicNum = new int[(int)quality];

        if (quality !=  ItemQuality.High) // 하급, 중급 품질은 성속성이 미 포함
        {
            itemAttribute = (ItemAttribute)Random.Range(1, System.Enum.GetValues(typeof(ItemAttribute)).Length - 1);
        }
        else // 상급 품질은 성속성이 포함
        {
            itemAttribute = (ItemAttribute)Random.Range(1, System.Enum.GetValues(typeof(ItemAttribute)).Length);
        }

        // 속성별로 마법책의 이름 설정
        itemName = itemInfo.bookNames[(int)itemAttribute - 1 ];

        switch (itemAttribute) // 속성, 품질 별로 마법 책 생성
        {
            case ItemAttribute.Non: // 무 속성
                itemSprite = itemInfo.nonMagicBookSprites[Random.Range(0, itemInfo.nonMagicBookSprites.Length)];

                for(int i = 0; i< (int)quality; i++)
                {
                    switch (quality)
                    {
                        case ItemQuality.Low:
                            bookMagicNum[i] = Random.Range(0, itemInfo.nonLowMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.nonLowMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.Normal:
                            bookMagicNum[i] = Random.Range(0, itemInfo.nonNormalMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.nonNormalMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.High:
                            bookMagicNum[i] = Random.Range(0, itemInfo.nonHighMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.nonHighMagicDesc[bookMagicNum[i]];
                            break;
                    }
                }
  
                break;
            case ItemAttribute.Fire: // 화 속성
                itemSprite = itemInfo.fireMagicBookSprites[Random.Range(0, itemInfo.fireMagicBookSprites.Length)];

                for (int i = 0; i < (int)quality; i++)
                {
                    switch (quality)
                    {
                        case ItemQuality.Low:
                            bookMagicNum[i] = Random.Range(0, itemInfo.fireLowMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.fireLowMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.Normal:
                            bookMagicNum[i] = Random.Range(0, itemInfo.fireNormalMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.fireNormalMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.High:
                            bookMagicNum[i] = Random.Range(0, itemInfo.fireHighMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.fireHighMagicDesc[bookMagicNum[i]];
                            break;
                    }
                }
                break;
            case ItemAttribute.Water: // 수 속성
                itemSprite = itemInfo.waterMagicBookSprites[Random.Range(0, itemInfo.waterMagicBookSprites.Length)];

                for (int i = 0; i < (int)quality; i++)
                {
                    switch (quality)
                    {
                        case ItemQuality.Low:
                            bookMagicNum[i] = Random.Range(0, itemInfo.waterLowMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.waterLowMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.Normal:
                            bookMagicNum[i] = Random.Range(0, itemInfo.waterNormalMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.waterNormalMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.High:
                            bookMagicNum[i] = Random.Range(0, itemInfo.waterHighMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.waterHighMagicDesc[bookMagicNum[i]];
                            break;
                    }
                }

                break;
            case ItemAttribute.Eeath: // 땅 속성
                itemSprite = itemInfo.earthMagicBookSprites[Random.Range(0, itemInfo.earthMagicBookSprites.Length)];

                for (int i = 0; i < (int)quality; i++)
                {
                    switch (quality)
                    {
                        case ItemQuality.Low:
                            bookMagicNum[i] = Random.Range(0, itemInfo.earthLowMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.earthLowMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.Normal:
                            bookMagicNum[i] = Random.Range(0, itemInfo.earthNormalMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.earthNormalMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.High:
                            bookMagicNum[i] = Random.Range(0, itemInfo.earthHighMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.earthHighMagicDesc[bookMagicNum[i]];
                            break;
                    }
                }
                break;
            case ItemAttribute.Dark: // 암 속성
                itemSprite = itemInfo.darkMagicBookSprites[Random.Range(0, itemInfo.darkMagicBookSprites.Length)];

                for (int i = 0; i < (int)quality; i++)
                {
                    switch (quality)
                    {
                        case ItemQuality.Low:
                            bookMagicNum[i] = Random.Range(0, itemInfo.darkLowMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.darkLowMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.Normal:
                            bookMagicNum[i] = Random.Range(0, itemInfo.darkNormalMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.darkNormalMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.High:
                            bookMagicNum[i] = Random.Range(0, itemInfo.darkHighMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.darkHighMagicDesc[bookMagicNum[i]];
                            break;
                    }
                }

                break;
            case ItemAttribute.Grass: // 풀 속성
                itemSprite = itemInfo.grassMagicBookSprites[Random.Range(0, itemInfo.grassMagicBookSprites.Length)];
                for (int i = 0; i < (int)quality; i++)
                {
                    switch (quality)
                    {
                        case ItemQuality.Low:
                            bookMagicNum[i] = Random.Range(0, itemInfo.grassLowMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.grassLowMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.Normal:
                            bookMagicNum[i] = Random.Range(0, itemInfo.grassNormalMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.grassNormalMagicDesc[bookMagicNum[i]];
                            break;
                        case ItemQuality.High:
                            bookMagicNum[i] = Random.Range(0, itemInfo.grassHighMagicDesc.Length);
                            bookMagicDesc[i] = itemInfo.grassHighMagicDesc[bookMagicNum[i]];
                            break;
                    }
                }
                break;
            case ItemAttribute.Holy: // 성 속성
                itemSprite = itemInfo.holyMagicBookSprites[Random.Range(0, itemInfo.holyMagicBookSprites.Length)];

                bookMagicNum[0]= Random.Range(0, itemInfo.holyMagicDesc.Length);
                bookMagicDesc[0] = itemInfo.holyMagicDesc[bookMagicNum[0]];
                break;

        }
        rewardImage.sprite = itemSprite; // 보상 창에 이미지 띄움

        ItemDatabase.instance.GetBook(type, itemAttribute, quality, itemSprite, itemName, bookMagicNum, bookMagicDesc);
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


