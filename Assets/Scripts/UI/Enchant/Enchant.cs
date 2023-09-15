using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Enchant : MonoBehaviour
{
    [SerializeField]
    private List<Item> itemList = new List<Item>();
    public EnchantCheckUI EnchantCheckUI;

    public GameObject[] enchantSelecetTexts;
    public ItemSlot enchantSpace = null;
    public ItemSlot[] waitSpaces = null;

    public bool itemSelect; // ��þƮ�� �������� �����ߴٸ�
 
    private void OnEnable()
    {
        itemSelect = false;
        ItemLoad();
        StartCoroutine(LoadImages());
    }

    private void ItemLoad() // �������� ��� ���Կ� ����
    {
        // �������� ��� ���Կ� �ֱ�
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            waitSpaces[i].item = ItemDatabase.instance.Set(i);

        }
    }
    private IEnumerator LoadImages() // ������ �ε�
    {
        // �̹��� �ε��� ���� �����ӱ��� ����
        yield return null;

        enchantSpace.ImageLoading();

        WaitSpaceImageLoading();
    }
    public bool EnchantItemOn(ItemSlot slot) // ��� ������ �������� ��þƮ ���Կ� �ִ� �Լ�
    {
        if (EnchantMaterial(slot.item)) // �κ��丮�� �ش� �������� ��ȭ ��ᰡ �ִٸ� true
        {
            EnchantSelectTextOn(1);
            itemSelect = true;
            enchantSpace.item = slot.item;
            enchantSpace.ImageLoading();


            WaitSpaceReset();
            EnchantMaterialLoad();
            WaitSpaceImageLoading();

            return true;
        }
        else  // �κ��丮�� �ش� �������� ��ȭ ��ᰡ ���ٸ� false
        {
            EnchantSelectTextOn(2);
            return false;
        }
  
    }
    public void EnchantItemoff() // ��þƮ ���Կ� �ִ� �������� ������
    {
        EnchantSelectTextOn(0);
        itemSelect = false;
        ItemReset();
        ItemLoad();
        StartCoroutine(LoadImages());
    }
    public void ItemReset() // ��þƮ â ����
    {
        //�ʱ�ȭ
        enchantSpace.item = null;
        enchantSpace.itemImage.sprite = null;

        WaitSpaceReset();

    }
    private void WaitSpaceReset() // ��� ���� ����
    {

        for (int i = 0; i < waitSpaces.Length; i++)
        {

            if (waitSpaces[i].item == null)
            {
                break;
            }
            waitSpaces[i].item = null;
            waitSpaces[i].itemImage.sprite = null;
        }
    }
    private void WaitSpaceImageLoading() // ��� ���� �̹��� �ε�
    {
        for (int i = 0; i < waitSpaces.Length; i++)
        {
            waitSpaces[i].ImageLoading();
        }
    }
    private bool EnchantMaterial(Item enchantItem) // ��ȭ ��Ḧ ����Ʈ�� �ֱ� (��ȭ ��ᰡ ������ false)
    {
        itemList.Clear();
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            Item item = ItemDatabase.instance.Set(i);
            if ((((enchantItem.type == ItemType.Staff && item.type == ItemType.Staff) || (enchantItem.type == ItemType.Book && item.skillNum == enchantItem.skillNum)))&& enchantItem != item)
            {
                itemList.Add(item);
            }
        }
        if(itemList.Count != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void EnchantMaterialLoad() // ����Ʈ�� �ִ� �������� ��� ���Կ� �ֱ�
    {
        WaitSpaceReset();

        // �������� ��� ���Կ� �ֱ�
        for (int i = 0; i < itemList.Count; i++)
        {
            waitSpaces[i].item = itemList[i];
        }
    }
    public void EnchantMaterialSelect(ItemSlot slot) // ��þƮ â On
    {
        EnchantCheckUI.UIOn(enchantSpace.item , slot.item);

    }
    public void EnchantSelectTextOn(int num) // ��þƮ�� �������� ���� �� ������ �ؽ�Ʈ On/Off
    {
        for(int i =0; i < enchantSelecetTexts.Length; i++)
        {
            bool active = i == num ? true : false;
            enchantSelecetTexts[i].SetActive(active);
        }
    }
}
