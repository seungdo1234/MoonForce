using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStageBtn : MonoBehaviour // 다음스테이지로 가는 버튼 (무기를 장착하지 않았거나, 인벤토리가 꽉 찼다면 활성화 X)
{
    public bool isActive;
    private Button btn;
    private Inventory inventory;
    public Text[] btnTexts;
    private void Start()
    {
        btn = GetComponent<Button>();
        inventory = GameManager.instance.inventory;
        btnTexts = GetComponentsInChildren<Text>(true);
    }

    private void Update()
    {
        if(!isActive && inventory.mainEqquipment.isEquip)
        {
            for(int i =0; i <inventory.waitEqquipments.Length; i++)
            {
                if(inventory.waitEqquipments[i].item.itemSprite == null)
                {
                    isActive = true;
                    btn.interactable = true;
                    btnTexts[0].gameObject.SetActive(true);
                    btnTexts[1].gameObject.SetActive(false);
                    break;
                }
            }
        }
        else if(isActive)
        {
            bool inventoryFull= false;
            for (int i = 0; i < inventory.waitEqquipments.Length; i++)
            {
                if (inventory.waitEqquipments[i].item.itemSprite == null)
                {
                    break;
                }
                else if (i == inventory.waitEqquipments.Length - 1)
                {
                    inventoryFull = true;
                }
            }
            if (!inventory.mainEqquipment.isEquip || inventoryFull)
            {
                isActive = false;
                btn.interactable = false;
                btnTexts[0].gameObject.SetActive(false);
                btnTexts[1].gameObject.SetActive(true);
            }
         
        }
    }
}
