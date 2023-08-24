using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public Text[] statusLevels;
    public Text availablePointText;

    private Button[] button;

    private void OnEnable()
    {
        StatusWindowActive();
    }
    private void Awake()
    {
        button = GetComponentsInChildren<Button>();
    }
    public void StatUp(int statNumber)
    {

        GameManager.instance.statManager.statLevels[statNumber]++;

        GameManager.instance.statManager.StatValueUp(statNumber); 

        GameManager.instance.availablePoint--;

        StatusWindowActive();
    }

    private void StatusWindowActive()
    {


        availablePointText.text = string.Format("사용 가능 포인트 : {0}", GameManager.instance.availablePoint);

        for (int i = 0; i < button.Length - 1; i++)
        {
            statusLevels[i].text = string.Format("Lv.{0}", GameManager.instance.statManager.statLevels[i]);
            button[i].interactable = GameManager.instance.availablePoint != 0 && GameManager.instance.statManager.statMaxLevels[i]  > GameManager.instance.statManager.statLevels[i] ? true : false;
        }

    }
}
