using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inferno : MonoBehaviour // ȭ���Ӽ� �����帮 ������ ��ų
{

    public GameObject[] infernos; // ���丣�� ������Ʈ


    public float coolTime;
    private Player player;
    private void Start()
    {
        player = GameManager.instance.player;
    }
    public void Init(float coolTime)
    {
        this.coolTime = coolTime;

        for(int i =0; i < infernos.Length; i++) // ������ �ʱ�ȭ
        {
            if (infernos[i].activeSelf)
            {
                infernos[i].SetActive(false);
            }
        }

        StartCoroutine(InfernoStart());
    }

    private IEnumerator InfernoStart() //����
    {
        int point = 0;
        float timer = 0;
        bool skillOn = false;
        while(!GameManager.instance.gameStop)
        {
            if(!skillOn)
            {
                timer += Time.deltaTime;
                if(timer >= coolTime)
                {
                    skillOn = true;
                    timer = 0;
                    transform.position = player.transform.position;
                }
                yield return null;
            }
            else
            {
                infernos[point].SetActive(true);
                point++;
                if(point == infernos.Length)
                {
                    point = 0;
                    skillOn = false;
                }
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

}
