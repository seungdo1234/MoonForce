using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClear : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;

    private Animator chestAnim;
    private Image chestImage;

    private void Awake()
    {
        chestAnim = GetComponentInChildren<Animator>();
        chestImage = GetComponentInChildren<Image>();
    }
   
    public void ChestOpen()
    {
        chestAnim.SetTrigger("Open");
    }

    private void OnEnable()
    {
        chestImage.color = new Color(1,1,1,1);

        // 랜덤 값을 구해 정해져있는 확률대로 상자 등장
        int random = Random.Range(1, 101);
        int percentSum = 0;

        for (int i = 0; i < GameManager.instance.chestPercent.Length; i++)
        {
            percentSum += GameManager.instance.chestPercent[i];

            if (random <= percentSum)
            {
                chestAnim.runtimeAnimatorController = animCon[i];
                break;
            }
        }

    }

}
