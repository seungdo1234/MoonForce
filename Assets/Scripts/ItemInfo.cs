using UnityEngine;

public enum ItemType { Default, Staff, Book }
public enum ItemRank { Default, Common, Rare, Epic, Legendary }

public class ItemInfo : MonoBehaviour
{


    [Header("# Item Infomation")]
    public ItemType type;
    public ItemRank rank;
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

    [Header("# Item Random Stat")]
    public float[] startAttack;
    public float[] endAttack;
    public float[] startRate;
    public float[] endRate;
    public float[] startMoveSpeed;
    public float[] endMoveSpeed;





}
