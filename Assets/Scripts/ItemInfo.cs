using UnityEngine;

public enum ItemType { Default, Staff, Book }
public enum ItemRank { Default, Common, Rare, Epic, Legendary }
public enum ItemQuality { Default, Low, Normal, High }

public class ItemInfo : MonoBehaviour
{


    [Header("# Item Infomation")]
    public ItemType type;
    public ItemRank rank;
    public ItemQuality quality;
    [Header("# Common Item")]
    public Sprite[] commonStaffImage;
    public string[] commonItemName;
    public string[] commonItemDesc;

    [Header("# Rare Item")]
    public Sprite[] rareStaffImage;
    public string[] rareitemName;
    public string[] rareItemDesc;

    [Header("# Epic Item")]
    public Sprite[] epicStaffImage;
    public string[] epicitemName;
    public string[] epicItemDesc;
    public string[] additionalOption;

    [Header("# Item Base Stat")]
    public int[] baseAttack;
    public float[] baseRate;
    public float[] MoveSpeed;


    [Header("# Item Increase Stat")]
    public int[] increaseAttack;
    public float[] increaseRate;
    public float[] increaseMoveSpeed;

}
