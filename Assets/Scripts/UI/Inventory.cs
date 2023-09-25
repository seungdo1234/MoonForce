using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [Header("# Item Slot")]
    public ItemSlot mainEqquipment;
    public ItemSlot[] subEqquipments;
    public ItemSlot[] waitEqquipments;

    [Header("# Skill Books")]
    public Item prevEquipStaff ; // ���� ������ ������
    [SerializeField]
    private List<Item> equipBooks = new List<Item>(); // ���� ������ ����å�� ���ִ� ����Ʈ
    private Item item;

    private void OnEnable()
    {
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            item = ItemDatabase.instance.Set(i);
            if(!item.isLoad) // �̹� �κ��丮�� �ε�� �������̶�� �ε尡 �ȵǰ� ��
            {
                item.isLoad = true;
                for(int j = 0; j <waitEqquipments.Length; j++) // �������� �� ������ ������ ã�� ���� 
                {
                    if(waitEqquipments[j].item.itemSprite == null)
                    {
                        waitEqquipments[j].item = item;
                        break;
                    }
                }
            }
        }

        StartCoroutine(LoadImages());
    }


    private IEnumerator LoadImages()
    {
        // �̹��� �ε��� ���� �����ӱ��� ����
        yield return null;

        for (int i = 0; i < waitEqquipments.Length; i++)
        {
            waitEqquipments[i].ImageLoading();
        }


    }

    public void InventoryReset()
    {
        if (mainEqquipment.item.itemSprite != null)
        {
            mainEqquipment.item.reset();
            mainEqquipment.ImageLoading();
            EquipStaff();
        }

        for (int i = 0; i < GameManager.instance.inventory.subEqquipments.Length; i++)
        {
            if (subEqquipments[i].item.itemSprite != null)
            {
                GameManager.instance.magicManager.magicInfo[subEqquipments[i].item.skillNum].isMagicActive = false;
                subEqquipments[i].item.reset();
                subEqquipments[i].ImageLoading();
            }
        }


        for (int i = 0; i < waitEqquipments.Length; i++)
        {
            if (waitEqquipments[i].item.itemSprite != null)
            {
                waitEqquipments[i].item.reset();
                waitEqquipments[i].ImageLoading();
            }
        }
    }
    public void SkillBookInit()
    {
        for (int i = 0; i < equipBooks.Count; i++) // ���� ������ ���� å ��ų �ʱ�ȭ
        {
            GameManager.instance.magicManager.magicInfo[equipBooks[i].skillNum].isMagicActive = false;
            equipBooks[i].isEquip = false;
            MagicAdditionalStat(equipBooks[i], -1);
        }
    }
    public void SkillBookActive()
    {

        equipBooks.Clear(); // ����Ʈ �ʱ�ȭ

        for (int i = 0; i < subEqquipments.Length; i++)
        {
            if (subEqquipments[i].item.itemSprite != null) // ������ ���� å ���� Ȱ��ȭ
            {
                GameManager.instance.magicManager.magicInfo[subEqquipments[i].item.skillNum].isMagicActive = true;
                MagicAdditionalStat(subEqquipments[i].item, 1);
                equipBooks.Add(subEqquipments[i].item);
                subEqquipments[i].item.isEquip = true;
            }
        }
    }
    public void EquipBooks() // ���� ����å ����
    {
        SkillBookInit();

        SkillBookActive();
    }

    private void MagicAdditionalStat(Item item , int operation) // ����å �߰� �ɷ� ����
    {

        for (int i = 0; i < item.aditionalAbility.Length; i++) 
        {

            switch (item.aditionalAbility[i])
            {
                case 0:
                    GameManager.instance.magicManager.magicInfo[item.skillNum].damagePer += GameManager.instance.magicManager.magicInfo[item.skillNum].damageIncreaseValue * operation;
                    break;
                case 1:

                    if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime == 0)
                    {
                        GameManager.instance.magicManager.magicInfo[item.skillNum].magicRateStep+= operation;
                    }
                    else
                    {
                        GameManager.instance.magicManager.magicInfo[item.skillNum].magicCoolTime -= GameManager.instance.magicManager.magicInfo[item.skillNum].coolTimeDecreaseValue * operation;
                    }
                    break;
                case 2:
                    if (GameManager.instance.magicManager.magicInfo[item.skillNum].magicCountIncrease)
                    {
                        GameManager.instance.magicManager.magicInfo[item.skillNum].magicCount+= operation;
                    }
                    else
                    {
                        GameManager.instance.magicManager.magicInfo[item.skillNum].magicSizeStep+= operation;
                    }
                    break;
            }
        }
        GameManager.instance.statManager.weaponNum+= operation ;
    }
    public void EquipStaff()
    {
        mainEqquipment.item.isEquip = true;
        prevEquipStaff.isEquip = false;
        // �ɷ�ġ ����
        GameManager.instance.attribute = mainEqquipment.item.itemAttribute;
        GameManager.instance.statManager.attack = GameManager.instance.statManager.baseAttack + mainEqquipment.item.attack;
        if (GameManager.instance.attribute == ItemAttribute.Dark)
        {
            GameManager.instance.statManager.rate = GameManager.instance.statManager.baseRate * 2 - mainEqquipment.item.rate;
        }
        else
        {
            GameManager.instance.statManager.rate = GameManager.instance.statManager.baseRate - mainEqquipment.item.rate;
        }
        GameManager.instance.statManager.moveSpeed = GameManager.instance.statManager.baseMoveSpeed + mainEqquipment.item.moveSpeed;

        if (mainEqquipment.item.rank == ItemRank.Legendary) // ������ �������� �����帮 �������̶�� ��ų On
        {
            GameManager.instance.magicManager.magicInfo[mainEqquipment.item.skillNum].isMagicActive = true;
        }

        if (prevEquipStaff.rank == ItemRank.Legendary) // ���������� �������� ������ ������ �̶�� �ش� ��ų Off
        {
            GameManager.instance.magicManager.magicInfo[prevEquipStaff.skillNum].isMagicActive = false;
        }

        prevEquipStaff = mainEqquipment.item;    // ������ ������ �����͸� ����
    }
}
