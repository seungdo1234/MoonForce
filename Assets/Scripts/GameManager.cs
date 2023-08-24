using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �ٸ� ��ũ��Ʈ������ ���� �����ϱ� ���� GameManager �ν��Ͻ� ȭ
    public static GameManager instance;
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public DamageTextPool damageTextPool;
    public MagicManager magicManager;
    public StatManager statManager;

    [Header("# Player Data")]
    public int availablePoint; // ���������� ���������� ���� ������ �ø� �� �ִ� ����Ʈ
    public ItemAttribute attribute;


    [Header("# Stage Data")]
    public bool gameStop; // ������ ������ �� true
    public float maxGameTime;
    public float curGameTime;
    public int kill;
    public int[] enemyMaxNum;
    public int enemyCurNum = 9999;
    public int level;

    [Header("# UI")]
    public GameObject clearReward;
    public GameObject startBtn;
    public GameObject hud;

    [Header("# Reward")]
    public int[] chestPercent;
    public int[] bronzeChest;
    public int[] silverChest;
    public int[] goldChest;
    public int[] specialChest;
    public int[] itemQualityPercent;

    private void Awake()
    {
        // �ڱ� �ڽ����� �ʱ�ȭ
        instance = this;
    }

    private void Start()
    {
      //  Time.timeScale = 0f;
    }
    public void GameStart()
    {
        startBtn.SetActive(false);
        Time.timeScale = 1f;
        curGameTime = maxGameTime;
        enemyCurNum = 0;
        kill = 0;
        gameStop = false;
        magicManager.StageStart();
    }
    private bool isClear()
    {
        if(kill == enemyMaxNum[level])
        {
            return true;
        }
        return false;
    }
    private IEnumerator StageClear()
    {
        gameStop = true;
        hud.SetActive(false);
        magicManager.StageClear();
        yield return new WaitForSeconds(3f);
        availablePoint++;
        level++;
        clearReward.SetActive(true);
        player.gameObject.SetActive(false);
        //    Time.timeScale = 0f;
    }
    public void NextStage()
    {
        //       Time.timeScale = 1f;
        player.gameObject.SetActive(true);
        gameStop = false;
        curGameTime = maxGameTime;
        enemyCurNum = 0;
        kill = 0;
        clearReward.SetActive(false);
        magicManager.StageStart();
    }
    private void Update()
    {
        if (gameStop)
        {
            return;
        }

        if (isClear())
        {
            StartCoroutine(StageClear());
        }
        curGameTime -= Time.deltaTime;
        
    }

}
