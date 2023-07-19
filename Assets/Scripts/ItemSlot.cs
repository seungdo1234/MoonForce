using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public enum SlotType { main , sub, space}


    public SlotType slotType;
    public Image CloneImage; // 클릭했을때 나오는 이미지

    [Header("# Item Info")]
    public Item item;

    private Image itemImage ;

    private void Awake()
    {
        //Image[] images = GetComponentsInChildren<Image>();
        // itemImage = images[1];

        itemImage = GetComponent<Image>();

    }


    public void ImageLoading()
    {
        if (item.itemSprite != null)
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item.itemSprite != null)
            {
                // 드래그한 슬롯의 이미지를 원래 슬롯의 위치로 돌려놓음
                itemImage.color = new Color(1, 1, 1, 1);
                CloneImage.sprite = null;
                CloneImage.gameObject.SetActive(false);

                StartCoroutine(SelectSlot(eventData));
            }
        }
    }

    private IEnumerator SelectSlot(PointerEventData eventData)
    {
        yield return null;

        // 드래그한 슬롯의 이미지가 다른 슬롯 위에 있다면 교환
        ItemSlot targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>();
        if (targetSlot != null && targetSlot != this)
        {
            Item tempItem = targetSlot.item;
            targetSlot.item = item;
            item = tempItem;

            targetSlot.ImageLoading();
            ImageLoading();
        }
    }
}
