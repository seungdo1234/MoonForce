using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Text text;

    public float lerpTime;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(Alpha(1,0));
    }

    private IEnumerator Alpha(int start, int end)
    {
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, currentTime / lerpTime);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
