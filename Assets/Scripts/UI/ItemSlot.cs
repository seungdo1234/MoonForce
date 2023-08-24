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


    public Image CloneImage; // Ŭ�������� ������ �̹���
    public ItemPreview preview;
    public bool fixedPreview;
    public Transform fixedPreviewTransform;

    [Header("# Item Info")]
    public Item item;
    public Item prevItem;

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

    public void OnEndDrag(PointerEventData eventData)
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

    private IEnumerator SelectSlot(PointerEventData eventData)
    {
        yield return null;

        itemImage.color = new Color(1, 1, 1, 1);

        // �巡���� ������ �̹����� �ٸ� ���� ���� �ִٸ� ��ȯ
        ItemSlot targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>();
        // ��� �ٲٴ� �������� ����� ������, ����å�� ���� ���ĭ�� �³� Ȯ�� �� ��� ��ü
        if (targetSlot != null && targetSlot != this && ((item.type ==ItemType.Staff && targetSlot.slotType != SlotType.sub) || (item.type == ItemType.Book && targetSlot.slotType != SlotType.main)))
        {

            Item tempItem = targetSlot.item;
            targetSlot.item = item;
            item = tempItem;


            // Item �����͸� �Ű����� �ش� ĭ �̹��� ��ε�
            targetSlot.ImageLoading();
            ImageLoading();
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData) // ������ ������ â ����
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
            GameManager.instance.statManager.attack = GameManager.instance.statManager.baseAttack + item.attack;
            if (GameManager.instance.attribute == ItemAttribute.Dark)
            {
                GameManager.instance.statManager.rate = GameManager.instance.statManager.baseRate * 2 - item.rate;
            }
            else
            {
                GameManager.instance.statManager.rate = GameManager.instance.statManager.baseRate - item.rate;
            }
            GameManager.instance.statManager.moveSpeed = GameManager.instance.statManager.baseMoveSpeed + item.moveSpeed;
        }


        if(slotType == SlotType.sub) // ����å�� �����ϸ� ������ �߻��ϴ� ����ź�� ������ �þ
        {

            if(!isEquip && item.itemSprite != null) // ����
            {
                prevItem = item;

                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].isMagicActive = true;

                for(int i = 0; i< prevItem.aditionalAbility.Length; i++) // �߰� �ɷ�ġ ����
                {

                    switch (prevItem.aditionalAbility[i])
                    {
                        case 0:
                            GameManager.instance.magicManager.magicInfo[prevItem.skillNum].damagePer += GameManager.instance.magicManager.magicInfo[prevItem.skillNum].damageIncreaseValue;
                            break;
                        case 1:

                            if (GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicCoolTime == 0)
                            {
                                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicRateStep++;
                            }
                            else
                            {
                                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicCoolTime -= GameManager.instance.magicManager.magicInfo[prevItem.skillNum].coolTimeDecreaseValue;
                            }
                            break;
                        case 2:
                            if (GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicCountIncrease)
                            {
                                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicCount++;
                            }
                            else
                            {
                                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicSizeStep++;
                            }
                            break;
                    }
                }

                GameManager.instance.statManager.weaponNum++; // ����ź ���� ����

                isEquip = true;
            }
            else if(isEquip && item.itemSprite == null) // ����
            {

                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].isMagicActive = false;

                GameManager.instance.statManager.weaponNum--; // ����ź ���� ����

                for (int i = 0; i < prevItem.aditionalAbility.Length; i++) // �߰� �ɷ�ġ ����
                {
                    switch (prevItem.aditionalAbility[i])
                    {
                        case 0:
                            GameManager.instance.magicManager.magicInfo[prevItem.skillNum].damagePer -= GameManager.instance.magicManager.magicInfo[prevItem.skillNum].damageIncreaseValue;
                            break;
                        case 1:

                            if (GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicCoolTime == 0)
                            {
                                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicRateStep--;
                            }
                            else
                            {
                                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicCoolTime += GameManager.instance.magicManager.magicInfo[prevItem.skillNum].coolTimeDecreaseValue;
                            }
                            break;
                        case 2:
                            if (GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicCountIncrease)
                            {
                                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicCount--;
                            }
                            else
                            {
                                GameManager.instance.magicManager.magicInfo[prevItem.skillNum].magicSizeStep--;
                            }
                            break;
                    }
                }

                isEquip = false;
            }
        }

    }
}
