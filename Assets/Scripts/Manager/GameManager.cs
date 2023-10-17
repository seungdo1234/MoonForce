using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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
    public ShopManager shopManager;
    public NextStageBtn nextStageBtn;
    public Enchant enchant;
    public CinemachineVirtualCamera virtualCamera;
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
    public bool isResurrection;
    public bool demeterOn;

    [Header("# UI")]
    public GameObject clearReward;
    public GameObject hud;
    public RedMoonEffect redMoon;
    public GameObject gameOverObject;
    public GameObject gameClearObject;
    public GameObject background;
    public FloatingJoystick joy;

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
        background.SetActive(true);
        hud.SetActive(false);
        // ������ ������ ���̽� �ʱ�ȭ
        ItemDatabase.instance.ItemReset();
        map.MapReset();
        PoolingReset();
        magicManager.MagicActiveCancel();
        player.transform.position = Vector3.zero;
        virtualCamera.Follow = null;
        virtualCamera.transform.position = new Vector3 (0,0,-10);
        player.gameObject.SetActive(false);
        AudioManager.instance.PlayBgm((int)Bgm.Main);
    }
    public void PoolingReset()
    {
        magicManager.PoolingReset();
        pool.PoolingReset();
    }
    private IEnumerator StageClear() // �������� Ŭ����
    {
        background.SetActive(true);
        isStage = false;
        gameStop = true;
        if (isRedMoon) // ���� ���� ���ö��� ��
        {
            redMoon.RedMoonEnd();
            isRedMoon = false;
        }
        redMoonEffect = false;
        joy.StageClear();
        hud.SetActive(false);
        PoolingReset();  // ��� ���� ������Ʈ ��Ȱ��ȭ
        pause.gameObject.SetActive(false); // ���� ��ư ��Ȱ��ȭ
        AudioManager.instance.EndBgm(); // Bgm ����
        GameManager.instance.demeterOn = false;
        yield return new WaitForSeconds(1f);

        if(level >= 49) // ������ ��������
        {
            GameEnd(0);
        }
        else
        {
            
            AudioManager.instance.PlayBgm((int)Bgm.Victory - 1); // �¸� Bgm ���
            yield return new WaitForSeconds(2f);
            availablePoint++;
            level++;
            nextStageBtn.LevelText(level); // ���� �������� ��ư �ؽ�Ʈ ����
            enemyMaxNum += 10;
            if (level % 5 == 0) // �� ���� ��
            {
                spawner.EnemyLevelUp();
            }
            clearReward.SetActive(true); // Ŭ���� ���� On
            player.gameObject.SetActive(false); // �÷��̾� ��Ȱ��ȭ
            shop.ShopReset(); // ���� �ʱ�ȭ
            if (statManager.essenceOn) // �̹� ���������� ������ Ȱ��ȭ �ƴٸ�
            {
                statManager.EssenceOff();
            }
        }

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

    public void GameEnd(int endType)
    {
        if (endType == 0)
        {
            AudioManager.instance.PlayBgm((int)Bgm.GameClear - 1);
            gameClearObject.SetActive(true);
        }
        else
        {
            gameStop = true;
            joy.StageClear();
            hud.SetActive(false);
            pause.gameObject.SetActive(false);
            AudioManager.instance.PlayBgm((int)Bgm.GameOver);
            gameOverObject.SetActive(true);
        }

        level = 0;
        spawner.spawnPerLevelUp = 0;
        
    }


    public void GameStart()
    {
        virtualCamera.Follow = player.transform;
        isResurrection = false;
        level = 0;
        gold = 0;
        enemyMaxNum = 5;
        spawner.spawnPerLevelUp = 0;
        AudioManager.instance.SelectSfx();
        statManager.GameStart();
        AudioManager.instance.PlayBgm((int)Bgm.MaintenanceRoom);
        rewardManager.ItemCreate(-1 , 0);
        shop.ShopReset();
        GameManager.instance.demeterOn = false;
        nextStageBtn.LevelText(level);
    }
    public void GameQuit()
    {
        AudioManager.instance.SelectSfx();
        Application.Quit();
    }
}
