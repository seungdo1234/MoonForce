using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public float levelTime;



    [Header("Enemy Spawn Data")]
    public SpawnData[] spawnData;
    public int[] enemySpawnNum = new int[5];

    [Header("Enemy Spawn Percent")]
    public int[] enemySpawnPer;
    // Ÿ�̸� 
    private float timer;

    private void Awake()
    {
        // �ټ��� ����Ʈ�� Transform ������ �޾ƿ��� ������ GetComponents ������
        spawnPoint = GetComponentsInChildren<Transform>();

    }
    private void Start()
    {

    }
    public  void StageStart()
    {
        EnemyRandomTypeSelect();

        for(int i =0; i < spawnData.Length; i++)
        {
            if(enemySpawnNum[i] != 0)
            {
                StartCoroutine(EnemySpawn(i));
            }
        }


    }

    private void EnemyRandomTypeSelect()
    {
        const int NumEnemyTypes = 5;
        const int MinRandomValue = 1;
        const int MaxRandomValue = 100;

        enemySpawnNum = new int[NumEnemyTypes];

        for (int i = 0; i < GameManager.instance.enemyMaxNum; i++) // �ش� ���������� Enemy ��ȯ�� ��ŭ �ݺ�
        {
            int percentSum = 0;
            int random = Random.Range(MinRandomValue, MaxRandomValue + 1); // Ȯ��

            for (int j = 0; j < enemySpawnPer.Length; j++) // ��� Ÿ���� Enemy�� �������� Ȯ���� ��� �迭 ��ŭ �ݺ� (ũ�� 5)
            {
                percentSum += enemySpawnPer[j];

                if (random <= percentSum) // �ش� ���ں��� ���ٸ� j�� ° Enemy Ÿ�� ��ȯ 
                {
                    enemySpawnNum[j]++;
                    break;
                }
            }
        }
    }

    private IEnumerator EnemySpawn(int enemyType)
    {
        float curTime = 0;
        int curEnemyNum = 0;

        while (curEnemyNum < enemySpawnNum[enemyType])
        {
            curTime += Time.deltaTime;

            if(curTime >= spawnData[enemyType].spawnTime)
            {
                Debug.Log(enemyType);
                curEnemyNum++;
                GameManager.instance.enemyCurNum++;
                curTime = 0;
                GameObject enemy = GameManager.instance.pool.Get(0);
                // *���� : GetComponentsInChildren�� �ڱ� �ڽŵ� �����̹Ƿ� 0�� Player�� Transform ������ �� -> ������ 1���� ����
                enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
                enemy.GetComponent<Enemy>().Init(spawnData[enemyType]);
            }
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    
    private void EnemySpawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        // *���� : GetComponentsInChildren�� �ڱ� �ڽŵ� �����̹Ƿ� 0�� Player�� Transform ������ �� -> ������ 1���� ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[0]);
    }
}

// ������ Enemy���� �����Ͱ� ����ִ� Ŭ���� (�ν����� �󿡼� ������ �ϱ����� ����ȭ (Serializable))
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}