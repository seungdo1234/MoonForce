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
            // ���� �� ItemRank�� Ȯ���� ����
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

    public void ShopItemCreate(int[] percent, int randomType, int rewardType) // ���� ������ ���� �Լ�
    {
        this.rewardType = rewardType;

        RandomValue(percent, randomType);
    }
    public void RandomValue(int[] percent, int randomType )
    {
        // ���� ���� ���� �������ִ� Ȯ����� ���� ����
        int random = Random.Range(1, 101);
        int percentSum = 0;

        for (int i = 0; i < percent.Length; i++)
        {
            percentSum += percent[i];
            if (random <= percentSum)
            {
                if (randomType == 0) // �������� Ŭ���� ���� ���� ����
                {
                    chestType = i;
                    chestAnim.runtimeAnimatorController = animCon[i];
                }
                else if (randomType == 1) //������(������, ����å) ����
                {
                    // rewardType: 0,2�� ������, 1,3�� ����å
                    GameManager.instance.rewardManager.ItemCreate(i, rewardType);
                }
                else if (randomType == 2) // ���� ����
                {
                    GameManager.instance.shop.PosionCreate(i);
                }
                break;
            }
        }
    }

}
