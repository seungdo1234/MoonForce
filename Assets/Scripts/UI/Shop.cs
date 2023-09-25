using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopWarningText { GoldEmpty, InventoryFull, OneStaffHave, Healing, Essence , ItemSell}
public class Shop : MonoBehaviour
{

    public ItemSlot staffSlot;
    public ItemSlot skillBookSlot;
    public ItemSlot essenceSlot;
    public ItemSlot posionSlot;
    public StageClear stageClear;
    public Text goldText;


    [Header("# WarningText")]
    public Text warningTextObject;
    public string[] warningTexts;
    public void ShopReset() // 상점 초기화 (매 스테이지 클리어 시 실행)
    {
        int level = GameManager.instance.level / 12;

        // 레벨 별 스태프 생성
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

        // 마법책 생성
        stageClear.ShopItemCreate(GameManager.instance.itemQualityPercent, 1 ,3);

        stageClear.ShopItemCreate(GameManager.instance.shopManager.healingPosionPercent, 2, -1);

        EssenceCreate();

    }
    private void OnEnable()
    {
        warningTextObject.gameObject.SetActive(false);
    }
    public void StaffCreate(Item item) // 스태프 생성
    {
        staffSlot.item = item;
        staffSlot.ImageLoading();
        staffSlot.ItemPriceLoad();
    }
    public void SkillBookCreate(Item item) // 마법책 생성
    {
        skillBookSlot.item = item;
        skillBookSlot.ImageLoading();
        skillBookSlot.ItemPriceLoad();
    }
    private void EssenceCreate() // 정수 생성
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
    public void PosionCreate(int posionType) // 힐링 포션 생성
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
    public void WarningTextOn(ShopWarningText warning) // 알림문 텍스트 생성
    {
        warningTextObject.gameObject.SetActive(false);

        warningTextObject.text = warningTexts[(int)warning];

        warningTextObject.gameObject.SetActive(true);
    }
    private void Update()
    {
        // 현재 골드
        goldText.text = string.Format("{0}", GameManager.instance.gold);
    }
}
