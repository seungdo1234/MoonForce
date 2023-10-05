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
    public int penetration; // 관통력
    public int weaponNum; // 총 : 한번에 발사 되는 총탄, 근접 : 삽의 갯수
    public float knockBackValue; // Enemy가 마력탄을 맞았을 때 넉백 수치

    [Header("# Init Stat")]
    public float initHealth;
    public int initAttack;
    public float initRate;
    public float initMoveSpeed;


    [Header("# Essence")]
    public bool essenceOn;
    public int activeEssenceNum;
    public float[] essenceStat;

    [Header("# Stat Level")]
    public int[] statLevels;
    public int[] statMaxLevels;
    public float[] statUpValue;

    [Header("# Fire Attribute Stat")]
    public float burningDamagePer; // 화상 데미지
    public float baseBurningDamagePer; // 화상 데미지
    public float burningEffectTime; // 화상 시간

    [Header("# Water Attribute Stat")]
    public float wettingDamagePer; // 젖은 상태일 때 마법 추가 데미지량
    public float baseWettingDamagePer; // 젖은 상태일 때 마법 추가 데미지량
    public float wettingEffectTime; // 젖은 상태 시간

    [Header("# Non Attribute Stat")]
    public float bulletDamagePer; // 마력탄 데미지 증가량
    public float baseBulletDamagePer; // 마력탄 데미지 증가량

    [Header("# Grass Attribute Stat")]
    public float restraintTime; // 속박 시간
    public float baseRestraintTime; // 속박 시간
     
    [Header("# Earth Attribute Stat")]
    public float speedReducedEffectTime; // 이동속도 감소 시간
    public float speedReducePer; // 이동속도 감소량
    public float baseSpeedReducePer; // 이동속도 감소량

    [Header("# Dark Attribute Stat")]
    public float darkExplosionDamagePer; // 폭발 데미지 
    public float baseDarkExplosionDamagePer; // 폭발 데미지 
    public float darknessExpTime; // 몇 초뒤 폭발할 것 인지 

    [Header("# Holy Attribute Stat")]
    public float instantKillPer; // 즉사 확률
    public float baseInstantKillPer; // 즉사 확률



    public void GameStart()
    {
        penetration = 0;
        weaponNum = 1;
        curHealth = initHealth;
        attack = initAttack;
        rate = initRate;
        moveSpeed = initMoveSpeed;
        burningDamagePer = baseBurningDamagePer;
        wettingDamagePer = baseWettingDamagePer;
        bulletDamagePer = baseBulletDamagePer;
        restraintTime = baseRestraintTime;
        speedReducePer = baseSpeedReducePer;
        darkExplosionDamagePer = baseDarkExplosionDamagePer;
        instantKillPer = baseInstantKillPer;

        for(int i =0; i<statLevels.Length; i++)
        {

            statLevels[i] = 0;
        }

    }

    public void EssenceOn(int essenceNum, float stat)
    {
        essenceOn = true;
        activeEssenceNum = essenceNum;

        switch (essenceNum)
        {
            case 0:
                essenceStat[0] = Mathf.Floor(attack * stat);
                attack += (int)essenceStat[0];
                break;
            case 1:
                essenceStat[1] = stat;
                moveSpeed += essenceStat[1];
                break;
            case 2:
                essenceStat[2] = stat;
                rate -= essenceStat[2];
                break;
            case 3:
                essenceStat[3] = stat;
                penetration += (int)essenceStat[3];
                break;
        }
    }
    public void EssenceOff()
    {
        essenceOn = false;

        switch (activeEssenceNum) {
            case 0:
                attack -= (int)essenceStat[0];
                break;
            case 1:
                moveSpeed -= essenceStat[1];
                break;
            case 2:
                rate += essenceStat[2];
                break;
            case 3:
                penetration -= (int)essenceStat[3];
                break;
        }

        essenceStat[activeEssenceNum] = 0;
        activeEssenceNum = -1;
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
