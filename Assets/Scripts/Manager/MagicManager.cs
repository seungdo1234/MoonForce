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
    }

    public void StageStart()
    {
        for (int i = 0; i < magicInfo.Length; i++)
        {
            if (magicInfo[i].isMagicActive)
            {
                StartCoroutine(StartMagic(i));
            }
        }
    }

    public void StageClear()
    {
        StopAllCoroutines();
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


    private IEnumerator StartMagic(int magicNumber)
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

        int random = Random.Range(0, player.scanner.nearestTarget.Length);


        Transform magic = Get(magicNumber).transform;
        magic.position = player.scanner.nearestTarget[random].transform.position;


    }
}
