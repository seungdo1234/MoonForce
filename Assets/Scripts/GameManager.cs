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
    public CameraShake cameraShake;
    private Spawner spawner;

    [Header("# Player Data")]
    public int availablePoint; // ���������� ���������� ���� ������ �ø� �� �ִ� ����Ʈ
    public ItemAttribute attribute;
    public int gold;

    [Header("# Stage Data")]
    public bool gameStop; // ������ ������ �� true
    public bool isRedMoon; // ���� �ð��ȿ� Enemy�� ���� ������ �� Enemy�� �������� ���� ���� ������\
    public bool redMoonEffect;
    public float maxGameTime;
    public float curGameTime;
    public int kill;
    public int enemyMaxNum;
    public int enemyCurNum = 9999;
    public int level;

    [Header("# UI")]
    public GameObject clearReward;
    public GameObject startBtn;
    public GameObject hud;
    public RedMoonEffect redMoon;

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
        spawner = player.GetComponentInChildren<Spawner>();
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
        if(kill == enemyMaxNum)
        {
            return true;
        }
        return false;
    }
    private IEnumerator StageClear()
    {

        gameStop = true;
        if (isRedMoon)
        {
            redMoon.RedMoonEnd();
            isRedMoon = false;
        }
        redMoonEffect = false;
        hud.SetActive(false);
        magicManager.StageClear();
        pool.StageClear();
        AudioManager.instance.EndBgm();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayBgm(3);
        yield return new WaitForSeconds(2f);
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
        spawner.StageStart();
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
            return;
        }

        if (isRedMoon)
        {
            return;
        }

        curGameTime -= Time.deltaTime;
        
        if(!redMoonEffect && curGameTime <= redMoon.lerpTime)
        {
            redMoon.RedMoonStart();
            redMoonEffect = true;
        }
        else if (curGameTime <= 0)
        {
            curGameTime = 0;
            isRedMoon = true;
        }
        
    }

}
