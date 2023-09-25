using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Main, Sub, WaitSpace, EnchantItemSpace, EnchantWaitSpace, EnchantCheck, ShopSpace, SellSpace }
public class ItemSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public SlotType slotType;


    public Image CloneImage; // 클릭했을때 나오는 이미지
    public ItemPreview preview;
    public bool fixedPreview;
    public Transform fixedPreviewTransform;

    [Header("# Enchant")]
    public Enchant enchant;

    [Header("# Shop")]
    public Text priceText;
    public Shop_Sell sell;
    public Shop shop;
    private int itemPrice;

    [Header("# Item Info")]
    public Item item;

    public Image itemImage;


    private void Awake()
    {
        //Image[] images = GetComponentsInChildren<Image>();
        // itemImage = images[1];

        itemImage = GetComponent<Image>();

    }
    public void ImageLoading()
    {
        if (item != null && item.itemSprite != null)
        {
            itemImage.sprite = item.itemSprite;
            itemImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            itemImage.color = new Color(1, 1, 1, 0);
        }
    }

    public void OnDrag(PointerEventData eventData) // 드래그
    {
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace && slotType != SlotType.ShopSpace && slotType != SlotType.SellSpace)
        {
            // eventData.button : 플레이어가 누른 키
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (item.itemSprite != null)
                {
                    itemImage.color = new Color(1, 1, 1, 0);
                    CloneImage.gameObject.SetActive(true);
                    CloneImage.sprite = itemImage.sprite;
                    CloneImage.rectTransform.position = Input.mousePosition;

                }
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData) // 드래그가 끝날 때
    {
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace && slotType != SlotType.ShopSpace && slotType != SlotType.SellSpace)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (item.itemSprite != null)
                {
                    // 드래그한 슬롯의 이미지를 원래 슬롯의 위치로 돌려놓음
                    CloneImage.sprite = null;
                    CloneImage.gameObject.SetActive(false);

                    StartCoroutine(SelectSlot(eventData));
                }
            }
        }
    }
    
    private IEnumerator SelectSlot(PointerEventData eventData) // 드래그가 끝날 때 해당 위치에 아이템 슬롯이 있다면 실행 
    {
        yield return null;

        itemImage.color = new Color(1, 1, 1, 1);

        // 드래그한 슬롯의 이미지가 다른 슬롯 위에 있다면 교환
        ItemSlot targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>();
        // 장비를 바꾸는 과정에서 무기는 스태프, 마법책은 서브 장비칸이 맞나 확인 후 장비 교체
        if (targetSlot != null && targetSlot != this && ((item.type == ItemType.Staff && targetSlot.slotType != SlotType.Sub) || (item.type == ItemType.Book && targetSlot.slotType != SlotType.Main)))
        {

            Item tempItem = targetSlot.item;
            targetSlot.item = item;
            item = tempItem;


            // Item 데이터를 옮겻으니 해당 칸 이미지 재로딩
            targetSlot.ImageLoading();
            ImageLoading();

            if (targetSlot.slotType == SlotType.Main || slotType == SlotType.Main)
            {
                GameManager.instance.inventory.EquipStaff();
            }
            else if (targetSlot.slotType == SlotType.Sub || slotType == SlotType.Sub)
            {
                GameManager.instance.inventory.EquipBooks();
            }

        }

    }
    public void OnPointerEnter(PointerEventData eventData) // 아이템 프리뷰 창 띄우기
    {
        if (slotType != SlotType.EnchantCheck && item != null && item.itemSprite != null)
        {

            preview.gameObject.SetActive(true);

            if (!fixedPreview)
            {
                preview.gameObject.transform.position = transform.position + new Vector3(-20, 60, 0);
            }
            else
            {
                preview.gameObject.transform.position = fixedPreviewTransform.position + new Vector3(0, 60, 0);
            }

            if (slotType == SlotType.Main)
            {
                preview.gameObject.transform.position = transform.position + new Vector3(-350, 0, 0);
            }

            if (slotType == SlotType.SellSpace)
            {
                preview.IsSell(true, itemPrice);
            }
            preview.ItemInfoSet(item);
        }
    }
    public void OnPointerExit(PointerEventData eventData) // 아이템 프리뷰 창 끄기
    {
        if (slotType == SlotType.SellSpace)
        {
            preview.IsSell(false, itemPrice);
        }

        preview.gameObject.SetActive(false);
    }
    private void EnchantItemSelecet()
    {
        AudioManager.instance.SelectSfx();
        switch (slotType) // 슬롯의 형태에 따라 다른 이벤트
        {
            case SlotType.EnchantItemSpace: // 인첸트 슬롯을 눌렀을 때
                enchant.EnchantItemoff(); // 인첸트 슬롯에 있는 아이템을 해제함
                break;
            case SlotType.EnchantWaitSpace: // 대기 슬롯을 눌렀을 때
                if (enchant.itemSelect) // 인첸트 슬롯에 아이템이 있을 때
                {
                    enchant.EnchantMaterialSelect(this);
                }
                else // 인첸트 슬롯에 아이템이 없을 때
                {
                    if (enchant.EnchantItemOn(this)) // 아이템을 인첸스 슬롯에 넣음 (가능하면 true, 불가능 하면 false)
                    {
                        preview.gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    private void ItemBuy()
    {
        if (GameManager.instance.gold >= itemPrice)
        {
            if (item.type == ItemType.Book || item.type == ItemType.Staff) // 아이템 슬롯에 빈자리게 있는지 확인
            {
                int waitItemNum = 0;
                for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
                {
                    if (!ItemDatabase.instance.Set(i).isEquip)
                    {
                        waitItemNum++;
                    }
                }

                if (waitItemNum >= GameManager.instance.inventory.waitEqquipments.Length) // 빈자리가 없다면 경고 메세지와 함께 구매 X
                {
                    shop.WarningTextOn(ShopWarningText.InventoryFull);
                    AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                    return;
                }
            }
            AudioManager.instance.PlayerSfx(Sfx.BuySell); // 효과음
            GameManager.instance.gold -= itemPrice;
            switch (item.type) // 어떤 아이템을 구매 했는지 ?
            {
                case ItemType.Staff:
                    ItemDatabase.instance.GetStaff(item.type, item.rank, item.quality, item.itemSprite, item.itemAttribute, item.itemName, item.attack, item.rate, item.moveSpeed, item.itemDesc, item.skillNum);
                    break;
                case ItemType.Book:
                    ItemDatabase.instance.GetBook(item.type, item.quality, item.itemSprite, item.bookName, item.skillNum, item.aditionalAbility);
                    break;
                case ItemType.Posion:
                    shop.WarningTextOn(ShopWarningText.Healing);
                    GameManager.instance.statManager.curHealth = Mathf.Min(GameManager.instance.statManager.curHealth + item.attack, GameManager.instance.statManager.maxHealth);
                    break;
                case ItemType.Esscence:
                    shop.WarningTextOn(ShopWarningText.Essence);
                    GameManager.instance.statManager.EssenceOn(item.skillNum, item.rate);
                    break;
            }
            item = null; // 구매 아이템은 null 처리
            ImageLoading(); // 이미지 로딩
        }
        else
        {
            shop.WarningTextOn(ShopWarningText.GoldEmpty); // 돈 없음 텍스트
            AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
        }
    }
    private void ItemSell()
    {
        
        if(item.type == ItemType.Staff) // 판매 아이템이 스태프 일 경우
        {
            int staffNum = 0;
            for (int i = 0; i < ItemDatabase.instance.itemCount(); i++) // 인벤토리에 스태프가 하나 밖에 없다면 판매 X
            {
                if (ItemDatabase.instance.Set(i).type == ItemType.Staff)
                {
                    staffNum++;
                }
            }

            if (staffNum < 2) // 갯수가 하나일 때
            {
                shop.WarningTextOn(ShopWarningText.OneStaffHave);
                AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                return;
            }
        }

        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            if (ItemDatabase.instance.Set(i) == item)
            {
                ItemDatabase.instance.ItemRemove(i);
                break;
            }
        }
        item = null;
        itemImage.sprite = null;
        sell.ItemLoad();
        GameManager.instance.gold += itemPrice;
        AudioManager.instance.PlayerSfx(Sfx.BuySell);
        shop.WarningTextOn(ShopWarningText.ItemSell); // 아이템 판매 텍스트
    }
    public void OnPointerClick(PointerEventData eventData) // 인첸트 창에서 클릭이벤트
    {
        if (item != null && item.itemSprite != null && eventData.button == PointerEventData.InputButton.Left)
        {
            if (slotType == SlotType.EnchantItemSpace || slotType == SlotType.EnchantWaitSpace)
            {
                EnchantItemSelecet();
            }
            else if (slotType == SlotType.ShopSpace)
            {
                ItemBuy();
            }
            else if (slotType == SlotType.SellSpace)
            {
                ItemSell();
            }
        }

    }
    public void ItemPriceLoad() // 랜덤 상점 아이템 가격 
    {
        switch (item.type)
        {
            case ItemType.Staff:
                itemPrice = GameManager.instance.shopManager.staffRankPrices[(int)item.rank - 1];
                break;
            case ItemType.Book:
                itemPrice = GameManager.instance.shopManager.bookQualityPrices[(int)item.quality - 1];
                break;
            case ItemType.Posion:
                itemPrice = GameManager.instance.shopManager.healingPosions[(int)item.quality - 1].healingPosionPrice;
                break;
            case ItemType.Esscence:
                itemPrice = item.attack;
                break;
        }
        priceText.text = string.Format("{0}", itemPrice);
    }

    public void ItemSalePriceLoad() // 내 아이템 가격
    {
        if (item != null)
        {
            switch (item.type)
            {
                case ItemType.Staff:
                    itemPrice = GameManager.instance.shopManager.staffRankSalePrices[(int)item.rank - 1];
                    break;
                case ItemType.Book:
                    itemPrice = GameManager.instance.shopManager.bookQualitySalePrices[(int)item.quality - 1];
                    break;
            }

        }
    }
}
