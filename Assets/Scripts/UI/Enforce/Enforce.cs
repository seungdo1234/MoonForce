using UnityEngine;
using UnityEngine.UI;

public class Enforce : MonoBehaviour
{
    public EnforceInfo[] enforceInfo;


    [Header("# EnforceInfoWindow")]
    public Text enforceNameText;
    public Text enforceDescText;
    public Text enforcePriceText;
    public Button buyBtn;

    private void OnEnable()
    {
        enforceNameText.text = null;
        enforceDescText.text = null;
        enforcePriceText.text = null;
        buyBtn.interactable = false;
    }

    public void EnforceSelect(int num)
    {
        AudioManager.instance.SelectSfx();

        int price = enforceInfo[num].initPrice + (enforceInfo[num].initPrice * enforceInfo[num].curLevel);

        buyBtn.interactable = GameManager.instance.gemStone < price ? false : true;

        enforceNameText.text = enforceInfo[num].name;
        if(enforceInfo[num].curLevel < enforceInfo[num].maxLevel)
        {
            enforceNameText.text += string.Format(" Lv.{0}", enforceInfo[num].curLevel + 1);
            enforcePriceText.text = string.Format("{0}", price);
        }
        else
        {
            buyBtn.interactable = false;
            enforceNameText.text += " Lv.Max";
            enforcePriceText.text = "00";
        }
        enforceDescText.text = enforceInfo[num].enforceDesc;


    }

}

[System.Serializable]
public class EnforceInfo
{
    public string name;
    public string enforceDesc;
    public int maxLevel;
    public int curLevel;
    public int initPrice;
    public int priceIncrease;
    public float statIncrease;


}
