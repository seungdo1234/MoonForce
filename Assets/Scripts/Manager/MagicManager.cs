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


    private IEnumerator StartMagic(int magicNumber)
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
                if (!player.scanner.nearestTarget[0]) // 주변이 ENemy가 없다면 continue
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
        Vector3 targetPos = player.scanner.nearestTarget[0].position;
        Vector3 dir = targetPos - player.transform.position;
        dir = dir.normalized; // 정규화


        // bullet 생성
        Transform bullet = Get(magicNumber).transform;
        bullet.position = player.transform.position; // bullet의 위치
                                              // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.right, dir); // Enemy 방향으로 bullet 회전

        bullet.GetComponent<Bullet>().Init(0, magicInfo[magicNumber].penetration, dir);
    }

    private void SpawnMagic(int magicNumber)
    {

        int random = Random.Range(0, player.scanner.nearestTarget.Length);


        Transform magic = Get(magicNumber).transform;
        magic.position = player.scanner.nearestTarget[random].transform.position;


    }
}
