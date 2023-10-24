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

    public int spawnPerLevelUp ;
    public EnemySpawnPer[] enemySpawnPer;

    private void Awake()
    {
        // 다수의 포인트의 Transform 정보를 받아오기 떄문에 GetComponents 여야함
        spawnPoint = GetComponentsInChildren<Transform>();

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

        for (int i = 0; i < GameManager.instance.enemyMaxNum; i++) // 해당 스테이지의 Enemy 소환량 만큼 반복
        {
            int percentSum = 0;
            int random = Random.Range(MinRandomValue, MaxRandomValue + 1); // 확률

            for (int j = 0; j < enemySpawnPer[spawnPerLevelUp].spawnPer.Length; j++) // 어떤 타입의 Enemy가 나오는지 확률이 담긴 배열 만큼 반복 (크기 5)
            {
                percentSum += enemySpawnPer[spawnPerLevelUp].spawnPer[j];

                if (random <= percentSum) // 해당 숫자보다 낮다면 j번 째 Enemy 타입 소환 
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

        while ( curEnemyNum < enemySpawnNum[enemyType])
        {
            curTime += Time.deltaTime;

            if(curTime >= spawnData[enemyType].spawnTime)
            {
                curEnemyNum++;
                GameManager.instance.enemyCurNum++;
                curTime = 0;
                GameObject enemy = GameManager.instance.pool.Get(0);
                // *주의 : GetComponentsInChildren은 자기 자신도 포함이므로 0은 Player의 Transform 정보가 들어감 -> 랜덤은 1부터 시작
                enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
                enemy.GetComponent<Enemy>().Init(spawnData[enemyType]);
            }

            if (GameManager.instance.gameStop)
            {
                break;
            }
            yield return null;
        }
    }

}

// 스폰된 Enemy들의 데이터가 들어있는 클래스 (인스펙터 상에서 나오게 하기위해 직렬화 (Serializable))
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int enemyType;
   
}
[System.Serializable]
public class EnemySpawnPer
{
    public int[] spawnPer;


}