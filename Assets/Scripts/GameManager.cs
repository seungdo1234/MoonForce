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

   
    private void Awake()
    {
        // 자기 자신으로 초기화
        instance = this;
    }

  
}
