using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager instance;

    public Percent[] chestPer;
    public Percent[] qualityPer;

    public int[] bronzeChest;
    public int[] silverChest;
    public int[] goldChest;
    public int[] specialChest;

    private void Awake()
    {
        instance = this;
    }
}

[System.Serializable]
public class Percent
{
    public int[] percent;
}
