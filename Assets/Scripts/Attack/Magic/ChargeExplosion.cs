using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeExplosion : MonoBehaviour
{
    public float lerpTime;

    private Animator anim;
    private CircleCollider2D col;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
    }

    public void Init(float coolTime)
    {
        lerpTime = coolTime;

        StartCoroutine(SkillStart());
    }

    private IEnumerator SkillStart()
    {
        float curTime = 0;
        while (true)
        {
            curTime += Time.deltaTime;

            if (curTime > lerpTime)
            {
                curTime = 0;
                anim.SetTrigger("Explosion");
            }

            yield return null;
        }
    }

    private void Update()
    {
        transform.position = GameManager.instance.player.transform.position;
    }
}
