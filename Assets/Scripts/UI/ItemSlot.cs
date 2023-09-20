using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Main, Sub, WaitSpace , EnchantItemSpace, EnchantWaitSpace , EnchantCheck , ShopSpace}
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

    public void OnDrag(PointerEventData eventData)
    {
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace && slotType != SlotType.ShopSpace)
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace && slotType != SlotType.ShopSpace)
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

    private IEnumerator SelectSlot(PointerEventData eventData)
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
                preview.gameObject.transform.position = transform.position + new Vector3(0, 60, 0);
            }
            else
            {
                preview.gameObject.transform.position = fixedPreviewTransform.position + new Vector3(0, 60, 0);
            }
            if (slotType == SlotType.Main)
            {
                preview.gameObject.transform.position = transform.position + new Vector3(-350, 0, 0);
            }
            preview.ItemInfoSet(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        preview.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) // ��þƮ â���� Ŭ���̺�Ʈ
    {
        if (slotType == SlotType.EnchantItemSpace || slotType == SlotType.EnchantWaitSpace)
        {
            if (eventData.button == PointerEventData.InputButton.Left) // ��Ŭ�� ��
            {
                if(item != null) // �ش� ���Կ� �������� ���� ���
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
            }
        }
        else if(slotType == SlotType.ShopSpace)
        {
            if (eventData.button == PointerEventData.InputButton.Left) // ��Ŭ�� ��
            {
                if (item != null) // ���� ���Կ� �������� ���� ���
                {
                    if (GameManager.instance.gold >= itemPrice )
                    {
                        if(item.type == ItemType.Book || item.type == ItemType.Staff)
                        {
                            int waitItemNum = 0;
                            for(int i =0; i < ItemDatabase.instance.itemCount(); i++)
                            {
                                if (!ItemDatabase.instance.Set(i).isEquip)
                                {
                                    waitItemNum++;
                                }
                            }

                            if(waitItemNum >= GameManager.instance.inventory.waitEqquipments.Length)
                            {
                                AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                                return;
                            }
                        }                 
                        AudioManager.instance.PlayerSfx(Sfx.BuySell);
                        GameManager.instance.gold -= itemPrice;
                        switch (item.type)
                        {
                            case ItemType.Staff:
                                ItemDatabase.instance.GetStaff(item.type, item.rank, item.quality, item.itemSprite, item.itemAttribute, item.itemName, item.attack, item.rate, item.moveSpeed, item.itemDesc, item.skillNum);
                            break;
                            case ItemType.Book:
                                ItemDatabase.instance.GetBook(item.type, item.quality, item.itemSprite, item.bookName, item.skillNum, item.aditionalAbility);
                                break;
                            case ItemType.Posion:
                                GameManager.instance.statManager.curHealth = Mathf.Min(GameManager.instance.statManager.curHealth + item.attack, GameManager.instance.statManager.maxHealth);
                                break;
                            case ItemType.Esscence:
                                priceText.text = string.Format("{0}", item.attack);
                                break;
                        }
                        item = null;
                        ImageLoading();
                    }
                    else
                    {
                        AudioManager.instance.PlayerSfx(Sfx.BuySellFail);
                    }
              
                }

            }
        }
    }
   public void ItemPriceLoad()
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


}
