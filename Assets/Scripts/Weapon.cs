using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stat")]
    public int weaponNum; // �� : �ѹ��� �߻� �Ǵ� ��ź, ���� : ���� ����
    public int damage; // ������
    public float rate; // ���� �ӵ�
    public int penetration; // �����

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

    private void Fire() // ���� ����� Enemy���� �Ѿ� �߻�
    {
        if (!player.scanner.nearestTarget[0])
        {
            return;
        }

        // Enemy ��ġ, ���� ���ϱ�
        for(int i = 0; i< weaponNum; i++)
        {
            if(player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            Vector3 targetPos = player.scanner.nearestTarget[i].position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized; // ����ȭ


            // bullet ����
            Transform bullet = GameManager.instance.pool.Get(1).transform;
            bullet.position = transform.position; // bullet�� ��ġ
                                                  // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // Enemy �������� bullet ȸ��

            // ���Ÿ� ������ Count�� �����
            bullet.GetComponent<Bullet>().Init(damage, penetration - 1, dir);
            bullet.GetComponent<Bullet>().Init(damage, penetration - 1, dir);
        }
       

    }

}
