using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Text text;


    private Vector3 target;
    private RectTransform rect;
    private void Awake()
    {
        text = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
//        StartCoroutine(Alpha(1,0));
    }

    public void Init(int damage, Vector3 target)
    {
        this.target = target;
        text.text = string.Format("{0}", damage);

        gameObject.SetActive(true);

    }

    public void TextEnd()
    {
        target = Vector3.zero;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(target != Vector3.zero)
        {
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target);
            rect.position = targetScreenPos;
        }
    }
}
