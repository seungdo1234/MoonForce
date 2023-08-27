using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public float levelTime;

    [Header("Enemy Spawn Data")]
    public SpawnData[] spawnData;

    [Header("Enemy Spawn Percent")]
    public float[] enemySpawnPer;
    // 타이머 
    private float timer;

    private void Awake()
    {
        // 다수의 포인트의 Transform 정보를 받아오기 떄문에 GetComponents 여야함
        spawnPoint = GetComponentsInChildren<Transform>();

    }
    
    public  void StageStart()
    {
        for(int i =0; i<GameManager.instance.enemyMaxNum; i++)
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameStop)
        {
            return;
        }

        timer += Time.deltaTime;
        // FloorToInt 소수점 아래는 버리고 Int형으로 바꾸는 함수
        // CeilToInt 올림 후 Int형으로 바꿈

        if (GameManager.instance.enemyCurNum < GameManager.instance.enemyMaxNum && timer > spawnData[0].spawnTime)
        {
            timer = 0;
            GameManager.instance.enemyCurNum++;
            EnemySpawn();
        }

    }
    
    private void EnemySpawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        // *주의 : GetComponentsInChildren은 자기 자신도 포함이므로 0은 Player의 Transform 정보가 들어감 -> 랜덤은 1부터 시작
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[0]);
    }
}

// 스폰된 Enemy들의 데이터가 들어있는 클래스 (인스펙터 상에서 나오게 하기위해 직렬화 (Serializable))
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}