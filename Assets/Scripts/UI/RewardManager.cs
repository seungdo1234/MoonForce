using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public ItemInfo itemInfo;

    public Image rewardImage;





    private ItemType type;
    private ItemRank rank;
    private ItemQuality quality;
    private Sprite itemSprite;
    private string itemName;
    private string desc;
    private int attack;
    private float rate;
    private float moveSpeed;

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            ItemCreate(0);
        }
    }


    public void ItemCreate(int itemRank) // ���� ������ ����
    {

        type = ItemType.Staff;

        if (itemRank == 0)
        {
            SetRandomValue(GameManager.instance.itemQualityPercent);
        }

        ItemDatabase.instance.Get(type, rank, quality, itemSprite, itemName, attack, rate, moveSpeed, desc);
    }

    private void SetRandomValue(int[] percent) // ���� ��
    {
        // ���� ���� ���� �������ִ� Ȯ����� ���� ����
        int random = Random.Range(1, 101);
        int percentSum = 0;

        rank = ItemRank.Common;

        for (int i = 0; i < percent.Length; i++)
        {
            percentSum += percent[i];

            if (random <= percentSum)
            {
                quality = (ItemQuality)i + 1;

                SetStat();
                break;
            }
        }
          itemSprite = itemInfo.commonStaffImage[Random.Range(0, itemInfo.commonStaffImage.Length)];
           rewardImage.sprite = itemSprite;
           int rand = Random.Range(0, itemInfo.commonItemName.Length);
           itemName = itemInfo.commonItemName[rand];
           desc = itemInfo.commonItemDesc[rand];
           moveSpeed = 0;
    }
    private void SetStat() // Quality�� ���� Attack���� , Rate �� ����
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
}


