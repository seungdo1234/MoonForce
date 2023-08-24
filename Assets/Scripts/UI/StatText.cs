using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatText : MonoBehaviour
{
    public ItemSlot MainEquipment;

    public Text availablePointText;
    private Text[] StatTexts;

    private void Awake()
    {
        StatTexts = GetComponentsInChildren<Text>();
    }
    private void OnEnable()
    {
        availablePointText.text = string.Format("사용 가능 포인트 : {0}", GameManager.instance.availablePoint);
    }
    public void LoadStat()
    {
        float  addMoveSpeed = (int)((MainEquipment.item.moveSpeed + 1) * 100);
        float rate = (GameManager.instance.statManager.baseRate * 2 - GameManager.instance.statManager.rate) * 100;

        StatTexts[1].text = string.Format("공격력   : {0:F0}  ({1:F0} + <color=red>{2:F0}</color>)", GameManager.instance.statManager.attack, GameManager.instance.statManager.baseAttack, MainEquipment.item.attack);
        StatTexts[2].text = string.Format("공격속도 : {0:F0}% ({1:F0}% + <color=red>{2:F0}</color>%)", rate, GameManager.instance.statManager.baseRate * 100, MainEquipment.item.rate * 100);
        StatTexts[3].text = string.Format("이동속도 : {0:F0}% ({1:F0}% + <color=red>{2:F0}</color>%)", addMoveSpeed,  100, MainEquipment.item.moveSpeed * 100);
        

    }
    private void Update()
    {
        if(MainEquipment.item != null)
        {
            LoadStat();
        }
    }
}
