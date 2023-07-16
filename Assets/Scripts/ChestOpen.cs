using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestOpen : MonoBehaviour
{
    public Image reward;

    public float lerpTime;

    public void Open()
    {
        StartCoroutine(Alpha(0, 1));
    }
    private IEnumerator Alpha(float start, float end)
    {
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, currentTime / lerpTime);
            reward.color = new Color(reward.color.r, reward.color.g, reward.color.b, alpha);
            yield return null;
        }
    }
}
