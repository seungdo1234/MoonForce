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

   
    private void Awake()
    {
        // �ڱ� �ڽ����� �ʱ�ȭ
        instance = this;
    }

  
}
