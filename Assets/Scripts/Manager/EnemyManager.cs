using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [Header("# EnemyData")]
    public EnemyData[] enemyDatas;
    public EnemyData[] mutationEnemyDatas;

    [Header("# EnemyBaseStat")]
    public int baseDamage;
    public float baseHealth;
    public float baseSpeed;

    [Header("# EnemyIncreaseStat")]
    public int increaseByDamage;
    public float increaseByHeath;
    public float increaseBySpeed;

    [Header("# EnemyCurrentStat")]
    public int damage;
    public float health;
    public float speed;


    private void Awake()
    {
        instance = this;
        EnemyReset();
    }

    public void EnemyReset()
    {
        damage = baseDamage;
        health = baseHealth;
        speed = baseSpeed;
    }

    //enemyLevelUp
    public void EnemyLevelUp()
    {
        int level = GameManager.instance.level;
        if ((level <= 10 && level % 2 == 0) || (level > 10 && (level - 10) % 3 == 0))
        {
            damage += increaseByDamage;
        }
        health += increaseByHeath;
        speed += increaseBySpeed;
        if ((level + 1) % 5 == 0)
        {
            GameManager.instance.spawner.spawnPerLevelUp++;
        }
    }

}

[System.Serializable]
public class EnemyData
{
    [Header("# BaseEnemy By Stat")]
    public float damagePer;
    public float healthPer;
    public float increasedSpeed;
    public float mass;
    [Header("# EnemyScale")]
    // Enemy�� ���� ũ��
    public Vector3 enemyBaseScale;
    // Enemy ������ ��������
    public float enemyScaleErrorRange;
    public float enemyScaleByDamage;
    public float enemyScaleByHealth;
    public float enemyScaleBySpeed;


    [Header("# EnemySprite")]
    // �ִϸ������� �����͸� �ٲٴ� ������Ʈ => RuntimeAnimatorController
    public RuntimeAnimatorController[] animCon;

}
