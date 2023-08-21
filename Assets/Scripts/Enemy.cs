using System.Collections;
using UnityEngine;

public enum EnemyStatusEffect { Defalt, Burn, Wet, Earth, GrassRestraint, Darkness }
public class Enemy : MonoBehaviour
{
    [Header("# EnemyBaseStat")]
    public float speed;
    public float health;
    public float maxHealth;

    [Header("# EnemyHit")]
    public bool enemyKnockBack; // �˹��� ��
    public bool enemyDamaged; // �������� �޾Ҵٸ� ���� �ð����� �ȹް���
    public float damagedTime; // ���ʵ��� �����ϰ���
    public float knockBackValue; // �ǰ� �� �˹� ��ġ

    [Header("# EnemyStatusEffect")]
    public EnemyStatusEffect statusEffect;
    public float burningEffectTime;
    public float wettingEffectTime;
    public bool isWetting; // wet �������� (�´ٸ� ���� ������ ++)
    public float wetDamagePer;
    public float restraintEffectTime;
    public bool isRestraint;
    public float speedReducedEffectTime;
    public float speedReducePer;
    public float darknessExpTime;

    private int burnningDamage;
    private float lerpTime;

    [Header("# EnemyType")]
    // �ִϸ������� �����͸� �ٲٴ� ������Ʈ => RuntimeAnimatorController
    public RuntimeAnimatorController[] animCon;


    [Header("# TargetPlayer")]
    public Rigidbody2D target; // Ÿ��


    private bool isLive; // Enmey�� ����ִ���

