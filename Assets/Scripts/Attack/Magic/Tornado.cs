using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float lerpTime;

    private Animator anim;
    private PolygonCollider2D col;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<PolygonCollider2D>();
    }

    private void OnEnable()
    {
        col.enabled = true;

        StartCoroutine(TornadoPlay());
    }

    private IEnumerator TornadoPlay()
    {

        yield return new WaitForSeconds(lerpTime);

        anim.SetTrigger("TornadoExtinction");

        col.enabled = false;

    }
}
