using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyStatusEffect {Defalt, Burn, Wet, Earth, GrassRestraint, Darkness }
public class Enemy : MonoBehaviour
{
    [Header("# EnemyBaseStat")]
    public float speed;
    public float health;
    public float maxHealth;

    [Header("# EnemyStatusEffect")]
    public EnemyStatusEffect statusEffect;
    public float statusEffectTime;

    private int burnningDamage;
    private float lerpTime;

    [Header("# EnemyType")]
    // �ִϸ������� �����͸� �ٲٴ� ������Ʈ => RuntimeAnimatorController
    public RuntimeAnimatorController[] animCon;


    [Header("# TargetPlayer")]
    public Rigidbody2D target; // Ÿ��

    [Header("# DamageText")]
    public GameObject damageText; // Ÿ��

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
     //  EnemyDamaged(Mathf.Floor( collision.GetComponent<Bullet>().damage));
       EnemyDamaged(collision.GetComponent<Bullet>().damage);
        StartCoroutine(KnockBack());

        StatusEffect();


        if (health > 0)
        {
            // Live, Hit Action
            anim.SetTrigger("Hit");


        }
        else
        {
            StopAllCoroutines();
            isLive = false; // �׾��� üũ
            col.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
            rigid.simulated = false; // rigidbody2D ����
            spriteRenderer.sortingOrder = 1; // ���� ENemy�� �ٸ� Enemy�� ������ �ʵ��� OrderLayer�� 1�� ����
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
        }
    }

    private void StatusEffect()
    {
        switch (GameManager.instance.attribute)
        {
            case ItemAttribute.Fire:
                if(statusEffect != EnemyStatusEffect.Burn)
                {
                    statusEffect = EnemyStatusEffect.Burn;
                    StartCoroutine(Burning());
                }
                else // �̹� ȭ�� ���¶�� �����̻� �ð��� Ƚ�� �ʱ�ȭ
                {
                    lerpTime = statusEffectTime;
                    burnningDamage = (int)statusEffectTime;
                }
                break;
            case ItemAttribute.Water:
                break;
           case ItemAttribute.Grass:
                break;
            case ItemAttribute.Eeath:
                break;
            case ItemAttribute.Dark:
                break;
            case ItemAttribute.Holy:
                break;
            default:
                return;

        }
    }
    private IEnumerator KnockBack()
    {
        yield return wait; // ���� �ϳ��� ���� �������� ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private IEnumerator Burning() // ȭ��
    {
        lerpTime = statusEffectTime;
        burnningDamage = (int)statusEffectTime;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        sprite.color = new Color(1, 0.7f, 0.7f, 1);

        while(lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;
            if(lerpTime < burnningDamage)
            {
                burnningDamage--;
                EnemyDamaged(Mathf.Floor(GameManager.instance.attack * 0.5f));
                anim.SetTrigger("Hit");
            }
            yield return null;
        }

        statusEffect = EnemyStatusEffect.Defalt; 

        yield return new WaitForSeconds(0.5f);

        sprite.color = new Color(1, 1, 1, 1);

    }


    private void EnemyDamaged(float damage)
    {
        health -= damage;

        Vector3 textPos = transform.position + new Vector3(0, 0.5f, 0);

        GameManager.instance.damageTextPool.Get(damage, textPos);
    }
    private void Dead()
    {
        gameObject.SetActive(false);
    }

}
