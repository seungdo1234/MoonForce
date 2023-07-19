using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public enum SlotType { main , sub, space}

    public SlotType slotType;
    public Item item;

    public Image itemImage ;

    private void Awake()
    {
       Image[] images = GetComponentsInChildren<Image>();
       itemImage = images[1];

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
}
