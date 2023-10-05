using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;

    public GameObject resurrection;

    [Header("Body of Rotation")]
    public Transform rotationBody;

    [Header("Attack Target")]
    public Scanner scanner;

    private bool isDamaged;
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    private void OnMove(InputValue value) // Input System���� �ڵ� ȣ��
    {
        if (!GameManager.instance.gameStop)
        {
            // value�� InputValue ���̹Ƿ� Vector2 ������ ��ȯ ���Ѿ��� -> Get<T>() ���� value�� ���� ��ȯ
            inputVec = value.Get<Vector2>();

        }
        else
        {
            inputVec = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameStop)
        {
            return;
        }
        // normalized�� �밢������ ���� �����¿�� ���� �ӵ��� �������� vecter ���� ����ȭ��
        // fixedDeltaTime�� FixedUpdate���� ���
        Vector2 nextVec = inputVec * Time.fixedDeltaTime * GameManager.instance.statManager.moveSpeed;
        // ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);

   
    }
    private void LateUpdate()
    {
        if (GameManager.instance.gameStop)
        {
            anim.SetFloat("Run", 0);
            return;
        }

        // magnitude : ���� ���� ���� ����
        anim.SetFloat("Run", inputVec.magnitude);

        if(inputVec.magnitude != 0)
        {
            AudioManager.instance.FootStepSfxPlayer();
        }

        if (inputVec.x != 0)
        {
            // inputVec.x < 0�� ���̶�� 1, �����̶�� -1�� ���̸� 1�̹Ƿ� flipX�� true�� ��. 
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private IEnumerator OnDamaged() // �������� �޾��� �� 2�ʵ��� �������� �����ʴ� ���°� �ǰ� �̵��ӵ��� ������
    {
        GameManager.instance.cameraShake.ShakeCamera(0.5f, 5, 5);
        gameObject.tag = "PlayerDamaged";
        gameObject.layer = 8;
        GameManager.instance.statManager.moveSpeed *= 1.2f;
        spriteRenderer.color = new Color(1, 1, 1, 0.7f);
        yield return new WaitForSeconds(2f);
        GameManager.instance.statManager.moveSpeed /= 1.2f;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        gameObject.tag = "Player";
        gameObject.layer = 7;
        isDamaged = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.gameStop || !collision.gameObject.CompareTag("Enemy") || isDamaged)
        {
            return;
        }
        isDamaged = true; 

        int damage = collision.gameObject.GetComponent<Enemy>().damage;

        PlayerHit(damage);

    }
    private IEnumerator Resurrection()
    {
        resurrection.SetActive(true);

        yield return new WaitForSeconds(1.4f);

        resurrection.SetActive(false);
    }
    public void PlayerHit(int damage)
    {

        GameManager.instance.statManager.curHealth -= damage;
        //   AudioManager.instance.PlayerSfx(AudioManager.Sfx.Hit);

        if (GameManager.instance.statManager.curHealth > 0)
        {
            AudioManager.instance.PlayerSfx(Sfx.Hurt);

            StartCoroutine(OnDamaged());
        }
        else if (GameManager.instance.statManager.curHealth <= 0)
        {
            if(GameManager.instance.attribute == ItemAttribute.Holy && !GameManager.instance.isResurrection)
            {
                GameManager.instance.isResurrection = true;
                StartCoroutine(OnDamaged());
                GameManager.instance.statManager.curHealth = 10;
                StartCoroutine(Resurrection());
                return;
            }
            isDamaged = false;
            anim.SetTrigger("Dead");
            GameManager.instance.GameEnd(1);

        }
    }
}
