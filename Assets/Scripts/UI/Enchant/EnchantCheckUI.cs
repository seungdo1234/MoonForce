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

    public GameObject touchBlock; // ��ȭ�ϴ� ���� �ٸ� �� ��ġ�� ���� Block �̹���
    public GameObject enchantResultNotice; // ��ȭ �� ������ �˸� ������Ʈ
    private Text enchantResultNoticeText; // �ؽ�Ʈ

    private ItemSlot[] itemSlots;
    private int enchantPercent;

    [Header("Skill Book")]
    public AditionalSelectBtn[] aditionalSelectBtn;
    public int selectEnchantAditionalNum;
    public int selectMaterialAditionalNum;
    private void Awake()
    {
        enchantResultNoticeText = enchantResultNotice.GetComponentInChildren<Text>(true);
        itemSlots = GetComponentsInChildren<ItemSlot>();
    }
    private void SkillBookEnchantInit()
    {
        if(selectEnchantAditionalNum != -1)
        {
            aditionalSelectBtn[0].InitButtonImage(selectEnchantAditionalNum);
            selectEnchantAditionalNum = -1;
        }
        if(selectMaterialAditionalNum != -1)
        {
            aditionalSelectBtn[1].InitButtonImage(selectMaterialAditionalNum);
            selectMaterialAditionalNum = -1;
        }

    }
    public void UIOn(Item enchant_Item , Item material_Item)
    {
        gameObject.SetActive(true);

        itemSlots[0].item = enchant_Item;
        itemSlots[1].item = material_Item;

        for(int i =0; i < itemSlots.Length; i++) // �̹���, ������ ���� �ε�
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

        enchantPercentText.text = string.Format("<color=black>��ȭ ���� Ȯ��:</color> <color=red>{0}%</color>", enchantPercent);
    }
   
    public void EnchantStart()
    {
        bool enchantResult;
        int random = Random.Range(1, 101);

        if(random <= enchantPercent) // ��ȭ ����
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

            enchantResult = true;
        }
        else // ��ȭ ����
        {
            enchantResult = false;
        }

        // ��� ������ ����
        for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
        {
            if (ItemDatabase.instance.Set(i) == itemSlots[1].item)
            {
                ItemDatabase.instance.ItemRemove(i);
                break;
            }
        }

        StartCoroutine(EnchantResultNoticeOn(enchantResult));
    }
    private IEnumerator EnchantResultNoticeOn(bool enchantResult) // ��ȭ ��� �˸� â
    {
        touchBlock.SetActive(true);
        enchantResultNotice.SetActive(true);
        yield return new WaitForSeconds(1);

        if (enchantResult)
        {
            enchantResultNoticeText.text = "��ȭ ���� !";
            AudioManager.instance.PlayerSfx(Sfx.EnchantSuccess);
        }
        else
        {
            enchantResultNoticeText.text = "��ȭ ���� �Ф�";
            AudioManager.instance.PlayerSfx(Sfx.EnchantFail);
        }
        enchantResultNoticeText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        enchantResultNoticeText.gameObject.SetActive(false);
        enchantResultNotice.SetActive(false);
        EnchantCancel();
        touchBlock.SetActive(false);
    }
    public void EnchantAditionalSelect(int num) // ����å ��ȭ�� �� ��ȭ�� ����å�� ��ų ����
    {
        if(selectEnchantAditionalNum != -1)
        {
            aditionalSelectBtn[0].InitButtonImage(selectEnchantAditionalNum);
        }
        selectEnchantAditionalNum = num;
        aditionalSelectBtn[0].SelectButtonImage(selectEnchantAditionalNum);

    }
    public void MaterialAditionalSelect(int num)// ����å ��ȭ�� �� ��� ����å�� ��ų ����
    { 
        if (selectMaterialAditionalNum != -1)
        {
            aditionalSelectBtn[1].InitButtonImage(selectMaterialAditionalNum);
        }
        selectMaterialAditionalNum = num;
        aditionalSelectBtn[1].SelectButtonImage(selectMaterialAditionalNum);

    }
    public void EnchantCancel() // ��þƮ�� ����ϰų� �������� �� ���� ��ȭâ�� �������ϴ� �Լ�
    {
        gameObject.SetActive(false);

        SkillBookEnchantInit();
        aditionalSelectBtn[0].gameObject.SetActive(false);
        aditionalSelectBtn[1].gameObject.SetActive(false);

        enchant.EnchantItemoff();
    }
    private IEnumerator EnchantStartBtnOn() // ����å ��þƮ �� ��ų �ΰ��� �����ؾ߸� ��ȭ�� �� �ְ� �ϴ� �ڷ�ƾ 
    {
        while(enchantStartBtn.interactable != true)
        {
            if (selectMaterialAditionalNum != -1 && selectEnchantAditionalNum != -1)
            {
                enchantStartBtn.interactable = true;
            }
            yield return null;
        }
    }
}