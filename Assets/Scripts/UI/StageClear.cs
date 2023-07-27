using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageClear : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    public RewardManager rewardManager;
    public Image reward;

    private int chestType;
    private Animator chestAnim;
    private Image chestImage;
    private Button[] btn;

    private void Awake()
    {
        chestAnim = GetComponentInChildren<Animator>();
        chestImage = GetComponentInChildren<Image>();
        btn = GetComponentsInChildren<Button>(true);
    }

    public void ChestOpen()
    {
        chestAnim.SetTrigger("Open");

        GetReward();

        btn[0].interactable = false;


        StartCoroutine(Delay(1f));
    }
    private void GetReward()
    {
        int[] chestReward = new int[4];

        if (chestType == 0)
        {
            chestReward = GameManager.instance.bronzeChest;
        }
        else if (chestType == 1)
        {
            chestReward = GameManager.instance.silverChest;
        }
        else if (chestType == 2)
        {
            chestReward = GameManager.instance.goldChest;
        }
        else if (chestType == 3)
        {
            chestReward = GameManager.instance.specialChest;
        }

        // 상자 당 ItemRank의 확률을 구함
        RandomValue(chestReward, 1);

    }
    public void NextStage()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        btn[0].gameObject.SetActive(false);

        btn[1].gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        btn[0].gameObject.SetActive(true);
        btn[1].gameObject.SetActive(false);
        btn[0].interactable = true;
        reward.color = new Color(1, 1, 1, 0);
        chestImage.color = new Color(1, 1, 1, 1);

        // 랜덤 값을 구해 정해져있는 확률대로 상자 등장
        RandomValue(GameManager.instance.chestPercent, 0);

    }

    private void RandomValue(int[] percent, int randomType)
    {
        // 랜덤 값을 구해 정해져있는 확률대로 상자 등장
        int random = Random.Range(1, 101);
        int percentSum = 0;

        for (int i = 0; i < percent.Length; i++)
        {
            percentSum += percent[i];

            if (random <= percentSum)
            {
                if (randomType == 0)
                {
                    chestType = i;
                    chestAnim.runtimeAnimatorController = animCon[i];
                }
                else
                {
                    rewardManager.ItemCreate(i);
                }
                break;
            }
        }
    }

}
