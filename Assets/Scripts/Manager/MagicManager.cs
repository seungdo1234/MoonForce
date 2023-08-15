using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{

    public Magic[] magicInfo;


    // Ǯ ����� �ϴ� ����Ʈ��
    private List<GameObject>[] pools;
    private Player player;

    private void Awake()
    {
       // EnemyPrefabs�� ���� ��ŭ ����Ʈ ũ�� �ʱ�ȭ
        // Pool�� ��� �迭 �ʱ�ȭ
        pools = new List<GameObject>[magicInfo.Length];

        // �迭 �ȿ� ����ִ� ������ ����Ʈ�鵵 �ʱ�ȭ
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    private void Start()
    {
        player = GameManager.instance.player;

      //  StageStart();
    }

    public void StageStart()
    {
        for (int i = 0; i < magicInfo.Length; i++)
        {
            if (magicInfo[i].isMagicActive)
            {
                if(magicInfo[i].magicCoolTime == 0)
                {
                    AlwaysPlayMagic(i);
                }
                else
                {
                    StartCoroutine(StartCoolTimeMagic(i));
                }
            }
        }
    }

    public void StageClear()
    {
        StopAllCoroutines();


        // ���������� Ŭ�����߱⶧���� Ȥ�ó� Ȱ��ȭ�� �������� ��Ȱ��ȭ ��Ŵ 
        for(int i = 0; i<magicInfo.Length; i++)
        {
            foreach (GameObject item in pools[i])
            {
                if (item.activeSelf)
                {
                    item.SetActive(false);
                }
            }
        }

    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ���� -> �߰��ϸ� select ������ �Ҵ�
        // �̹� ������ Enemy�� �׾��� �� Destroy���� �ʰ� ��Ȱ��
        foreach (GameObject item in pools[index])
        {
            // ���빰 ������Ʈ�� ��Ȱ��ȭ(��� ����)���� Ȯ��
            if (!item.activeSelf)
            {
                // ��� �ִ� ���ӿ�����Ʈ select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ���� ��ã�Ҵٸ� -> ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(magicInfo[index].magicEffect, transform);

            pools[index].Add(select);
        }

        return select;
    }


    private IEnumerator StartCoolTimeMagic(int magicNumber)
    {
        float timer = 0;
        while (!GameManager.instance.gameStop)
        {
            if (timer <= magicInfo[magicNumber].magicCoolTime)
            {
                timer += Time.deltaTime;
            }
            if (timer > magicInfo[magicNumber].magicCoolTime) // ��Ÿ���� á�� ��
            {
                if (!player.scanner.nearestTarget[0]) // �ֺ��� ENemy�� ���ٸ� continue
                {
                    yield return null; // ��� ���·� ��ȯ
                    continue;
                }
                timer = 0; // �ִٸ� ��Ÿ�� �ʱ�ȭ

                if (magicInfo[magicNumber].isFlying) 
                {
                    Fire(magicNumber); // ���̾ �߻� 
                }
                else 
                {
                    SpawnMagic(magicNumber);
                }
            }
            yield return null; // �ݺ� 
        }
    }

    

    private void Fire(int magicNumber)
    {
        Vector3 targetPos = player.scanner.nearestTarget[0].position;
        Vector3 dir = targetPos - player.transform.position;
        dir = dir.normalized; // ����ȭ


        // bullet ����
        Transform bullet = Get(magicNumber).transform;
        bullet.position = player.transform.position; // bullet�� ��ġ
                                              // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        bullet.rotation = Quaternion.FromToRotation(Vector3.right, dir); // Enemy �������� bullet ȸ��

        bullet.GetComponent<Bullet>().Init(0, magicInfo[magicNumber].penetration, dir);
    }

    private void SpawnMagic(int magicNumber)
    {
        int length = 0;

        for (int i = 0; i < player.scanner.nearestTarget.Length; i++)
        {
            if (player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            length++;
        }

        int spawnCount = Mathf.Min(length, magicInfo[magicNumber].magicCount);

        List<int> availableIndices = new List<int>();

        for (int i = 0; i < length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedIndex = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex);

            Transform magic = Get(magicNumber).transform;
            magic.position = player.scanner.nearestTarget[selectedIndex].transform.position;

            if (magicNumber == 3)
            {
                Enemy enemy = player.scanner.nearestTarget[selectedIndex].GetComponent<Enemy>();
                magic.GetComponent<MoltenSpear>().Init(enemy);
            }
        }
    }

    private void AlwaysPlayMagic(int magicNumber)
    {
        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // bullet�� �θ� MagicManager����  Player�� RotationBody�� �ٲٱ� ���� Transform���� ����
            Transform bullet;

            bullet = Get(magicNumber).transform;
            bullet.parent = GameManager.instance.player.rotationBody;
          

            //  �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;


            // Bullet�� ���� ���ϱ�
            Vector3 rotVec = Vector3.forward * 360 * i / magicInfo[magicNumber].magicCount;
            bullet.Rotate(rotVec);

            
            // �÷��̾�� ���� ���� �Ÿ��� ����߸�
            bullet.Translate(bullet.up * 1.5f, Space.World);

        }

    }
}
