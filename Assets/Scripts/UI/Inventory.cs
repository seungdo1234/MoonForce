using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [Header("# Item Slot")]
    public ItemSlot mainEqquipment;
    public ItemSlot[] subEqquipments;
    public ItemSlot[] waitEqquipments;

    
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


}
