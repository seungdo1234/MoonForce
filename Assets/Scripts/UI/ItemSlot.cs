using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Main, Sub, WaitSpace, EnchantItemSpace, EnchantWaitSpace, EnchantCheck, ShopSpace, SellSpace }
public class ItemSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public SlotType slotType;


    public Image CloneImage; // Ŭ�������� ������ �̹���
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

    public void OnDrag(PointerEventData eventData) // �巡��
    {
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace && slotType != SlotType.ShopSpace && slotType != SlotType.SellSpace)
        {
            // eventData.button : �÷��̾ ���� Ű
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
    public void OnEndDrag(PointerEventData eventData) // �巡�װ� ���� ��
    {
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace && slotType != SlotType.ShopSpace && slotType != SlotType.SellSpace)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (item.itemSprite != null)
                {
                    // �巡���� ������ �̹����� ���� ������ ��ġ�� ��������
                    CloneImage.sprite = null;
                    CloneImage.gameObject.SetActive(false);

                    StartCoroutine(SelectSlot(eventData));
                }
            }
        }
    }
    
    private IEnumerator SelectSlot(PointerEventData eventData) // �巡�װ� ���� �� �ش� ��ġ�� ������ ������ �ִٸ� ���� 
    {
        yield return null;

        itemImage.color = new Color(1, 1, 1, 1);

        // �巡���� ������ �̹����� �ٸ� ���� ���� �ִٸ� ��ȯ
        ItemSlot targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>();
        // ��� �ٲٴ� �������� ����� ������, ����å�� ���� ���ĭ�� �³� Ȯ�� �� ��� ��ü
        if (targetSlot != null && targetSlot != this && ((item.type == ItemType.Staff && targetSlot.slotType != SlotType.Sub) || (item.type == ItemType.Book && targetSlot.slotType != SlotType.Main)))
        {

            Item tempItem = targetSlot.item;
            targetSlot.item = item;
            item = tempItem;


            // Item �����͸� �Ű����� �ش� ĭ �̹��� ��ε�
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
    public void OnPointerEnter(PointerEventData eventData) // ������ ������ â ����
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
    public void OnPointerExit(PointerEventData eventData) // ������ ������ â ����
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
        switch (slotType) // ������ ���¿� ���� �ٸ� �̺�Ʈ
        {
            case SlotType.EnchantItemSpace: // ��þƮ ������ ������ ��
                enchant.EnchantItemoff(); // ��þƮ ���Կ� �ִ� �������� ������
                break;
            case SlotType.EnchantWaitSpace: // ��� ������ ������ ��
                if (enchant.itemSelect) // ��þƮ ���Կ� �������� ���� ��
                {
                    enchant.EnchantMaterialSelect(this);
                }
                else // ��þƮ ���Կ� �������� ���� ��
                {
                    if (enchant.EnchantItemOn(this)) // �������� ��þ�� ���Կ� ���� (�����ϸ� true, �Ұ��� �ϸ� false)
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
            if (item.type == ItemType.Book || item.type == ItemType.Staff) // ������ ���Կ� ���ڸ��� �ִ��� Ȯ��
            {
                int waitItemNum = 0;
                for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
                {
                    if (!ItemDatabase.instance.Set(i).isEquip)
                    {
                        waitItemNum++;
                    }
                }

                if (waitItemNum >= GameManager.instance.inventory.waitEqquipments.Length) // ���ڸ��� ���ٸ� ��� �޼����� �Բ� ���� X
                {
                    shop.WarningTextOn(ShopWarningText.InventoryFull);
                    AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                    return;
                }
            }
            AudioManager.instance.PlayerSfx(Sfx.BuySell); // ȿ����
            GameManager.instance.gold -= itemPrice;
            switch (item.type) // � �������� ���� �ߴ��� ?
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
            item = null; // ���� �������� null ó��
            ImageLoading(); // �̹��� �ε�
        }
        else
        {
            shop.WarningTextOn(ShopWarningText.GoldEmpty); // �� ���� �ؽ�Ʈ
            AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
        }
    }
    private void ItemSell()
    {
        
        if(item.type == ItemType.Staff) // �Ǹ� �������� ������ �� ���
        {
            int staffNum = 0;
            for (int i = 0; i < ItemDatabase.instance.itemCount(); i++) // �κ��丮�� �������� �ϳ� �ۿ� ���ٸ� �Ǹ� X
            {
                if (ItemDatabase.instance.Set(i).type == ItemType.Staff)
                {
                    staffNum++;
                }
            }

            if (staffNum < 2) // ������ �ϳ��� ��
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
        shop.WarningTextOn(ShopWarningText.ItemSell); // ������ �Ǹ� �ؽ�Ʈ
    }
    public void OnPointerClick(PointerEventData eventData) // ��þƮ â���� Ŭ���̺�Ʈ
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
    public void ItemPriceLoad() // ���� ���� ������ ���� 
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

    public void ItemSalePriceLoad() // �� ������ ����
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
