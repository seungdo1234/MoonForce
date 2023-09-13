using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enchant : MonoBehaviour
{

    public ItemSlot enchantSpace;
    public ItemSlot[] waitSpaces;

    private void OnEnable()
    {
        
        

        // �������� ��� ���Կ� �ֱ�
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            waitSpaces[i].item = ItemDatabase.instance.Set(i);

        }

        StartCoroutine(LoadImages());
        
    }


    private IEnumerator LoadImages()
    {
        // �̹��� �ε��� ���� �����ӱ��� ����
        yield return null;

        enchantSpace.ImageLoading();

        for (int i = 0; i < waitSpaces.Length; i++)
        {
            waitSpaces[i].ImageLoading();
        }
    }

    public void EnchantItemOn(ItemSlot slot)
    {
        Item tempItem = slot.item;
        slot.item = enchantSpace.item;
        enchantSpace.item = tempItem;


        enchantSpace.ImageLoading();
        slot.ImageLoading();
    }

    public void EnchantItemoff()
    {

        for (int i = 0; i < waitSpaces.Length; i++)
        {
            if(waitSpaces[i].item == null)
            {
                continue;
            }

            if (waitSpaces[i].item.itemSprite == null)
            {
                Item tempItem = waitSpaces[i].item;
                waitSpaces[i].item = enchantSpace.item;
                enchantSpace.item = tempItem;

                enchantSpace.ImageLoading();
                waitSpaces[i].ImageLoading();
                break;
            }
        }
    }
    public void ItemReset()
    {
        //�ʱ�ȭ
        enchantSpace.item = null;
        enchantSpace.itemImage.sprite = null;

        for (int i = 0; i < waitSpaces.Length; i++)
        {

            if (waitSpaces[i].item == null)
            {
                break;
            }
            waitSpaces[i].item = null;
            waitSpaces[i].itemImage.sprite = null;
        }

        gameObject.SetActive(false);
    }
}
