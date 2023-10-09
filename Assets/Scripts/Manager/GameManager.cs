using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 다른 스크립트에서도 쉽게 참조하기 위해 GameManager 인스턴스 화
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
    private Spawner spawner;

    [Header("# Player Data")]
    public int availablePoint; // 스테이지가 끝날때마다 스탯 레벨을 올릴 수 있는 포인트
    public ItemAttribute attribute; // Player의 무기 속성
    public int gold; // 골드

    [Header("# Stage Data")]
    public bool gameStop; // 게임이 멈췄을 때 true
    public bool isStage; // 스테이지를 진행 중 일때 True
    public bool isRedMoon; // 제한 시간안에 Enemy를 잡지 못했을 시 Enemy가 강해지는 붉은 달이 떠오름\
    public bool redMoonEffect; // 배경이 빨개질 때
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
        // 자기 자신으로 초기화
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
    public void GameLobby() // 로비로 갈 때 (죽거나, 설정 창에서 가거나)
    {
        background.SetActive(true);
        hud.SetActive(false);
        // 아이템 데이터 베이스 초기화
        ItemDatabase.instance.ItemReset();
        map.MapReset();
        PoolingReset();
        magicManager.MagicActiveCancel();
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(false);
        AudioManager.instance.PlayBgm((int)Bgm.Main);
    }
    public void PoolingReset()
    {
        magicManager.PoolingReset();
        pool.PoolingReset();
    }
    private IEnumerator StageClear() // 스테이지 클리어
    {
        background.SetActive(true);
        isStage = false;
        gameStop = true;
        if (isRedMoon) // 붉은 달이 떠올랐을 때
        {
            redMoon.RedMoonEnd();
            isRedMoon = false;
        }
        redMoonEffect = false;
        hud.SetActive(false);
        PoolingReset();  // 모든 폴링 오브젝트 비활성화
        pause.gameObject.SetActive(false); // 퍼즈 버튼 비활성화
        AudioManager.instance.EndBgm(); // Bgm 끄기
        GameManager.instance.demeterOn = false;
        yield return new WaitForSeconds(1f);

        if(level >= 49) // 마지막 스테이지
        {
            GameEnd(0);
        }
        else
        {
            
            AudioManager.instance.PlayBgm((int)Bgm.Victory - 1); // 승리 Bgm 재생
            yield return new WaitForSeconds(2f);
            availablePoint++;
            level++;
            nextStageBtn.LevelText(level); // 다음 스테이지 버튼 텍스트 변경
            enemyMaxNum += 10;
            if (level % 5 == 0) // 적 레벨 업
            {
                spawner.EnemyLevelUp();
            }
            clearReward.SetActive(true); // 클리어 보상 On
            player.gameObject.SetActive(false); // 플레이어 비활성화
            shop.ShopReset(); // 상점 초기화
            if (statManager.essenceOn) // 이번 스테이지에 정수가 활성화 됐다면
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
        
        if(!redMoonEffect && curGameTime <= redMoon.lerpTime) // 화면이 빨개지는 중
        {
            redMoon.RedMoonStart();
            redMoonEffect = true;
        }
        else if (curGameTime <= 0) // 붉은 달 On
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
        isResurrection = false;
        gold = 0;
        enemyMaxNum = 5;
        spawner.spawnPerLevelUp = 0;
        AudioManager.instance.SelectSfx();
        statManager.GameStart();
        AudioManager.instance.PlayBgm((int)Bgm.MaintenanceRoom);
        rewardManager.ItemCreate(-1 , 0);
        shop.ShopReset();
        GameManager.instance.demeterOn = false;
    }
}
