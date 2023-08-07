using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // 관통

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        // 무속성일 때 마력탄 데미지 증가
        if(GameManager.instance.attribute == ItemAttribute.Non)
        {
            this.damage = damage + (damage * 0.1f);
        }
        else
        {
            this.damage = damage;
        }

        this.per = per;

        // 방향
        if (per >= 0) // per가 -1이면 근거리 공격
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
            // 속도 초기화
            rigid.velocity = Vector2.zero;
            // 재활용하기 위해 bullet 비활성화
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
