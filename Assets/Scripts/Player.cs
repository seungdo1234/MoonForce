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

    private void OnMove(InputValue value) // Input System으로 자동 호출
    {

        // value는 InputValue 값이므로 Vector2 값으로 변환 시켜야함 -> Get<T>() 으로 value의 형식 변환
        inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // normalized는 대각선으로 갈때 상하좌우와 같은 속도를 내기위해 vecter 값을 정규화함
        // fixedDeltaTime은 FixedUpdate에서 사용
        Vector2 nextVec = inputVec * Time.fixedDeltaTime * moveSpeed;
        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void LateUpdate()
    {
        // magnitude : 벡터 값의 순수 길이
        anim.SetFloat("Run", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            // inputVec.x < 0이 참이라면 1, 거짓이라면 -1로 참이면 1이므로 flipX가 true가 됨. 
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
            // transform.childCount : 자식 오브젝트 갯수 반환
            for (int i = 2; i < transform.childCount; i++)
            {
                // 자식 오브젝트 선택
                transform.GetChild(i).gameObject.SetActive(false);
            }
        //    anim.SetTrigger("Dead");
         //   GameManager.instance.GameOver();

        }

    }
}
