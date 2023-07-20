using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPreview : MonoBehaviour
{
    public Text itemInfomation;


    private void Awake()
    {
        itemInfomation = GetComponentInChildren<Text>();

    }

   public void ItemInfoSet(Item item)
    {
        itemInfomation.text = string.Format("{0} ({1})\n���ݷ� + {2}\n���ݼӵ� + {3}\n\n{4}", item.itemName, item.quality, item.attack, item.rate * 100, item.itemDesc);
    }
}
