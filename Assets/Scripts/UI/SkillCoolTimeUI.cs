using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeUI : MonoBehaviour
{
    public Sprite[] skillSprites;
    public SkillCoolTimeSlot[] slots;


    private MagicManager magic;
    private void Awake()
    {
        magic =  GameManager.instance.magicManager;

    }

    // ��ų ��Ÿ�� ǥ�� Ȱ��ȭ
    public void StageStart()
    {
        int acitiveMagic = 0;

        for(int i = 0; i< magic.magicInfo.Length; i++)
        {
            if (magic.magicInfo[i].isMagicActive)
            {
                slots[acitiveMagic].gameObject.SetActive(true);
                slots[acitiveMagic].Init(skillSprites[i - 1], magic.magicInfo[i].magicCoolTime);
                acitiveMagic++;
            }

            if(acitiveMagic == slots.Length)
            {
                break;
            }
        }

        for(int i = acitiveMagic; i<slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false) ;
        }

    }

    // ��Ÿ�� ����
    public void CoolTimeStart(int slotNum)
    {
        slots[slotNum].CoolTime();
    }

}
