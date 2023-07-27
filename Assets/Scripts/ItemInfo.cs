using UnityEngine;

public enum ItemType { Default, Staff, Book }
public enum ItemRank { Default, Common, Rare, Epic, Legendary }
public enum ItemQuality { Default, Low, Normal, High }
public enum ItemAttribute {Default, Non, Fire, Water, Eeath , Grass , Dark , Holy }

public class ItemInfo : MonoBehaviour
{


    [Header("# Item Infomation")]
    public ItemType type;
    public ItemRank rank;
    public ItemQuality quality;
    [Header("# Common Item")]
    public Sprite[] commonImage;
    public string[] commonItemName;
    public string[] commonItemDesc;

    [Header("# Rare Item")]
    public Sprite[] rareImage;
    public string[] rareItemName;
    public string[] rareItemDesc;

    [Header("# Epic Item")]
    public Sprite[] epicImage;
    public string[] epicItemName;
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

    [Header("# Book Name")]
    public string[] bookNames;

    [Header("# Fire Book")]
    public Sprite[] fireMagicBookSprites;
    public string[] fireLowMagicDesc;
    public string[] fireNormalMagicDesc;
    public string[] fireHighMagicDesc;

    [Header("# Water Book")]
    public Sprite[] waterMagicBookSprites;
    public string[] waterLowMagicDesc;
    public string[] waterNormalMagicDesc;
    public string[] waterHighMagicDesc;

    [Header("# Non Book")]
    public Sprite[] nonMagicBookSprites;
    public string[] nonLowMagicDesc;
    public string[] nonNormalMagicDesc;
    public string[] nonHighMagicDesc;

    [Header("# Grass Book")]
    public Sprite[] grassMagicBookSprites;
    public string[] grassLowMagicDesc;
    public string[] grassNormalMagicDesc;
    public string[] grassHighMagicDesc;

    [Header("# Earth Book")]
    public Sprite[] earthMagicBookSprites;
    public string[] earthLowMagicDesc;
    public string[] earthNormalMagicDesc;
    public string[] earthHighMagicDesc;

    [Header("# Dark Book")]
    public Sprite[] darkMagicBookSprites;
    public string[] darkLowMagicDesc;
    public string[] darkNormalMagicDesc;
    public string[] darkHighMagicDesc;

    [Header("# Holy Book")]
    public Sprite[] holyMagicBookSprites;
    public string[] holyMagicDesc;


}
