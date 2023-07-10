using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stat")]
    public int weaponNum; // 총 : 한번에 발사 되는 총탄, 근접 : 삽의 갯수
    public int damage; // 데미지
    public float rate; // 공격 속도
    public int penetration; // 관통력

    private Player player;
    private float timer;
    private void Start()
    {
        player = GameManager.instance.player;
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > rate)
        {
            timer = 0;
            Fire();
        }
    }

    private void Fire() // 가장 가까운 Enemy한테 총알 발사
    {
        if (!player.scanner.nearestTarget[0])
        {
            return;
        }

        // Enemy 위치, 방향 구하기
        for(int i = 0; i< weaponNum; i++)
        {
            if(player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            Vector3 targetPos = player.scanner.nearestTarget[i].position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized; // 정규화


            // bullet 생성
            Transform bullet = GameManager.instance.pool.Get(1).transform;
            bullet.position = transform.position; // bullet의 위치
                                                  // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // Enemy 방향으로 bullet 회전

            // 원거리 공격은 Count는 관통력
            bullet.GetComponent<Bullet>().Init(damage, penetration - 1, dir);
            bullet.GetComponent<Bullet>().Init(damage, penetration - 1, dir);
        }
       

    }

}
