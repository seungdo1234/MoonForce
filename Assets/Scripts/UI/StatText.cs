using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatText : MonoBehaviour
{
    public ItemSlot MainEquipment;



    private Text attackText;
    private Text rateText;
    private Text SpeedText;

    private void Awake()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        attackText = texts[1];
        rateText = texts[2];
        SpeedText = texts[3];
    }
    public void LoadStat()
    {
        float  addMoveSpeed = (int)((MainEquipment.item.moveSpeed + 1) * 100);
        float rate = (GameManager.instance.baseRate * 2 - GameManager.instance.rate) * 100; 

        attackText.text = string.Format("공격력   : {0:F0}  ({1:F0} + <color=red>{2:F0}</color>)", GameManager.instance.attack, GameManager.instance.baseAttack, MainEquipment.item.attack);
          rateText.text = string.Format("공격속도 : {0:F0}% ({1:F0}% + <color=red>{2:F0}</color>%)", rate, GameManager.instance.baseRate * 100, MainEquipment.item.rate * 100);
         SpeedText.text = string.Format("이동속도 : {0:F0}% ({1:F0}% + <color=red>{2:F0}</color>%)", addMoveSpeed,  100, MainEquipment.item.moveSpeed * 100);
        

    }
    private void Update()
    {
        if(MainEquipment.item != null)
        {
            LoadStat();
        }
    }
}
