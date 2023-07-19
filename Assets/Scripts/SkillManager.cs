using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public SkillData[] skillData;
    public void StageClearRewardSelection(int skillNum)
    {
        skillData[skillNum].curLevel++;

    }
    private void Start()
    {
    }
}
