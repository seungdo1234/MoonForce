using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // ����

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        // ���Ӽ��� �� ����ź ������ ����
        if(GameManager.instance.attribute == ItemAttribute.Non)
        {
            this.damage = damage + (damage * 0.1f);
        }
        else
        {
            this.damage = damage;
        }

        this.per = per;

        // ����
        if (per >= 0) // per�� -1�̸� �ٰŸ� ����
        {
            rigid.velocity = dir * 10f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
        {
            return;
        }

        per--;

        if (per == -1)
        {
            // �ӵ� �ʱ�ȭ
            rigid.velocity = Vector2.zero;
            // ��Ȱ���ϱ� ���� bullet ��Ȱ��ȭ
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
        {
            return;
        }

        gameObject.SetActive(false);
    }
}