    private int hitAnimID;
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
        hitAnimID = Animator.StringToHash("Hit");
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
        if (!enemyKnockBack) // �˹� �� �϶� �ӵ��� �ʱ�ȭ�ϸ� �ȵǱ� ������
        {
            rigid.velocity = Vector2.zero;
        }
    }

    private void LateUpdate()
    {

        if (isRestraint || !isLive)
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
        enemyDamaged = false;
        // Enemy�� �׾��� �� ������Ʈ ������ ���� �ٽ� ���ܳ� �� health�� maxHealth�� �ʱ�ȭ
        health = maxHealth;
        col.enabled = true; // �ݶ��̴� Ȱ��ȭ
        rigid.simulated = true; // rigidbody2D Ȱ��ȭ
        spriteRenderer.sortingOrder = 3; // OrderLayer�� 3�� ����
        spriteRenderer.color = new Color(1, 1, 1, 1); // �÷� �ʱ�ȭ
        statusEffect = EnemyStatusEffect.Defalt; // ���� �ʱ�ȭ
        gameObject.layer = 6; // ���̾ Enemy�� ����
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
        if (!(collision.gameObject.layer == 10 || collision.gameObject.layer == 11) || !isLive || enemyDamaged)
        {
            return;
        }

        if (collision.gameObject.layer == 11) // ���� ����ź�̶��
        {
            knockBackValue = GameManager.instance.knockBackValue;
            EnemyDamaged(collision.GetComponent<Bullet>().damage, 1);
            StatusEffect();
        }
        else // �����̶�� 
        {
            int number = collision.GetComponent<MagicNumber>().magicNumber;
            float damage = GameManager.instance.attack * GameManager.instance.magicManager.magicInfo[number].damagePer;

            if (number == 9 || number == 11)
            {
                StartCoroutine(IsDamaged());
                if(number == 11)
                {
                    GameObject elec = collision.gameObject;

                    StartCoroutine(ElectricShock(elec));
                }
            }

            knockBackValue = GameManager.instance.magicManager.magicInfo[number].knockBackValue;

            EnemyDamaged(damage, 2);


        }
        EnemyHit();
    }

    private void EnemyHit()
    {

        if (health > 0)
        {
            // Live, Hit Action
            anim.SetTrigger("Hit");
            StartCoroutine(KnockBack());

        }
        else
        {
            StopAllCoroutines();
            if (anim.speed != 1) // ����� ���¿��� �״´ٸ�
            {
                anim.speed = 1f;
            }
            isRestraint = false;
            spriteRenderer.color = new Color(1, 1, 1, 1); // ���� �� ������
            isLive = false; // �׾��� üũ
            col.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
            rigid.simulated = false; // rigidbody2D ����
            spriteRenderer.sortingOrder = 1; // ���� ENemy�� �ٸ� Enemy�� ������ �ʵ��� OrderLayer�� 1�� ����
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) // �������� ���ظ� �ִ� ������ �浹 �� �϶�
    {
        if (collision.gameObject.layer != 10 || enemyDamaged || !isLive)
        {
            return;
        }

        int number = collision.GetComponent<MagicNumber>().magicNumber;

        if (number != 9 && number != 11) // �������� ���ظ� �ִ� ������ �ƴ϶��
        {
            return;
        }
        StartCoroutine(IsDamaged());

        float damage = GameManager.instance.attack * GameManager.instance.magicManager.magicInfo[number].damagePer;

        EnemyDamaged(damage, 2);

        EnemyHit();
    }
    private IEnumerator IsDamaged() // Enemy�� �������� ���ظ� �ִ� ���� ���� ������� ���� �ð��ڿ� �������� �ޱ� ���� �ڷ�ƾ
    {
        enemyDamaged = true;
        yield return new WaitForSeconds(damagedTime);
        enemyDamaged = false;
    }
    private void StatusEffect()
    {
        switch (GameManager.instance.attribute)
        {
            case ItemAttribute.Fire:
                lerpTime = burningEffectTime + 1;
                burnningDamage = (int)burningEffectTime;

                if (statusEffect != EnemyStatusEffect.Burn)
                {
                    statusEffect = EnemyStatusEffect.Burn;
                    StartCoroutine(Burning());
                }
                break;
            case ItemAttribute.Water:
                lerpTime = wettingEffectTime;

                if (statusEffect != EnemyStatusEffect.Wet)
                {
                    statusEffect = EnemyStatusEffect.Wet;
                    StartCoroutine(Wetting());
                }

                break;
            case ItemAttribute.Grass:
                lerpTime = restraintEffectTime;
                if (statusEffect != EnemyStatusEffect.GrassRestraint)
                {
                    statusEffect = EnemyStatusEffect.GrassRestraint;
                    StartCoroutine(Restraint());
                }

                break;
            case ItemAttribute.Eeath:

                lerpTime = speedReducedEffectTime;

                if (statusEffect != EnemyStatusEffect.Earth)
                {
                    statusEffect = EnemyStatusEffect.Earth;
                    StartCoroutine(ReducedSpeed());
                }

                break;
            case ItemAttribute.Dark:

                if (statusEffect != EnemyStatusEffect.Darkness)
                {
                    statusEffect = EnemyStatusEffect.Darkness;
                    StartCoroutine(DarknessExplosion());
                }
                break;
            case ItemAttribute.Holy:

                break;
            default:
                return;

        }
    }
    private IEnumerator KnockBack()
    {
        enemyKnockBack = true;
        yield return wait; // ���� �ϳ��� ���� �������� ������
        rigid.mass = 100;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * knockBackValue, ForceMode2D.Impulse);
        yield return wait; // ���� �ϳ��� ���� �������� ������
        rigid.mass = 1;
        enemyKnockBack = false;

    }
    private IEnumerator ElectricShock(GameObject elec) // �����ũ�� �¾��� �� ����
    {
        float speed = this.speed;

        spriteRenderer.color = new Color(1, 1, 0.5f, 1);

        isRestraint = true;
        anim.speed = 0f;
        this.speed = 0f;
        rigid.velocity = Vector2.zero;

        while (true)
        {
            if (!elec.activeSelf)
            {
                break;
            }

            yield return null;
        }

        isRestraint = false;
        anim.speed = 1f;
        this.speed = speed;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    private IEnumerator Burning() // ȭ��
    {
        spriteRenderer.color = new Color(1, 0.7f, 0.7f, 1);

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;
            if (lerpTime < burnningDamage)
            {
                burnningDamage--;
                EnemyDamaged(Mathf.Floor(GameManager.instance.attack * 0.5f), 2);
                anim.SetTrigger("Hit");
            }
            yield return null;
        }

        statusEffect = EnemyStatusEffect.Defalt;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    private IEnumerator Wetting() // ���� ���¶�� ���� �������� ++ 
    {

        spriteRenderer.color = new Color(0.6f, 0.6f, 1, 1);

        isWetting = true;

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        statusEffect = EnemyStatusEffect.Defalt;

        isWetting = false;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    private IEnumerator ReducedSpeed() // �� �Ӽ� ������ ���� ���¶�� �̵��ӵ� --
    {
        float speed = this.speed;

        spriteRenderer.color = new Color(1, 0.6f, 0.3f, 1);

        anim.speed = 1 - speedReducePer;

        this.speed -= this.speed * speedReducePer;

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        statusEffect = EnemyStatusEffect.Defalt;

        this.speed = speed;
        anim.speed = 1;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private IEnumerator Restraint() // Ǯ �Ӽ� ������ ���� ���¶�� �ӹ�
    {
        float speed = this.speed;


        spriteRenderer.color = new Color(0, 1, 0, 1);

        isRestraint = true;
        anim.speed = 0f;
        gameObject.layer = 9;
        spriteRenderer.sortingOrder = 2; // �ӹڴ��� ���� �������� Enemy�� ������ �ʱ��ϱ����� 
        this.speed = 0f;
        rigid.velocity = Vector2.zero;

        anim.ResetTrigger(hitAnimID);
        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        isRestraint = false;

        statusEffect = EnemyStatusEffect.Defalt;

        anim.speed = 1f;
        spriteRenderer.sortingOrder = 3;
        gameObject.layer = 6;
        this.speed = speed;
        spriteRenderer.color = new Color(1, 1, 1, 1);

    }
    private IEnumerator DarknessExplosion() // �� �Ӽ� ������ ���� ���¶�� �̵��ӵ� --
    {

        spriteRenderer.color = new Color(1, 0.45f, 1, 1);

        lerpTime = darknessExpTime;

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        Transform exp = GameManager.instance.magicManager.Get(1).transform;

        exp.position = transform.position;

        float expScale = (GameManager.instance.weaponNum - 1) * 0.15f;
        exp.localScale = new Vector3(1, 1, 1) + new Vector3(expScale, expScale, expScale);

        statusEffect = EnemyStatusEffect.Defalt;

        spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    public void EnemyDamaged(float damage, int hitType)
    {
        if (isWetting && hitType ==2)
        {
            damage *= wetDamagePer;
        }

        int damageValue = 0;

        if (hitType == 1 && GameManager.instance.attribute == ItemAttribute.Holy)
        {
            int random = Random.Range(1, 101);

            if (GameManager.instance.instantKillPer >= random) // ���
            {
                damageValue = 999;
            }
        }

        if (damageValue == 0) // ��簡 �ƴ϶��
        {
            damageValue = (int)Mathf.Floor(damage);
        }

        health -= damageValue;

        Vector3 textPos = transform.position + new Vector3(0, 0.5f, 0);

        GameManager.instance.damageTextPool.Get(damageValue, textPos);
    }
    private void Dead()
    {
        gameObject.SetActive(false);
    }


}
