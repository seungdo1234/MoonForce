using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    public List<Item> itemList;

    public ItemSlot staffSlot;
    public ItemSlot skillBookSlot;
    public ItemSlot essenceSlot;
    public ItemSlot posionSlot;

    public void ShopReset()
    {

    }
    private void OnEnable()
    {
        GameManager.instance.rewardManager.ItemCreate(0, 1);
    }
    private void ItemExhibition()
    {

    }

    public void StaffCreate(Item item)
    {
        staffSlot.item = item;
        staffSlot.ImageLoading();
    }
    public void SkillBookCreate(Item item)
    {
        skillBookSlot.item = item;
        skillBookSlot.ImageLoading();
    }
    private void EssenceCreate()
    {

    }
    private void PosionCreate()
    {

    }
}
