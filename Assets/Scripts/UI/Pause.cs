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
        // Canvas의 자식 오브젝트의 갯수를 구합니다.
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
        // Canvas의 자식 오브젝트의 갯수를 기반으로 배열을 초기화합니다.
        pausePrevObjects = new GameObject[canvasChildCoint];
        int parentCount = 0;

        // Canvas의 모든 자식 오브젝트를 순회합니다.
        foreach (Transform child in canvas)
        {
            // 만약 자식 오브젝트의 자식이 없다면 (부모 오브젝트라면)

            if (child.gameObject.activeSelf)
            {
                // 배열에 추가합니다.
                pausePrevObjects[parentCount] = child.gameObject;
                parentCount++;
            }
            
        }
    }
}
