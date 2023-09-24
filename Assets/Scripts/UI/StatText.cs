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
        availablePointText.text = string.Format("��� ���� ����Ʈ : {0}", GameManager.instance.availablePoint);
        LoadStat();
    }

    public void LoadStat()
    {
        StatManager statManager = GameManager.instance.statManager;
        Item mainEquipment = MainEquipment.item;

        float addMoveSpeed = (int)((statManager.moveSpeed - statManager.baseMoveSpeed ) * 100);
        float baseRate;
        float rate;

        // ��� �Ӽ��� ���ݼӵ��� 2�� �������� ������ ���ǹ����� ��������
        if (GameManager.instance.attribute == ItemAttribute.Dark)
        {
            baseRate = 50 + ((1.5f - statManager.baseRate) * 100);
            rate = baseRate + ((statManager.baseRate * 2 - statManager.rate) * 100);
        }
        else
        {
            baseRate = 100 + ((1.5f - statManager.baseRate) * 100);
            rate = baseRate + ((statManager.baseRate - statManager.rate) * 100);
        }

        StatTexts[1].text = string.Format("���ݷ�   : {0:F0}  ({1:F0} + <color=red>{2:F0}</color> + <color=blue>{3:F0}</color>)", statManager.attack, statManager.baseAttack, mainEquipment.attack , statManager.essenceStat[0]);
        StatTexts[2].text = string.Format("���ݼӵ� : {0:F0}% ({1:F0}% + <color=red>{2:F0}</color>% + <color=blue>{3:F0}</color>%)", rate, baseRate, mainEquipment.rate * 100 , statManager.essenceStat[2] * 100);
        StatTexts[3].text = string.Format("�̵��ӵ� : {0:F0}% ({1:F0}% + <color=blue>{2:F0}</color>%)", 100 + addMoveSpeed, 100, addMoveSpeed);
        StatTexts[4].text = string.Format("ü�� : <color=red>{0:F0}</color>/{1:F0}", statManager.curHealth, statManager.maxHealth);

        if(GameManager.instance.attribute == ItemAttribute.Dark)
        {
            StatTexts[5].text = string.Format("����� : {0:F0} ", 1);
        }
        else
        {
            StatTexts[5].text = string.Format("����� : {0:F0} ({1:F0} +  <color=blue>{2:F0}</color>)", statManager.penetration, statManager.penetration, statManager.essenceStat[3]);
        }
      }

    private void Update()
    {
        if (MainEquipment.item != null)
        {
            LoadStat();
        }
    }
}
