using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [Header("# Basic Stat")]
    public float maxHealth;
    public float curHealth;
    public int attack;
    public int baseAttack;
    public float rate;
    public float baseRate;
    public float moveSpeed;
    public float baseMoveSpeed;
    public int penetration; // �����
    public int weaponNum; // �� : �ѹ��� �߻� �Ǵ� ��ź, ���� : ���� ����
    public float knockBackValue; // Enemy�� ����ź�� �¾��� �� �˹� ��ġ

    [Header("# Stat Level")]
    public int[] statLevels;
    public int[] statMaxLevels;
    public float[] statUpValue;

    [Header("# Fire Attribute Stat")]
    public float burningDamagePer; // ȭ�� ������
    public float burningEffectTime; // ȭ�� �ð�

    [Header("# Water Attribute Stat")]
    public float wettingDamagePer; // ���� ������ �� ���� �߰� ��������
    public float wettingEffectTime; // ���� ���� �ð�

    [Header("# Non Attribute Stat")]
    public float bulletDamagePer; // ����ź ������ ������

    [Header("# Grass Attribute Stat")]
    public float restraintTime; // �ӹ� �ð�
     
    [Header("# Earth Attribute Stat")]
    public float speedReducedEffectTime; // �̵��ӵ� ���� �ð�
    public float speedReducePer; // �̵��ӵ� ���ҷ�

    [Header("# Dark Attribute Stat")]
    public float darkExplosionDamagePer; // ���� ������ 
    public float darknessExpTime; // �� �ʵ� ������ �� ���� 

    [Header("# Holy Attribute Stat")]
    public float instantKillPer; // ��� Ȯ��



    private void Awake()
    {
        curHealth = maxHealth;
        attack = baseAttack;
        rate = baseRate;
        moveSpeed = baseMoveSpeed;
    }


    public void StatValueUp(int statNumber)
    {
        switch (statNumber)
        {
            case 0:
                baseAttack += (int)statUpValue[statNumber];
                attack = baseAttack;
                break;
            case 1:
                baseRate -= statUpValue[statNumber];
                rate = baseRate;
                break;
            case 2:
                baseMoveSpeed += statUpValue[statNumber];
                moveSpeed = baseMoveSpeed;
                break;
            case 3:
                maxHealth += statUpValue[statNumber];
                curHealth += statUpValue[statNumber];
                break;
            case 4:
                penetration += (int)statUpValue[statNumber];
                break;
            case 5:
                burningDamagePer += statUpValue[statNumber];
                break;
            case 6:
                wettingDamagePer += statUpValue[statNumber];
                break;
            case 7:
                bulletDamagePer += statUpValue[statNumber];
                break;
            case 8:
                restraintTime += statUpValue[statNumber];
                break;
            case 9:
                speedReducePer += statUpValue[statNumber];
                break;
            case 10:
                darkExplosionDamagePer += statUpValue[statNumber];
                break;
            case 11:
                instantKillPer += statUpValue[statNumber];
                break;
        }
    }
}
