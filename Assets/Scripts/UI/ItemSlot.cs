using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Main, Sub, WaitSpace , EnchantItemSpace, EnchantWaitSpace }
public class ItemSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public SlotType slotType;


    public Image CloneImage; // 클릭했을때 나오는 이미지
    public ItemPreview preview;
    public bool fixedPreview;
    public Transform fixedPreviewTransform;

    [Header("# Enchant")]
    public Enchant enchant;

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
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace)
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
        if (slotType != SlotType.EnchantItemSpace && slotType != SlotType.EnchantWaitSpace)
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
        if (item != null && item.itemSprite != null)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotType == SlotType.EnchantItemSpace || slotType == SlotType.EnchantWaitSpace)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if(item.itemSprite != null)
                {
                    switch (slotType)
                    {
                        case SlotType.EnchantItemSpace:
                            enchant.EnchantItemoff();
                            break;
                        case SlotType.EnchantWaitSpace:
                            enchant.EnchantItemOn(this);
                            break;
                    }

                }
            }
        }
    }
}
