using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopWarningText { GoldEmpty, InventoryFull, OneStaffHave, Healing, Essence , ItemSell}
public class Shop : MonoBehaviour
{
    [SerializeField]
    private List<Item> itemList;

    public ItemSlot staffSlot;
    public ItemSlot skillBookSlot;
    public ItemSlot essenceSlot;
    public ItemSlot posionSlot;
    public StageClear stageClear;
    public Text goldText;


    [Header("# WarningText")]
    public Text warningTextObject;
    public string[] warningTexts;
    public void ShopReset()
    {
        int level = GameManager.instance.level / 12;

        if(level == 0)
        {
            stageClear.RandomValue(GameManager.instance.bronzeChest, 1, 1);
        }
        else if(level == 1)
        {
            stageClear.RandomValue(GameManager.instance.silverChest, 1, 1);
        }
        else if (level >= 2)
        {
            stageClear.RandomValue(GameManager.instance.goldChest, 1, 1);
        }

        GameManager.instance.rewardManager.ItemCreate(0, 2);

        stageClear.RandomValue(GameManager.instance.shopManager.healingPosionPercent, 2, 1);

        EssenceCreate();

    }
    private void OnEnable()
    {
        warningTextObject.gameObject.SetActive(false);
    }
    public void StaffCreate(Item item)
    {
        staffSlot.item = item;
        staffSlot.ImageLoading();
        staffSlot.ItemPriceLoad();
    }
    public void SkillBookCreate(Item item)
    {
        skillBookSlot.item = item;
        skillBookSlot.ImageLoading();
        skillBookSlot.ItemPriceLoad();
    }
    private void EssenceCreate()
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
    public void PosionCreate(int posionType)
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
    public void WarningTextOn(ShopWarningText warning)
    {
        warningTextObject.gameObject.SetActive(false);

        warningTextObject.text = warningTexts[(int)warning];

        warningTextObject.gameObject.SetActive(true);
    }
    private void Update()
    {
        goldText.text = string.Format("{0}", GameManager.instance.gold);
    }
}
