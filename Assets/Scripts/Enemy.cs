using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;

    // �ִϸ������� �����͸� �ٲٴ� ������Ʈ => RuntimeAnimatorController
    public RuntimeAnimatorController[] animCon;

    public Rigidbody2D target; // Ÿ��

    private bool isLive; // Enmey�� ����ִ���

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Collider2D col; // 2D�� Collider2D�� ��� �ݶ��̴��� ������ �� ����
    // ���� FixedUpdate���� ��ٸ�
    private WaitForFixedUpdate wait;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }


    private void FixedUpdate() // �������� �̵��� FixedUpdate
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) // Enemy�� �׾��ٸ� return
        {
            return;
        }
        // ������ ����
        Vector2 dirVec = target.position - rigid.position;
        // ���� �̵�
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        // �÷��̾�� �ε������� �ӵ��� ����� ������ �׻� 0���� �ʱ�ȭ
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {

        if (!isLive)
        {
            return;
        }
        // ��������Ʈ ���� ��ȯ -> �÷��̾ Enmey���� ���ʿ� ���� ��
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    // ��ũ��Ʈ�� Ȱ��ȭ �� ��, ȣ���ϴ� �Լ�
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true; // ���� ����
        // Enemy�� �׾��� �� ������Ʈ ������ ���� �ٽ� ���ܳ� �� health�� maxHealth�� �ʱ�ȭ
        health = maxHealth;
        col.enabled = true; // �ݶ��̴� Ȱ��ȭ
        rigid.simulated = true; // rigidbody2D Ȱ��ȭ
        spriteRenderer.sortingOrder = 2; // OrderLayer�� 2�� ����
        anim.SetBool("Dead", false);
    }

    // Enemy ���� �� �ʱ�ȭ
    public void Init(SpawnData data)
    {
        // �ִϸ��̼��� �ش� ��������Ʈ�� �°� �ٲ���
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����
        if (!collision.CompareTag("Bullet") || !isLive)
        {
            return;
        }

        // �ǰ�
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // Live, Hit Action
            anim.SetTrigger("Hit");


        }
        else
        {
            isLive = false; // �׾��� üũ
            col.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
            rigid.simulated = false; // rigidbody2D ����
            spriteRenderer.sortingOrder = 1; // ���� ENemy�� �ٸ� Enemy�� ������ �ʵ��� OrderLayer�� 1�� ����
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
        }
    }

    private IEnumerator KnockBack()
    {
        yield return wait; // ���� �ϳ��� ���� �������� ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

}
