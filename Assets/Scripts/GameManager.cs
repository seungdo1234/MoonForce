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

    [Header("# Player Data")]
    public float maxHealth;
    public float curHealth;
    public int attack;
    public int baseAttack;
    public float rate;
    public float baseRate;
    public float moveSpeed;
    public float baseMoveSpeed;
    public int penetration;
    public int weaponNum; // 총 : 한번에 발사 되는 총탄, 근접 : 삽의 갯수

    [Header("# Stage Data")]
    public bool gameStop;
    public float maxGameTime;
    public float curGameTime;
    public int kill;
    public int[] enemyMaxNum;
    public int enemyCurNum = 9999;
    public int level;

    [Header("# UI")]
    public GameObject clearReward;
    public GameObject startBtn;

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
        yield return new WaitForSeconds(3f);
        level++;
        clearReward.SetActive(true);
        Time.timeScale = 0f;
    }
    public void NextStage()
    {
        Time.timeScale = 1f;
        gameStop = false;
        curGameTime = maxGameTime;
        enemyCurNum = 0;
        kill = 0;
        clearReward.SetActive(false);
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
