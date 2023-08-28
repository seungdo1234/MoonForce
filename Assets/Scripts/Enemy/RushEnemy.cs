using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : MonoBehaviour
{


    public float rushDistance; // �뽬 �Ÿ�
    public float targetDetationDistance; // Player Ž�� �Ÿ�
    public float castTime; // ���� �ð�
    public float rushSpeed; // ���ʸ��� �뽬 �Ÿ��� ��������.
    public float rushDelayTime; // ���� ��Ÿ��
    public bool isReady; // �غ� �ƴ��� 
    public bool isAttack; // �÷��̾ ���� �ߴ���

    private Rigidbody2D rigid;
    private Animator anim;
    private Enemy enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

    }

    public void Init()
    {
        StartCoroutine(RushStart());
    }

    private IEnumerator RushStart()
    {
        float curTime = 0f;
        Transform target = enemy.target.transform;
        Vector3 dir = Vector3.zero;
        Vector3 initialPosition = Vector3.zero;

        while (true)
        {

            if (!isReady)
            {
                float distance = Vector3.Distance(transform.position, target.position); // Player�� Enemy������ �Ÿ��� ����

                if(!enemy.isRestraint && distance < targetDetationDistance) // targetDetationDistance�ȿ� ���Դٸ� ������ �غ� ��
                {
                    isReady = true;
                    initialPosition = transform.position; // ���� ��ġ ����
                    dir = (target.position - transform.position).normalized; // ���� ����
                    anim.speed = 0f; // �ִϸ��̼� �ӵ� 0
                    rigid.velocity = Vector3.zero; // Ȥ�ó� ������ ���̷� �ӵ��� ���� �� �ֱ� ������ �ʱ�ȭ
                    enemy.spriteRenderer.flipX = dir.x > 0 ? false : true; // Player �������� Flip
                }
            }
            else
            {
                if (enemy.isRestraint) // �غ� �߿� �ӹڿ� �ɸ��ٸ� ���� �ʱ�ȭ
                {
                    isReady = false;
                    curTime = 0;
                }
                curTime += Time.deltaTime;

                if(curTime > castTime) // ĳ���� �ð��� �Ǹ�
                {

                    rigid.velocity = dir * rushSpeed; // �ش� �������� ����
                    anim.speed = 1f;
                    rigid.mass = 1000;
                    while (true)
                    {
                        float distance = Vector3.Distance(transform.position, initialPosition); // ó�� ��ġ�� ���� ��ġ�� rushDistance ����ŭ �������� break
                        if (distance> rushDistance || isAttack || enemy.isRestraint) // Ȥ�� Player�� �����߰ų� (isAttacking), enemy�� ���°� ������ �� ���� ���¶�� (isRestraint) �� ��� ������ ����
                        {
                            break;
                        }
                        yield return null;
                    }

                    yield return null;

                    rigid.mass = 100;
                    isReady = false;
                    isAttack = false;
                    curTime = 0f;

                    yield return new WaitForSeconds(rushDelayTime);
                }
            }
     
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        isAttack = true;
    }

}
