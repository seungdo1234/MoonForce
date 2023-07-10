using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float moveSpeed;

    [Header("Attack Target")]
    public Scanner scanner;

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
        if(inputVec.x != 0)
        {
            // inputVec.x < 0�� ���̶�� 1, �����̶�� -1�� ���̸� 1�̹Ƿ� flipX�� true�� ��. 
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*
        if(!GameManager.instance.isLive)
        {
            return;
        }
        GameManager.instance.health -= Time.deltaTime * 10;

        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Hit);

        if (GameManager.instance.health < 0)
        {
            // transform.childCount : �ڽ� ������Ʈ ���� ��ȯ
            for (int i = 2; i<transform.childCount; i++)
            {
                // �ڽ� ������Ʈ ����
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();

        }
        */
    }
}
