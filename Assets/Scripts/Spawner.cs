using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;




    // Ÿ�̸� 
    private float timer;

    private void Awake()
    {
        // �ټ��� ����Ʈ�� Transform ������ �޾ƿ��� ������ GetComponents ������
        spawnPoint = GetComponentsInChildren<Transform>();

    }
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        // FloorToInt �Ҽ��� �Ʒ��� ������ Int������ �ٲٴ� �Լ�
        // CeilToInt �ø� �� Int������ �ٲ�

        if (GameManager.instance.enemyCurNum < GameManager.instance.enemyMaxNum[GameManager.instance.level] && timer > spawnData[0].spawnTime)
        {
            timer = 0;
            GameManager.instance.enemyCurNum++;
            EnemySpawn();
        }

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