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
    public SkillCoolTimeUI coolTime;
    public Spawner spawner;

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
    public int baseEnemyNum;
    public int enemyMaxNum;
    public int enemyCurNum = 9999;
    public int level;
    public int maxLevel;
    public int enemySpawnNumIncrese;
    public bool isResurrection;
    public bool demeterOn;



    [Header("# UI")]
    public GameObject lobby;
    public GameObject lobbyAnim;
    public GameObject clearReward;
    public GameObject hud;
    public RedMoonEffect redMoon;
    public GameObject gameOverObject;
    public GameObject gameClearObject;
    public GameObject background;
    public FloatingJoystick joy;
    public LoadingImage loadingImage;
    public float lobbyDelayTime;

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
        gameStop = true;
        loadingImage.Loading(0, 1);
        AudioManager.instance.EndBgm();
        StartCoroutine(LoadingTime());
    }
    private IEnumerator LoadingTime()
    {
        yield return new WaitForSeconds(loadingImage.lerpTime);

        LobbyGo();
        yield return new WaitForSeconds(lobbyDelayTime);
        loadingImage.Loading(1, 0);

        yield return new WaitForSeconds(loadingImage.lerpTime);
    }
    public void LobbyGo()
    {
        lobby.SetActive(true);
        lobbyAnim.SetActive(true);
        player.transform.position = Vector3.zero;
        background.SetActive(true);
        hud.SetActive(false);
        // ������ ������ ���̽� �ʱ�ȭ
        ItemDatabase.instance.ItemReset();
        map.MapReset();
        PoolingReset();
        magicManager.MagicActiveCancel();
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

        if(level >= maxLevel) // ������ ��������
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
            enemyMaxNum += 50;
            EnemyManager.instance.EnemyLevelUp();
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
        hud.SetActive(true);
        isStage = true;
        player.gameObject.SetActive(true);
        gameStop = false;
        curGameTime = maxGameTime;
        enemyCurNum = 0;
        kill = 0;
        clearReward.SetActive(false);
        magicManager.StageStart();
        spawner.StageStart();
        AudioManager.instance.PlayBgm((int)Bgm.Stage);

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
        availablePoint = 0;
        isResurrection = false;
        level = 0;
        gold = 0;
        enemyMaxNum = baseEnemyNum;
        GameManager.instance.demeterOn = false;
        AudioManager.instance.SelectSfx();
        EnemyManager.instance.EnemyReset();
        NextStage();
    }
    public void GameQuit()
    {
        AudioManager.instance.SelectSfx();
        Application.Quit();
    }
}
