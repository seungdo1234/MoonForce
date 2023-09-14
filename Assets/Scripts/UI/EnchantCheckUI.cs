using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantCheckUI : MonoBehaviour
{
    public Enchant enchant;
    public ItemPreview[] itemPreview;
    public Text enchantPercentText;
    public Button enchantStartBtn;
    private ItemSlot[] itemSlots;
    private int enchantPercent;

    [Header("Skill Book")]
    public AditionalSelectBtn[] aditionalSelectBtn;
    public int selectEnchantAditionalNum;
    public int selectMaterialAditionalNum;
    private void Awake()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
    }
    private void SkillBookEnchantInit()
    {
        selectEnchantAditionalNum = -1;
        selectMaterialAditionalNum = -1;
    }
    public void UIOn(Item enchant_Item , Item material_Item)
    {
        gameObject.SetActive(true);

        itemSlots[0].item = enchant_Item;
        itemSlots[1].item = material_Item;

        for(int i =0; i < itemSlots.Length; i++) // 이미지, 아이템 정보 로딩
        {
            itemSlots[i].ImageLoading();
            itemPreview[i].ItemInfoSet(itemSlots[i].item);
        }

        int enchantPerStep;
        if (enchant_Item.type == ItemType.Staff)
        {
            enchantStartBtn.interactable = true;
            enchantPerStep = Mathf.Max(0, (int)itemSlots[0].item.rank - (int)itemSlots[1].item.rank);
            enchantPercent = EnchantManager.instance.staffEnchantPercents[enchantPerStep];
        }
        else
        {
            enchantStartBtn.interactable = false;
            StartCoroutine(EnchantStartBtnOn());

            enchantPerStep = Mathf.Max(0, (int)itemSlots[0].item.quality - (int)itemSlots[1].item.quality);
            enchantPercent = EnchantManager.instance.bookenchantPercents[enchantPerStep];

            aditionalSelectBtn[0].ButtonOn(enchant_Item.aditionalAbility.Length);
            aditionalSelectBtn[1].ButtonOn(material_Item.aditionalAbility.Length);
        }

        enchantPercentText.text = string.Format("<color=black>강화 성공 확률:</color> <color=red>{0}%</color>", enchantPercent);
    }
   
    public void EnchantStart()
    {

        int random = Random.Range(1, 101);

        if(random <= enchantPercent) // 강화 성공
        {
            Item enchantItem = itemSlots[0].item;
            switch (enchantItem.type)
            {
                case ItemType.Staff:
                    enchantItem.attack += EnchantManager.instance.staffAttackIncrease[(int)enchantItem.rank - 1];
                    enchantItem.enchantStep++;
                    break;
                case ItemType.Book:
                    Item materialItem = itemSlots[1].item;
                    enchantItem.aditionalAbility[selectEnchantAditionalNum] = materialItem.aditionalAbility[selectMaterialAditionalNum];
                    break;
            }

            Debug.Log("강화 성공 !");
        }
        else // 강화 실패
        {
            Debug.Log("강화 실패 ㅠㅠ");
        }

        // 재료 아이템 삭제
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            if (ItemDatabase.instance.Set(i) == itemSlots[1].item)
            {
                ItemDatabase.instance.ItemRemove(i);
                break;
            }
        }

        EnchantCancel();
    }
    public void EnchantAditionalSelect(int num)
    {
        if(selectEnchantAditionalNum != -1)
        {
            aditionalSelectBtn[0].InitButtonImage(selectEnchantAditionalNum);
        }
        selectEnchantAditionalNum = num;
        aditionalSelectBtn[0].SelectButtonImage(selectEnchantAditionalNum);

    }
    public void MaterialAditionalSelect(int num)
    {
        if (selectMaterialAditionalNum != -1)
        {
            aditionalSelectBtn[1].InitButtonImage(selectMaterialAditionalNum);
        }
        selectMaterialAditionalNum = num;
        aditionalSelectBtn[1].SelectButtonImage(selectMaterialAditionalNum);

    }
    public void EnchantCancel()
    {
        gameObject.SetActive(false);

        SkillBookEnchantInit();
        aditionalSelectBtn[0].gameObject.SetActive(false);
        aditionalSelectBtn[1].gameObject.SetActive(false);

        enchant.EnchantItemoff();
    }


    private IEnumerator EnchantStartBtnOn()
    {
        while(enchantStartBtn.interactable != true)
        {
            if (selectMaterialAditionalNum > -1 && selectMaterialAditionalNum > -1)
            {
                enchantStartBtn.interactable = true;
            }
            yield return null;
        }
    }
}
