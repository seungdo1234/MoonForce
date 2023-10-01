using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMeteor : MonoBehaviour
{
    public Transform[] wayPoints;

    public float delayTime;
    public float coolTime;

    private Player player;
    private void Start()
    {
        player = GameManager.instance.player;
    }
   
    public void Init(float coolTime)
    {
        this.coolTime = coolTime;

        for(int i= 0; i<wayPoints.Length; i++)
        {
            if (wayPoints[i].gameObject.activeSelf)
            {
                wayPoints[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine(RockMeteorStart());
    }

    private IEnumerator RockMeteorStart()
    {
        int point = 0;
        float curTime = 0;
        bool ready = false;
        while (true)
        {
            if (!ready)
            {
                curTime += Time.deltaTime;
                if(curTime >= coolTime)
                {
                    gameObject.transform.position = player.transform.position;
                    wayPoints = ArrayShuffle(wayPoints);
                    ready = true;
                }
                yield return null;

            }
            else
            {
                wayPoints[point].gameObject.SetActive(true);
                point++;
                yield return new WaitForSeconds(delayTime);
                if (point == wayPoints.Length)
                {
                    ready = false;
                    curTime = 0;
                    point = 0;
                }
            }


            
        }
    }

  private T[] ArrayShuffle<T> (T[] array)
    {
        T temp;
        int random1, random2;

        for (int i = 0; i < array.Length; ++i)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array;
    }

}
