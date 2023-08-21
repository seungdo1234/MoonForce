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
    public bool enemyKnockBack; // 넉백일 때
    public bool enemyDamaged; // 데미지를 받았다면 일정 시간동안 안받게함
    public float damagedTime; // 몇초동안 무적일건지
    public float knockBackValue; // 피격 시 넉백 수치

    [Header("# EnemyStatusEffect")]
    public EnemyStatusEffect statusEffect;
    public float burningEffectTime;
    public float wettingEffectTime;
    public bool isWetting; // wet 상태인지 (맞다면 마법 데미지 ++)
    public float wetDamagePer;
    public float restraintEffectTime;
    public bool isRestraint;
    public float speedReducedEffectTime;
    public float speedReducePer;
    public float darknessExpTime;

    private int burnningDamage;
    private float lerpTime;

    [Header("# EnemyType")]
    // 애니메이터의 데이터를 바꾸는 컴포넌트 => RuntimeAnimatorController
    public RuntimeAnimatorController[] animCon;


    [Header("# TargetPlayer")]
    public Rigidbody2D target; // 타겟


    private bool isLive; // Enmey가 살아있는지

    private int hitAnimID;
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
        hitAnimID = Animator.StringToHash("Hit");
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
        if (!enemyKnockBack) // 넉백 중 일때 속도를 초기화하면 안되기 때문에
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
        // 스프라이트 방향 전환 -> 플레이어가 Enmey보다 왼쪽에 있을 때
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    // 스크립트가 활성화 될 때, 호출하는 함수
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true; // 생존 여부
        enemyDamaged = false;
        // Enemy가 죽었을 때 오브젝트 폴링에 의해 다시 생겨날 때 health를 maxHealth로 초기화
        health = maxHealth;
        col.enabled = true; // 콜라이더 활성화
        rigid.simulated = true; // rigidbody2D 활성화
        spriteRenderer.sortingOrder = 3; // OrderLayer를 3로 내림
        spriteRenderer.color = new Color(1, 1, 1, 1); // 컬러 초기화
        statusEffect = EnemyStatusEffect.Defalt; // 상태 초기화
        gameObject.layer = 6; // 레이어를 Enemy로 변경
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
        if (!(collision.gameObject.layer == 10 || collision.gameObject.layer == 11) || !isLive || enemyDamaged)
        {
            return;
        }

        if (collision.gameObject.layer == 11) // 만약 마력탄이라면
        {
            knockBackValue = GameManager.instance.knockBackValue;
            EnemyDamaged(collision.GetComponent<Bullet>().damage, 1);
            StatusEffect();
        }
        else // 마법이라면 
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
            if (anim.speed != 1) // 디버프 상태에서 죽는다면
            {
                anim.speed = 1f;
            }
            isRestraint = false;
            spriteRenderer.color = new Color(1, 1, 1, 1); // 색깔 되 돌리기
            isLive = false; // 죽었다 체크
            col.enabled = false; // 콜라이더 비활성화
            rigid.simulated = false; // rigidbody2D 정지
            spriteRenderer.sortingOrder = 1; // 죽은 ENemy가 다른 Enemy를 가리지 않도록 OrderLayer를 1로 내림
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) // 지속적인 피해를 주는 마법과 충돌 중 일때
    {
        if (collision.gameObject.layer != 10 || enemyDamaged || !isLive)
        {
            return;
        }

        int number = collision.GetComponent<MagicNumber>().magicNumber;

        if (number != 9 && number != 11) // 지속적인 피해를 주는 마법이 아니라면
        {
            return;
        }
        StartCoroutine(IsDamaged());

        float damage = GameManager.instance.attack * GameManager.instance.magicManager.magicInfo[number].damagePer;

        EnemyDamaged(damage, 2);

        EnemyHit();
    }
    private IEnumerator IsDamaged() // Enemy가 지속적인 피해를 주는 마법 위에 있을경우 일정 시간뒤에 데미지를 받기 위한 코루틴
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
        yield return wait; // 다음 하나의 물리 프레임을 딜레이
        rigid.mass = 100;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * knockBackValue, ForceMode2D.Impulse);
        yield return wait; // 다음 하나의 물리 프레임을 딜레이
        rigid.mass = 1;
        enemyKnockBack = false;

    }
    private IEnumerator ElectricShock(GameObject elec) // 전기쇼크를 맞았을 때 감전
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
    private IEnumerator Burning() // 화상
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

    private IEnumerator Wetting() // 젖은 상태라면 마법 데미지가 ++ 
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
    private IEnumerator ReducedSpeed() // 땅 속성 공격을 받은 상태라면 이동속도 --
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

    private IEnumerator Restraint() // 풀 속성 공격을 받은 상태라면 속박
    {
        float speed = this.speed;


        spriteRenderer.color = new Color(0, 1, 0, 1);

        isRestraint = true;
        anim.speed = 0f;
        gameObject.layer = 9;
        spriteRenderer.sortingOrder = 2; // 속박당한 적이 지나가는 Enemy를 가리지 않기하기위해 
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
    private IEnumerator DarknessExplosion() // 땅 속성 공격을 받은 상태라면 이동속도 --
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

            if (GameManager.instance.instantKillPer >= random) // 즉사
            {
                damageValue = 999;
            }
        }

        if (damageValue == 0) // 즉사가 아니라면
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
