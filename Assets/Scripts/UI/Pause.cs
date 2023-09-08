using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject[] pausePrevObjects;
    public Transform canvas;
    private int canvasChildCoint;

    public Image pauseBtnImage;
    public Sprite pauseBtnOnImage;
    public Sprite pauseBtnOffImage;
    private void Start()
    {
        // Canvas�� �ڽ� ������Ʈ�� ������ ���մϴ�.
       canvasChildCoint = canvas.childCount ;

    }
    public void PauseOn()
    {
        CollectActiveObjects();
        ActiveDeactivateObjects(false);
        pauseBtnImage.sprite = pauseBtnOffImage;
        Time.timeScale = 0;

    }
    public void PauseOff()
    {
        Time.timeScale = 1;
        ActiveDeactivateObjects(true);
        pauseBtnImage.sprite = pauseBtnOnImage;
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
