using System.Collections;
using UnityEngine;

public enum EnemyStatusEffect { Defalt, Burn, Wet, Earth, GrassRestraint, Darkness }
public class Enemy : MonoBehaviour
{
    [Header("# EnemyBaseStat")]
    public int damage;
    public float speed;
    public float health;
    public float maxHealth;
    public int enemyType;

    [Header("# EnemyHit")]
    public bool enemyKnockBack; // �˹��� ��
    public bool enemyDamaged; // �������� �޾Ҵٸ� ���� �ð����� �ȹް���
    public float damagedTime; // ���ʵ��� �����ϰ���
    public float knockBackValue; // �ǰ� �� �˹� ��ġ

    [Header("# EnemyStatusEffect")]
    public EnemyStatusEffect statusEffect;
    public bool isWetting; // wet �������� (�´ٸ� ���� ������ ++)
    public bool isRestraint;

    private int burnningDamage;
    private float lerpTime;

    [Header("# EnemyType")]
    // �ִϸ������� �����͸� �ٲٴ� ������Ʈ => RuntimeAnimatorController
    public RuntimeAnimatorController[] animCon;


    [Header("# TargetPlayer")]
    public Rigidbody2D target; // Ÿ��


    public bool isLive; // Enmey�� ����ִ���

    private RushEnemy rush;
    private RangeAttackEnemy rangeAttackEnemy;
    private int hitAnimID;
    public Rigidbody2D rigid;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
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
        rush = GetComponent<RushEnemy>();
        rangeAttackEnemy = GetComponent<RangeAttackEnemy>();
    }


    private void FixedUpdate() // �������� �̵��� FixedUpdate
    {
        if (GameManager.instance.gameStop || !isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || rush.isReady || !rangeAttackEnemy.isReady || GameManager.instance.demeterOn) // Enemy�� �׾��ٸ� return
        {

            if (GameManager.instance.gameStop || GameManager.instance.demeterOn || !isLive)
            {
                rigid.velocity = Vector2.zero;
            }
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

        if ( GameManager.instance.gameStop || isRestraint || !isLive || rush.isReady || !rangeAttackEnemy.isReady || GameManager.instance.demeterOn)
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
        enemyType = data.spriteType;
        damage = data.damage;
        rush.isReady = false;
        rangeAttackEnemy.isReady = true;

        if (enemyType == 3)
        {
            rigid.mass = 100;
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            rush.Init();
        }
        else
        {
            rigid.mass = 1;
            rush.isReady = false;
            rush.isAttack = false;
            rush.isRushing = false;

            switch (enemyType)
            {
                case 0 :
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
                case 1:
                    transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                    break;
                case 2:
                    transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    break;
                case 4:
                    transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    rangeAttackEnemy.Init();
                    break;
            }
        }
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
            knockBackValue = GameManager.instance.statManager.knockBackValue;
            StatusEffect();
            EnemyDamaged(collision.GetComponent<Bullet>().damage, 1);
        }
        else // �����̶�� 
        {
            int number = collision.GetComponent<MagicNumber>().magicNumber;
            float damage = GameManager.instance.statManager.attack * GameManager.instance.magicManager.magicInfo[number].damagePer;

            if (number == 1 || number == 15 || number == 17)
            {
                StartCoroutine(IsDamaged());
                if(number == 17)
                {
                    GameObject elec = collision.gameObject;

                    StartCoroutine(ElectricShock(elec));
                }
            }

            knockBackValue = GameManager.instance.magicManager.magicInfo[number].knockBackValue;

            EnemyDamaged(damage, 2);


        }
    }

    private void EnemyHit()
    {

        if (health > 0)
        {
            AudioManager.instance.PlayerSfx(Sfx.EnemyHit);
            // Live, Hit Action
            anim.SetTrigger("Hit");
            StartCoroutine(KnockBack());

        }
        else
        {
            if (isLive) // �ߺ� ų ���� �ذ�
            {
                AudioManager.instance.PlayerSfx(Sfx.Dead);

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

                GameManager.instance.gold += enemyType + 1;
                if (statusEffect == EnemyStatusEffect.Darkness)
                {
                    ExplosionSpawn();
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision) // �������� ���ظ� �ִ� ������ �浹 �� �϶�
    {
        if (collision.gameObject.layer != 10 || enemyDamaged || !isLive)
        {
            return;
        }

        int number = collision.GetComponent<MagicNumber>().magicNumber;

        if (number != 15 && number != 17) // �������� ���ظ� �ִ� ������ �ƴ϶��
        {
            return;
        }
        StartCoroutine(IsDamaged());

        float damage = GameManager.instance.statManager.attack * GameManager.instance.magicManager.magicInfo[number].damagePer;

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
            case ItemAttribute.Fire: // �ҼӼ� �ǰ�
                lerpTime = GameManager.instance.statManager.burningEffectTime + 1;
                burnningDamage = (int)GameManager.instance.statManager.burningEffectTime;

                if (statusEffect != EnemyStatusEffect.Burn)
                {
                    statusEffect = EnemyStatusEffect.Burn;
                    StartCoroutine(Burning());
                }
                break;
            case ItemAttribute.Water: // ���Ӽ� �ǰ�
                lerpTime = GameManager.instance.statManager.wettingEffectTime;

                if (statusEffect != EnemyStatusEffect.Wet)
                {
                    statusEffect = EnemyStatusEffect.Wet;
                    StartCoroutine(Wetting());
                }

                break;
            case ItemAttribute.Grass: // Ǯ�Ӽ� �ǰ�
                lerpTime = GameManager.instance.statManager.restraintTime;
                if (statusEffect != EnemyStatusEffect.GrassRestraint)
                {
                    statusEffect = EnemyStatusEffect.GrassRestraint;
                    StartCoroutine(Restraint());
                }

                break;
            case ItemAttribute.Eeath: // ���Ӽ� �ǰ�

                lerpTime = GameManager.instance.statManager.speedReducedEffectTime;

                if (statusEffect != EnemyStatusEffect.Earth)
                {
                    statusEffect = EnemyStatusEffect.Earth;
                    StartCoroutine(ReducedSpeed());
                }

                break;
            case ItemAttribute.Dark: // ��ҼӼ� �ǰ�

                if (statusEffect != EnemyStatusEffect.Darkness)
                {
                    statusEffect = EnemyStatusEffect.Darkness;
                    StartCoroutine(DarknessExplosion());
                }
                break;
            default:
                return;

        }
    }
    private IEnumerator KnockBack()
    {
        if (!rush.isReady)
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
        else
        {
            yield return wait; // ���� �ϳ��� ���� �������� ������
        }
       

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
                EnemyDamaged(Mathf.Floor(GameManager.instance.statManager.attack * GameManager.instance.statManager.burningDamagePer), 2);
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

        anim.speed = 1 - GameManager.instance.statManager.speedReducePer;

        this.speed -= this.speed * GameManager.instance.statManager.speedReducePer;

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

        anim.ResetTrigger(hitAnimID); // �ִϸ��̼��� ����
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
    private IEnumerator DarknessExplosion() 
        // ��� �Ӽ� ������ �¾��� �� ���� �ð� �� ����
    {

        spriteRenderer.color = new Color(1, 0.45f, 1, 1);

        lerpTime = GameManager.instance.statManager.darknessExpTime;

        while (lerpTime > 0)
        {
            lerpTime -= Time.deltaTime;

            yield return null;
        }

        ExplosionSpawn();

        statusEffect = EnemyStatusEffect.Defalt;

        spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    private void ExplosionSpawn()
    {

        Transform exp = GameManager.instance.magicManager.Get(0).transform;

        GameManager.instance.magicManager.magicInfo[0].damagePer = GameManager.instance.statManager.darkExplosionDamagePer;

        exp.position = transform.position;

        float expScale = (GameManager.instance.statManager.weaponNum - 1) * 0.15f;
        exp.localScale = new Vector3(1, 1, 1) + new Vector3(expScale, expScale, expScale);
    }

    public void EnemyDamaged(float damage, int hitType)
    {
        if (isWetting && hitType ==2)
        {
            damage *= GameManager.instance.statManager.wettingDamagePer;
        }

        int damageValue = 0;

        if (hitType == 1 && GameManager.instance.attribute == ItemAttribute.Holy)
        {
            int random = Random.Range(1, 101);

            if (GameManager.instance.statManager.instantKillPer >= random) // ���
            {
                Transform instantMotion = GameManager.instance.magicManager.Get(6).transform;
                instantMotion.position = transform.position;

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

        EnemyHit();
    }
    private void Dead()
    {
        gameObject.SetActive(false);
    }


}
