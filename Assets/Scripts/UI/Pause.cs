using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Header("Pause")]
    public GameObject[] pausePrevObjects;
    public Transform canvas;
    private int canvasChildCoint;

    public Image pauseBtnImage;
    public Sprite pauseBtnOnImage;
    public Sprite pauseBtnOffImage;
    [Header("MainMenu")]
    public Map map;


    private void Start()
    {
        // Canvas�� �ڽ� ������Ʈ�� ������ ���մϴ�.
       canvasChildCoint = canvas.childCount ;

    }
    public void PauseOn() // ���� ��
    {
        CollectActiveObjects(); // ������ �� Ȱ��ȭ �� �ִ� UI ������Ʈ���� �ϳ��� �迭�� ����
        ActiveDeactivateObjects(false); // UI ��Ȱ��ȭ
        pauseBtnImage.sprite = pauseBtnOffImage; // Pause ��ư ������ ���� 
        Time.timeScale = 0; // ���߱�

    }
    public void PauseOff()
    {
        Time.timeScale = 1; // ���
        ActiveDeactivateObjects(true); // Ȱ��ȭ
        pauseBtnImage.sprite = pauseBtnOnImage;
    }

  public void MainMenu() // ���� �޴��� ���ư� ��
    {
        Time.timeScale = 1; // ���

        GameManager.instance.gameStop = true; // ���� ����

        ActiveDeactivateObjects(false); // UI ��Ȱ��ȭ
        pauseBtnImage.sprite = pauseBtnOnImage;

        // Ȱ��ȭ�� Ǯ�� ������Ʈ�� ��Ȱ��ȭ
        GameManager.instance.PoolingReset();
        // �÷��̾�� �� �ʱ�ȭ
        GameManager.instance.player.transform.position = Vector3.zero;
        map.MapReset();

        GameManager.instance.player.gameObject.SetActive(false); // �÷��̾� ��Ȱ��ȭ

        // ������ ������ ���̽� �ʱ�ȭ
        ItemDatabase.instance.ItemReset();

        AudioManager.instance.PlayBgm((int)Bgm.Main);
        // 0���� 1���� ��Ȱ��ȭ �Ǹ� �ȵǱ� ������ �ٽ� Ȱ��ȭ
        pausePrevObjects[0].SetActive(true);
        pausePrevObjects[1].SetActive(true);

    }
    private void ActiveDeactivateObjects(bool isActive)
    {
        for(int i =0; i < canvasChildCoint; i++)
        {
            pausePrevObjects[i].SetActive(isActive);
            if(pausePrevObjects[i+1] == gameObject)
            {
                break;
            }
        }
    }
    private void CollectActiveObjects()
    {
        // Canvas�� �ڽ� ������Ʈ�� ������ ������� �迭�� �ʱ�ȭ�մϴ�.
        pausePrevObjects = new GameObject[canvasChildCoint];
        int parentCount = 0;

        // Canvas�� ��� �ڽ� ������Ʈ�� ��ȸ�մϴ�.
        foreach (Transform child in canvas)
        {
            // ���� �ڽ� ������Ʈ�� �ڽ��� ���ٸ� (�θ� ������Ʈ���)

            if (child.gameObject.activeSelf)
            {
                // �迭�� �߰��մϴ�.
                pausePrevObjects[parentCount] = child.gameObject;
                parentCount++;
            }
            
        }
    }
}
