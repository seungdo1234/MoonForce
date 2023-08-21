using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("# Player Data")]
    public ItemAttribute attribute;
    public float maxHealth;
    public float curHealth;
    public int attack;
    public int baseAttack;
    public float rate;
    public float baseRate;
    public float moveSpeed;
    public float baseMoveSpeed;
    public int penetration; // 관통력
    public int weaponNum; // 총 : 한번에 발사 되는 총탄, 근접 : 삽의 갯수
    public int instantKillPer; // 즉사 확률
    public float knockBackValue; // Enemy가 마력탄을 맞았을 때 넉백 수치



    [Header("# Stage Data")]
    public bool gameStop; // 게임이 멈췄을 때 true
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
        // 자기 자신으로 초기화
        instance = this;
        curHealth = maxHealth;
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
