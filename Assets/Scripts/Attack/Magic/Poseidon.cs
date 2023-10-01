using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poseidon : MonoBehaviour
{
    public GameObject[] posedions;
    public float coolTime;
    public void Init(float coolTime)
    {
        this.coolTime = coolTime;

        for (int i = 0; i < posedions.Length; i++) // 생성전 초기화
        {
            if (posedions[i].activeSelf)
            {
                posedions[i].SetActive(false);
            }
        }

        StartCoroutine(PosedionsStart());
    }
    private IEnumerator PosedionsStart()
    {

        while (!GameManager.instance.gameStop)
        {
            yield return new WaitForSeconds(coolTime);

            transform.position = GameManager.instance.player.transform.position;

            for (int i =0;i <posedions.Length; i++)
            {
                posedions[i].SetActive(true);
            }
        }
    }
}
