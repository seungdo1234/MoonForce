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

    public bool itemSelect; // 인첸트할 아이템을 선택했다면
 
    private void OnEnable()
    {
        itemSelect = false;
        ItemLoad();
        StartCoroutine(LoadImages());
    }

    private void ItemLoad() // 아이템을 대기 슬롯에 넣음
    {
        // 아이템을 대기 슬롯에 넣기
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            waitSpaces[i].item = ItemDatabase.instance.Set(i);

        }
    }
    private IEnumerator LoadImages() // 아이템 로드
    {
        // 이미지 로딩을 다음 프레임까지 연기
        yield return null;

        enchantSpace.ImageLoading();

        WaitSpaceImageLoading();
    }
    public bool EnchantItemOn(ItemSlot slot) // 대기 슬롯의 아이템을 인첸트 슬롯에 넣는 함수
    {
        if (EnchantMaterial(slot.item)) // 인벤토리에 해당 아이템의 강화 재료가 있다면 true
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
        else  // 인벤토리에 해당 아이템의 강화 재료가 없다면 false
        {
            EnchantSelectTextOn(2);
            return false;
        }
  
    }
    public void EnchantItemoff() // 인첸트 슬롯에 있는 아이템을 해제함
    {
        EnchantSelectTextOn(0);
        itemSelect = false;
        ItemReset();
        ItemLoad();
        StartCoroutine(LoadImages());
    }
    public void ItemReset() // 인첸트 창 리셋
    {
        //초기화
        enchantSpace.item = null;
        enchantSpace.itemImage.sprite = null;

        WaitSpaceReset();

    }
    private void WaitSpaceReset() // 대기 슬롯 리셋
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
    private void WaitSpaceImageLoading() // 대기 슬롯 이미지 로딩
    {
        for (int i = 0; i < waitSpaces.Length; i++)
        {
            waitSpaces[i].ImageLoading();
        }
    }
    private bool EnchantMaterial(Item enchantItem) // 강화 재료를 리스트에 넣기 (강화 재료가 없을시 false)
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
    private void EnchantMaterialLoad() // 리스트에 있는 아이템을 대기 슬롯에 넣기
    {
        WaitSpaceReset();

        // 아이템을 대기 슬롯에 넣기
        for (int i = 0; i < itemList.Count; i++)
        {
            waitSpaces[i].item = itemList[i];
        }
    }
    public void EnchantMaterialSelect(ItemSlot slot) // 인첸트 창 On
    {
        EnchantCheckUI.UIOn(enchantSpace.item , slot.item);

    }
    public void EnchantSelectTextOn(int num) // 인첸트할 아이템을 누를 때 나오는 텍스트 On/Off
    {
        for(int i =0; i < enchantSelecetTexts.Length; i++)
        {
            bool active = i == num ? true : false;
            enchantSelecetTexts[i].SetActive(active);
        }
    }
}
