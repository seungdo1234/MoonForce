using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public ItemSlot mainEqquipment;
    public ItemSlot[] subEqquipments;
    public ItemSlot[] waitEqquipments;

    private Item item;

    private void OnEnable()
    {
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            item = ItemDatabase.instance.Set(i);
            waitEqquipments[i].item = item;
        }

        StartCoroutine(LoadImages());
    }


    private IEnumerator LoadImages()
    {
        // �̹��� �ε��� ���� �����ӱ��� ����
        yield return null;

        for (int i = 0; i < 15; i++)
        {
            waitEqquipments[i].ImageLoading();
        }


    }


}
