using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Pause pause;
    public Inventory inventory;
    public RewardManager rewardManager;
    public Shop shop;
    private Spawner spawner;

    [Header("# Player Data")]
    public int availablePoint; // ���������� ���������� ���� ������ �ø� �� �ִ� ����Ʈ
    public ItemAttribute attribute; // Player�� ���� �Ӽ�
    public int gold; // ���

    [Header("# Stage Data")]
    public bool gameStop; // ������ ������ �� true
    public bool isStage; // ���������� ���� �� �϶� True
    public bool isRedMoon; // ���� �ð��ȿ� Enemy�� ���� ������ �� Enemy�� �������� ���� ���� ������\
    public bool redMoonEffect; // ����� ������ ��
    public float maxGameTime; 
    public float curGameTime;
    public int kill;
    public int enemyMaxNum;
    public int enemyCurNum = 9999;
    public int level;
    public int enemySpawnNumIncrese;

    [Header("# UI")]
    public GameObject clearReward;
    public GameObject hud;
    public RedMoonEffect redMoon;
    public GameObject gameOverObject;

    [Header("MainMenu")]
    public Map map;

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
    private bool isClear()
    {
        if(kill == enemyMaxNum)
        {
            return true;
        }
        return false;
    }
    public void GameLobby() // �κ�� �� �� (�װų�, ���� â���� ���ų�)
    {
        // ������ ������ ���̽� �ʱ�ȭ
        ItemDatabase.instance.ItemReset();
        map.MapReset();
        PoolingReset();
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(false);
    }
    public void PoolingReset()
    {
        magicManager.PoolingReset();
        pool.PoolingReset();
    }
    private IEnumerator StageClear() // �������� Ŭ����
    {
        isStage = false;
        gameStop = true;
        if (isRedMoon) // ���� ���� ���ö��� ��
        {
            redMoon.RedMoonEnd();
            isRedMoon = false;
        }
        redMoonEffect = false;
        hud.SetActive(false);
        PoolingReset();
        AudioManager.instance.EndBgm(); // Bgm ����
        yield return new WaitForSeconds(1f);
        pause.gameObject.SetActive(false); // ���� ��ư ��Ȱ��ȭ
        AudioManager.instance.PlayBgm((int)Bgm.Victory); // �¸� Bgm ���
        yield return new WaitForSeconds(2f);
        availablePoint++; 
        level++;

        enemyMaxNum += 10;
        if(level % 5 == 0)
        {
            spawner.EnemyLevelUp();
        }
        clearReward.SetActive(true);
        player.gameObject.SetActive(false);
        //    Time.timeScale = 0f;
    }
    public void NextStage()
    {
        isStage = true;
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
        
        if(!redMoonEffect && curGameTime <= redMoon.lerpTime) // ȭ���� �������� ��
        {
            redMoon.RedMoonStart();
            redMoonEffect = true;
        }
        else if (curGameTime <= 0) // ���� �� On
        {
            curGameTime = 0;
            isRedMoon = true;
        }
        
    }

    public void GameOver()
    {
        gameStop = true;

        AudioManager.instance.PlayBgm((int)Bgm.GameOver);
        hud.SetActive(false);
        pause.gameObject.SetActive(false);
        gameOverObject.SetActive(true);
    }
    public void GameStart()
    {
        AudioManager.instance.SelectSfx();
        statManager.GameStart();
        AudioManager.instance.PlayBgm((int)Bgm.MaintenanceRoom);
        rewardManager.ItemCreate(-1 , 0);
    }
}
