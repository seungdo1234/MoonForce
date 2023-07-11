using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;

    // 애니메이터의 데이터를 바꾸는 컴포넌트 => RuntimeAnimatorController
    public RuntimeAnimatorController[] animCon;

    public Rigidbody2D target; // 타겟

    private bool isLive; // Enmey가 살아있는지

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Collider2D col; // 2D는 Collider2D로 모든 콜라이더를 가져올 수 있음
    // 다음 FixedUpdate까지 기다림
    private WaitForFixedUpdate wait;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }


    private void FixedUpdate() // 물리적인 이동은 FixedUpdate
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) // Enemy가 죽었다면 return
        {
            return;
        }
        // 가야할 방향
        Vector2 dirVec = target.position - rigid.position;
        // 실제 이동
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        // 플레이어와 부딪혔을때 속도가 생기기 때문에 항상 0으로 초기화
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {

        if (!isLive)
        {
            return;
        }
        // 스프라이트 방향 전환 -> 플레이어가 Enmey보다 왼쪽에 있을 때
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    // 스크립트가 활성화 될 때, 호출하는 함수
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true; // 생존 여부
        // Enemy가 죽었을 때 오브젝트 폴링에 의해 다시 생겨날 때 health를 maxHealth로 초기화
        health = maxHealth;
        col.enabled = true; // 콜라이더 활성화
        rigid.simulated = true; // rigidbody2D 활성화
        spriteRenderer.sortingOrder = 2; // OrderLayer를 2로 내림
        anim.SetBool("Dead", false);
    }

    // Enemy 생성 전 초기화
    public void Init(SpawnData data)
    {
        // 애니메이션을 해당 스프라이트에 맞게 바꿔줌
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 필터
        if (!collision.CompareTag("Bullet") || !isLive)
        {
            return;
        }

        // 피격
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // Live, Hit Action
            anim.SetTrigger("Hit");


        }
        else
        {
            isLive = false; // 죽었다 체크
            col.enabled = false; // 콜라이더 비활성화
            rigid.simulated = false; // rigidbody2D 정지
            spriteRenderer.sortingOrder = 1; // 죽은 ENemy가 다른 Enemy를 가리지 않도록 OrderLayer를 1로 내림
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
        }
    }

    private IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임을 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

}
