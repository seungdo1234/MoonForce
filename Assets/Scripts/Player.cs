using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float moveSpeed;

    public HUD healthUI;

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

        // value�� InputValue ���̹Ƿ� Vector2 ������ ��ȯ ���Ѿ��� -> Get<T>() ���� value�� ���� ��ȯ
        inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // normalized�� �밢������ ���� �����¿�� ���� �ӵ��� �������� vecter ���� ����ȭ��
        // fixedDeltaTime�� FixedUpdate���� ���
        Vector2 nextVec = inputVec * Time.fixedDeltaTime * moveSpeed;
        // ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void LateUpdate()
    {
        // magnitude : ���� ���� ���� ����
        anim.SetFloat("Run", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            // inputVec.x < 0�� ���̶�� 1, �����̶�� -1�� ���̸� 1�̹Ƿ� flipX�� true�� ��. 
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private IEnumerator OnDamaged()
    {
        gameObject.tag = "PlayerDamaged";
        gameObject.layer = 8;
        moveSpeed *= 2;
        spriteRenderer.color = new Color(1, 1, 1, 0.7f);
        yield return new WaitForSeconds(2f);
        moveSpeed /= 2;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        gameObject.tag = "Player";
        gameObject.layer = 7;
        isDamaged = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.instance.gameStop && !collision.gameObject.CompareTag("Enemy") && !isDamaged)
        {
            return;
        }
        isDamaged = true;

        GameManager.instance.curHealth--;
        healthUI.PlayerHit();
        //   AudioManager.instance.PlayerSfx(AudioManager.Sfx.Hit);

        if (GameManager.instance.curHealth > 0)
        {
            StartCoroutine(OnDamaged());
        }
        else if (GameManager.instance.curHealth == 0)
        {
            // transform.childCount : �ڽ� ������Ʈ ���� ��ȯ
            for (int i = 2; i < transform.childCount; i++)
            {
                // �ڽ� ������Ʈ ����
                transform.GetChild(i).gameObject.SetActive(false);
            }
        //    anim.SetTrigger("Dead");
         //   GameManager.instance.GameOver();

        }

    }
}
