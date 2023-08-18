using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{

    public Magic[] magicInfo;


    // 풀 담당을 하는 리스트들
    private List<GameObject>[] pools;
    private Player player;

    private void Awake()
    {
       // EnemyPrefabs의 길이 만큼 리스트 크기 초기화
        // Pool를 담는 배열 초기화
        pools = new List<GameObject>[magicInfo.Length];

        // 배열 안에 들어있는 각각의 리스트들도 초기화
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
                    StartCoroutine( AlwaysPlayMagic(i));
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


        // 스테이지를 클리어했기때문에 혹시나 활성화된 마법들을 비활성화 시킴 
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

        // 선택한 풀의 놀고(비활성화 된) 있는 게임오브젝트 접근 -> 발견하면 select 변수에 할당
        // 이미 생성한 Enemy가 죽었을 때 Destroy하지 않고 재활용
        foreach (GameObject item in pools[index])
        {
            // 내용물 오브젝트가 비활성화(대기 상태)인지 확인
            if (!item.activeSelf)
            {
                // 놀고 있는 게임오브젝트 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 만약 못찾았다면 -> 새롭게 생성하고 select 변수에 할당
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
            if (timer > magicInfo[magicNumber].magicCoolTime) // 쿨타임이 찼을 때
            {
                if (!player.scanner.nearestTarget[0] && !magicInfo[magicNumber].isNonTarget) // 주변이 ENemy가 없고 타겟이 필요한 스킬이라면 continue
                {

                    yield return null; // 대기 상태로 전환
                    continue;
                }
                timer = 0; // 있다면 쿨타임 초기화

                if (magicInfo[magicNumber].isFlying) 
                {
                    Fire(magicNumber); // 파이어볼 발사 
                }
                else 
                {
                    SpawnMagic(magicNumber);
                }
            }
            yield return null; // 반복 
        }
    }

    

    private void Fire(int magicNumber)
    {


        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // Magic 생성
            Transform magic = Get(magicNumber).transform;


            magic.position = player.transform.position; // Magict의 위치


            if (magicNumber == 8) // 윈드 컷터
            {
                //  초기화
                magic.localRotation = Quaternion.identity;


                // Bullet의 각도 구하기
                Vector3 rotVec = Vector3.forward * 360 * i / magicInfo[magicNumber].magicCount;


                magic.Rotate(rotVec + new Vector3(0, 0, 90));

                magic.GetComponent<WindCutter>().Init();
                continue;
            }

            Vector3 targetPos = player.scanner.nearestTarget[0].position;
            Vector3 dir = targetPos - player.transform.position;
            dir = dir.normalized; // 정규화

                                                // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
            magic.rotation = Quaternion.FromToRotation(Vector3.right, dir); // Enemy 방향으로 bullet 회전

            if (magicNumber == 0) // 파이어볼
            {
                magic.GetComponent<Bullet>().Init(0, magicInfo[magicNumber].penetration, dir);
            }
            else if (magicNumber == 6) // 괭이
            {
                magic.GetComponent<Hoe>().Init(dir);
            }
            else if (magicNumber == 7) // 돌 던지기
            {
                magic.GetComponent<RockThrow>().Init(dir);
            }
        }

    }

    private void SpawnMagic(int magicNumber)
    {
        int length = 0;

        for (int i = 0; i < player.scanner.nearestTarget.Length; i++) // 타겟 정하기
        {
            if (player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            length++;
        }

        int spawnCount = Mathf.Min(length, magicInfo[magicNumber].magicCount); // 마법 몇개를 스폰할건지 정함.

        List<int> availableIndices = new List<int>(); // 중복된 Enemy 제거하귀 위한 리스트 선언

        for (int i = 0; i < length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedIndex = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex); // 이미 선택된 Enemy를 리스트에서 제거

            Transform magic = Get(magicNumber).transform;
            magic.position = player.scanner.nearestTarget[selectedIndex].transform.position;

            if (magicNumber == 3)
            {
                Enemy enemy = player.scanner.nearestTarget[selectedIndex].GetComponent<Enemy>();
                magic.GetComponent<MoltenSpear>().Init(enemy);
            }
        }
    }

    private IEnumerator AlwaysPlayMagic(int magicNumber)
    {
        yield return null;

        switch (magicNumber)
        {
            case 4:
                ShovelSpawn(magicNumber);
                break;
            case 5:
                RakeSpawn(magicNumber);
                break;
        }


    }

    private void ShovelSpawn(int magicNumber)
    {
        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // bullet의 부모를 MagicManager에서  Player의 RotationBody로 바꾸기 위해 Transform으로 저장
            Transform bullet;

            bullet = Get(magicNumber).transform;
            bullet.parent = GameManager.instance.player.rotationBody;


            //  초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;


            // Bullet의 각도 구하기
            Vector3 rotVec = Vector3.forward * 360 * i / magicInfo[magicNumber].magicCount;
   
            bullet.Rotate(rotVec);


            // 플레이어로 부터 일정 거리를 떨어뜨림
            bullet.Translate(bullet.up * 1.5f, Space.World);

        }
    }
    private void RakeSpawn(int magicNumber)
    {
        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // bullet의 부모를 MagicManager에서  Player의 RotationBody로 바꾸기 위해 Transform으로 저장

            Transform bullet = Get(magicNumber).transform;
            bullet.position = GameManager.instance.player.transform.position;



        }
    }
}
