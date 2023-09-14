using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AditionalSelectBtn : MonoBehaviour
{
    public Sprite initSprite;
    public Sprite selectSprite;
    public Image[] AditionalSelectBtns;
    private VerticalLayoutGroup layoutGroup;
    private void Awake()
    {
        AditionalSelectBtns = GetComponentsInChildren<Image>();
        layoutGroup = GetComponent<VerticalLayoutGroup>();
    }

    public void ButtonOn(int num)
    {
        gameObject.SetActive(true);

        for (int i = AditionalSelectBtns.Length - 1; i >= 0; i--)
        {
            if(i <= num - 1)
            {
                AditionalSelectBtns[i].gameObject.SetActive(true);
            }
            else
            {
                AditionalSelectBtns[i].gameObject.SetActive(false);
            }
            AditionalSelectBtns[num].sprite = initSprite;
        }

        layoutGroup.spacing = num != 2 ? 0 : -30;
    }

    public void SelectButtonImage(int num)
    {
        AditionalSelectBtns[num].sprite = selectSprite;
    }
    public void InitButtonImage(int num)
    {
        AditionalSelectBtns[num].sprite = initSprite;
    }
}
