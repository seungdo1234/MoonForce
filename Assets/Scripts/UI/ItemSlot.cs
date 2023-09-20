using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Main, Sub, WaitSpace , EnchantItemSpace, EnchantWaitSpace , EnchantCheck , ShopSpace}
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace && slotType != SlotType.ShopSpace)
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

    private IEnumerator SelectSlot(PointerEventData eventData)
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

    public void OnPointerClick(PointerEventData eventData) // 인첸트 창에서 클릭이벤트
    {
        if (slotType == SlotType.EnchantItemSpace || slotType == SlotType.EnchantWaitSpace)
        {
            if (eventData.button == PointerEventData.InputButton.Left) // 좌클릭 시
            {
                if(item != null) // 해당 슬롯에 아이템이 있을 경우
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
            }
        }
        else if(slotType == SlotType.ShopSpace)
        {
            if (eventData.button == PointerEventData.InputButton.Left) // 좌클릭 시
            {
                if (item != null) // 상점 슬롯에 아이템이 있을 경우
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
