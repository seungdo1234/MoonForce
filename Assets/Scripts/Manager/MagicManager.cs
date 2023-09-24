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
                if(magicInfo[i].magicCoolTime == 0 || i == 13 || i == 1)
                {
                    StartCoroutine( AlwaysPlayMagic(i));
                }
                else
                {
                    StartCoroutine(StartCoolTimeMagic(i));
                }
            }
        }
    }

    public void PoolingReset()
    {
        StopAllCoroutines();


        // ���������� Ŭ�����߱⶧���� Ȥ�ó� Ȱ��ȭ�� �������� ��Ȱ��ȭ ��Ŵ 
        for(int i = 0; i<magicInfo.Length; i++)
        {
            foreach (GameObject item in pools[i])
            {
                if (item.activeSelf)
                {
                    MagicNumber magic = item.GetComponent<MagicNumber>();
                    if (i != 1 && magic.isSizeUp)
                    {
                        magic.GetComponent<Transform>().localScale = magic.resetScale;
                        magic.isSizeUp = false;
                    }
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
                if (!player.scanner.nearestTarget[0] && !magicInfo[magicNumber].isNonTarget) // �ֺ��� ENemy�� ���� Ÿ���� �ʿ��� ��ų�̶�� continue
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


        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // Magic ����
            Transform magic = Get(magicNumber).transform;


            magic.position = player.transform.position; // Magict�� ��ġ


            if (magicNumber == 9) // ���� ����
            {
                //  �ʱ�ȭ
                magic.localRotation = Quaternion.identity;


                // Bullet�� ���� ���ϱ�
                Vector3 rotVec = Vector3.forward * 360 * i / magicInfo[magicNumber].magicCount;


                magic.Rotate(rotVec + new Vector3(0, 0, 90));

                magic.GetComponent<WindCutter>().Init();
                continue;
            }


            MagicSizeUp(magic, magicNumber);

            Vector3 targetPos = player.scanner.nearestTarget[0].position;
            Vector3 dir = targetPos - player.transform.position;
            dir = dir.normalized; // ����ȭ

                                                // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
            magic.rotation = Quaternion.FromToRotation(Vector3.right, dir); // Enemy �������� bullet ȸ��

            if (magicNumber == 2) // ���̾
            {
                magic.GetComponent<Bullet>().Init(0, magicInfo[magicNumber].penetration, dir, 11.5f);
            }
            else if (magicNumber == 7) // ����
            {
                magic.GetComponent<Hoe>().Init(dir);
            }
            else if (magicNumber == 8) // �� ������
            {
                magic.GetComponent<RockThrow>().Init(dir);
            }
        }

    }

    private void SpawnMagic(int magicNumber)
    {

        int length = 0;

        for (int i = 0; i < player.scanner.nearestTarget.Length; i++) // Ÿ�� ���ϱ�
        {
            if (player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            length++;
        }

        int spawnCount = Mathf.Min(length, magicInfo[magicNumber].magicCount); // ���� ��� �����Ұ��� ����.

        List<int> availableIndices = new List<int>(); // �ߺ��� Enemy �����ϱ� ���� ����Ʈ ����

        for (int i = 0; i < length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedIndex = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex); // �̹� ���õ� Enemy�� ����Ʈ���� ����

            Transform magic = Get(magicNumber).transform;
            magic.position = player.scanner.nearestTarget[selectedIndex].transform.position;
            MagicSizeUp(magic, magicNumber);
            if (magicNumber == 4)
            {
                Enemy enemy = player.scanner.nearestTarget[selectedIndex].GetComponent<Enemy>();
                magic.GetComponent<MoltenSpear>().Init(enemy);
            }
        }
    }

    private void MagicSizeUp(Transform magic , int magicNumber) // ���� ũ�� Up
    {
        if (!magic.GetComponent<MagicNumber>().isSizeUp && !magicInfo[magicNumber].magicCountIncrease && magicInfo[magicNumber].magicSizeStep != 0)
        {
            int magicSizeStep = magicInfo[magicNumber].magicSizeStep;
            magic.localScale = new Vector3(magic.localScale.x + (magicSizeStep * 0.25f), magic.localScale.y + (magicSizeStep * 0.25f), magic.localScale.z + (magicSizeStep * 0.25f));
            magic.GetComponent<MagicNumber>().isSizeUp = true;
        }
    }
    private IEnumerator AlwaysPlayMagic(int magicNumber)
    {
        yield return null;

        switch (magicNumber)
        {
            case 1:
                InfernoSpawn(magicNumber);
                break;
            case 5:
                ShovelSpawn(magicNumber);
                break;
            case 6:
                MagicBallSpawn(magicNumber);
                break;
            case 13:
                ChargeExplosionSpawn(magicNumber);
                break;
        }


    }
    private void InfernoSpawn(int magicNumber)
    {
        GameObject magic = Get(magicNumber);

        magic.GetComponent<Inferno>().Init(magicInfo[magicNumber].magicCoolTime);
    }
    private void ChargeExplosionSpawn(int magicNumber)
    {
        GameObject magic = Get(magicNumber);

        magic.GetComponent<ChargeExplosion>().Init(magicInfo[magicNumber].magicCoolTime, magicInfo[magicNumber].magicSizeStep);
    }
    private void ShovelSpawn(int magicNumber)
    {
        // ���� �ӵ� �ܰ� ��ŭ ���� ȸ�� �ӵ� ++
        int rotationSpeed = 90 + (magicInfo[magicNumber].magicRateStep * 20);
        GameManager.instance.player.rotationBody.GetComponent<RotationWeapon>().rotationSpeed = rotationSpeed;
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
            bullet.Translate(bullet.up * 1f, Space.World);

        }
    }
    private void MagicBallSpawn(int magicNumber)
    {
        float lerpTime = 1.5f - (0.25f * magicInfo[magicNumber].magicRateStep);
        magicInfo[magicNumber].magicEffect.GetComponent<MagicBall>().lerpTime = lerpTime;

        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // bullet�� �θ� MagicManager����  Player�� RotationBody�� �ٲٱ� ���� Transform���� ����

            Transform bullet = Get(magicNumber).transform;
            bullet.position = GameManager.instance.player.transform.position;



        }
    }
}
