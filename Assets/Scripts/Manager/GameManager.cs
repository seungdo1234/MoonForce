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
        // 아이템 데이터 베이스 초기화
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
    private IEnumerator StageClear() // 스테이지 클리어
    {
        isStage = false;
        gameStop = true;
        if (isRedMoon) // 붉은 달이 떠올랐을 때
        {
            redMoon.RedMoonEnd();
            isRedMoon = false;
        }
        redMoonEffect = false;
        hud.SetActive(false);
        PoolingReset();
        AudioManager.instance.EndBgm(); // Bgm 끄기
        yield return new WaitForSeconds(1f);
        pause.gameObject.SetActive(false); // 퍼즈 버튼 비활성화
        AudioManager.instance.PlayBgm((int)Bgm.Victory); // 승리 Bgm 재생
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
