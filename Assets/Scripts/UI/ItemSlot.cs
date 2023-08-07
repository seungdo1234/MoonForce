using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotType { main, sub, space }
public class ItemSlot : MonoBehaviour, IDragHandler, IEndDragHandler , IPointerEnterHandler, IPointerExitHandler
{

    public SlotType slotType;

    public bool isEquip;


    public Image CloneImage; // 클릭했을때 나오는 이미지
    public ItemPreview preview;
    public bool fixedPreview;
    public Transform fixedPreviewTransform;

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
                CloneImage.sprite = null;
                CloneImage.gameObject.SetActive(false);

                StartCoroutine(SelectSlot(eventData));
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
        if (targetSlot != null && targetSlot != this && ((item.type ==ItemType.Staff && targetSlot.slotType != SlotType.sub) || (item.type == ItemType.Book && targetSlot.slotType != SlotType.main)))
        {

            Item tempItem = targetSlot.item;
            targetSlot.item = item;
            item = tempItem;


            // Item 데이터를 옮겻으니 해당 칸 이미지 재로딩
            targetSlot.ImageLoading();
            ImageLoading();
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData) // 아이템 프리뷰 창 띄우기
    {
        if (item.itemSprite != null)
        {

            preview.gameObject.SetActive(true);

            if(!fixedPreview)
            {
                preview.gameObject.transform.position = transform.position + new Vector3(0, 60, 0);
            }
            else
            {
                preview.gameObject.transform.position = fixedPreviewTransform.position + new Vector3(0, 60, 0);
            }
            if(slotType == SlotType.main)
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

    private void Update()
    {

        if( slotType == SlotType.main)
        {
            GameManager.instance.attribute = item.itemAttribute;
            GameManager.instance.attack = GameManager.instance.baseAttack + item.attack;
            if (GameManager.instance.attribute == ItemAttribute.Dark)
            {
                GameManager.instance.rate = GameManager.instance.baseRate * 2 - item.rate;
            }
            else
            {
                GameManager.instance.rate = GameManager.instance.baseRate - item.rate;
            }
            GameManager.instance.moveSpeed = GameManager.instance.baseMoveSpeed + item.moveSpeed;
        }


        if(slotType == SlotType.sub) // 마법책을 장착하면 적에게 발사하는 마력탄의 갯수가 늘어남
        {

            if(!isEquip && item.itemSprite != null)
            {
                if(GameManager.instance.attribute != ItemAttribute.Dark)
                {
                    GameManager.instance.weaponNum++;
                }
                else // 암속성일 경우는 장착한 마법책의 갯수 만큼 폭발의 범위가 늘어남
                {
                    GameManager.instance.magicManager.darknessExplosion.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);
                }
                isEquip = true;
            }
            else if(isEquip && item.itemSprite == null)
            {
                if (GameManager.instance.attribute != ItemAttribute.Dark)
                {
                    GameManager.instance.weaponNum--;
                }
                else
                {
                    GameManager.instance.magicManager.darknessExplosion.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
                }
                isEquip = false;
            }
        }

    }
}
