using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageClear : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    public Image reward;

    private int rewardType;
    private int chestType;
    private Animator chestAnim;
    private Image chestImage;
    public GameObject rewardTypeSelecet;
    public Button chestOpenBtn;
    public Button nextBtn;

    private void Awake()
    {
        chestAnim = GetComponentInChildren<Animator>(true);
        chestImage = GetComponentInChildren<Image>(true);

    }

    public void RewardSelect(int rewardType)
    {
        AudioManager.instance.SelectSfx();
        this.rewardType = rewardType;
        rewardTypeSelecet.SetActive(false);
        chestOpenBtn.gameObject.SetActive(true);


        chestAnim.gameObject.SetActive(true);
        
        if(rewardType == 0)
        {
            RandomValue(GameManager.instance.chestPercent, 0);
        }
        else
        {
            RandomValue(GameManager.instance.itemQualityPercent, 0);
        }
    }

    public void ChestOpen()
    {
        chestAnim.SetTrigger("Open");

        AudioManager.instance.PlayerSfx(Sfx.ChestOpen);

        GetReward();

        chestOpenBtn.interactable = false;


        StartCoroutine(Delay(1f));
    }
    private void GetReward()
    {
        int[] chestReward = new int[4];

        if(rewardType == 0)
        {
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
        else
        {
            GameManager.instance.rewardManager.ItemCreate(chestType, 1);
        }

    }
    public void NextStage()
    {
        gameObject.SetActive(false);
        chestImage.color = new Color(1, 1, 1, 1);
        chestAnim.gameObject.SetActive(false);
        reward.color = new Color(1, 1, 1, 0);
        rewardTypeSelecet.SetActive(true);
        chestOpenBtn.interactable = true;
        nextBtn.gameObject.SetActive(false);
    }

    private IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        chestOpenBtn.gameObject.SetActive(false);

        nextBtn.gameObject.SetActive(true);
    }

    public void ShopItemCreate(int[] percent, int randomType, int rewardType) // 상점 아이템 생성 함수
    {
        this.rewardType = rewardType;

        RandomValue(percent, randomType);
    }
    public void RandomValue(int[] percent, int randomType )
    {
        // 랜덤 값을 구해 정해져있는 확률대로 상자 등장
        int random = Random.Range(1, 101);
        int percentSum = 0;

        for (int i = 0; i < percent.Length; i++)
        {
            percentSum += percent[i];
            if (random <= percentSum)
            {
                if (randomType == 0) // 스테이지 클리어 보상 상자 생성
                {
                    chestType = i;
                    chestAnim.runtimeAnimatorController = animCon[i];
                }
                else if (randomType == 1) //아이템(스태프, 마법책) 생성
                {
                    // rewardType: 0,2는 스태프, 1,3은 마법책
                    GameManager.instance.rewardManager.ItemCreate(i, rewardType);
                }
                else if (randomType == 2) // 포션 생성
                {
                    GameManager.instance.shop.PosionCreate(i);
                }
                break;
            }
        }
    }

}
