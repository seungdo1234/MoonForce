using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopWarningText { GoldEmpty, InventoryFull, OneStaffHave, Healing, Essence , ItemSell, Ready } // �˸� ����
public class Shop : MonoBehaviour
{
    private ItemSlot shopWaitSlot;

    [Header("# Buy")]
    public ItemSlot staffSlot; // ������ ����
    public ItemSlot skillBookSlot; // ����å ����
    public ItemSlot essenceSlot; // ���� ����;
    public ItemSlot posionSlot; // ���� ���� ����
    public StageClear stageClear; // ������ ���� ������Ʈ
    public Text goldText; // ���� ��� �ؽ�Ʈ
    public GameObject[] soldOutTexts; // �ȸ� ǥ��


    [Header("# WarningText")]
    public Text warningTextObject;
    public string[] warningTexts;
    public void ShopReset() // ���� �ʱ�ȭ (�� �������� Ŭ���� �� ����)
    {
        int level = GameManager.instance.level / 12;

        // ���� �� ������ ����
        if(level == 0)
        {
            stageClear.ShopItemCreate(GameManager.instance.bronzeChest, 1, 2);
        }
        else if(level == 1)
        {
            stageClear.ShopItemCreate(GameManager.instance.silverChest, 1, 2);
        }
        else if (level >= 2)
        {
            stageClear.ShopItemCreate(GameManager.instance.goldChest, 1, 2);
        }

        // ����å ����
        stageClear.ShopItemCreate(GameManager.instance.itemQualityPercent, 1 ,3);

        stageClear.ShopItemCreate(GameManager.instance.shopManager.healingPosionPercent, 2, -1);

        EssenceCreate();

        for(int i=0; i < soldOutTexts.Length; i++)
        {
            soldOutTexts[i].SetActive(false);
        }

    }
    private void OnEnable()
    {
        warningTextObject.gameObject.SetActive(false);
    }
    public void StaffCreate(Item item) // ������ ����
    {
        staffSlot.item = item;
        staffSlot.ImageLoading();
        staffSlot.ItemPriceLoad();
    }
    public void SkillBookCreate(Item item) // ����å ����
    {
        skillBookSlot.item = item;
        skillBookSlot.ImageLoading();
        skillBookSlot.ItemPriceLoad();
    }
    private void EssenceCreate() // ���� ����
    {
        int rand = Random.Range(0, GameManager.instance.shopManager.essences.Length);

        essenceSlot.item = new Item();
        essenceSlot.item.type = ItemType.Esscence;
        essenceSlot.item.skillNum = rand;
        essenceSlot.item.itemName = GameManager.instance.shopManager.essences[rand].essenceName;
        essenceSlot.item.itemSprite = GameManager.instance.shopManager.essences[rand].essenceSprite;
        essenceSlot.item.rate = GameManager.instance.shopManager.essences[rand].essenceIncreaseStat;
        essenceSlot.item.attack = GameManager.instance.shopManager.essences[rand].essencePrice;
        essenceSlot.ImageLoading();
        essenceSlot.ItemPriceLoad();
    }
    public void PosionCreate(int posionType) // ���� ���� ����
    {
        posionSlot.item = new Item();
        posionSlot.item.quality = (ItemQuality)posionType + 1;
        posionSlot.item.type = ItemType.Posion;
        posionSlot.item.itemName = GameManager.instance.shopManager.healingPosions[posionType].healingPosionName;
        posionSlot.item.itemSprite = GameManager.instance.shopManager.healingPosions[posionType].healingPosionSprite;
        posionSlot.item.attack = GameManager.instance.shopManager.healingPosions[posionType].healingPosionRecoveryHealth;

        posionSlot.ImageLoading();
        posionSlot.ItemPriceLoad();
    }
    public void WarningTextOn(ShopWarningText warning) // �˸��� �ؽ�Ʈ ����
    {
        warningTextObject.gameObject.SetActive(false);

        warningTextObject.text = warningTexts[(int)warning];

        warningTextObject.gameObject.SetActive(true);
    }
    public bool ShopReady(ItemSlot shopSlot) //  ��þƮ ������ ���� ��� ����
    {
        if (shopWaitSlot != null && shopWaitSlot == shopSlot)
        {
            shopWaitSlot = null;
            return true;
        }
        else
        {
            WarningTextOn(ShopWarningText.Ready);
            shopWaitSlot = shopSlot;
            return false;
        }
    }
    public void ShopReadyCancel()
    {
        shopWaitSlot = null;
    }
    private void Update()
    {
        // ���� ���
        goldText.text = string.Format("{0}", GameManager.instance.gold);
    }
}
