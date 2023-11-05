using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject lobbyAnim;
    public GameObject chractorSelect;

    private void OnEnable()
    {
    //    lobbyAnim.SetActive(true);
    }

    public void Click(int btnNum)
    {
        AudioManager.instance.SelectSfx();
        gameObject.SetActive(false);
        lobbyAnim.SetActive(false);
        switch (btnNum)
        {
            case 0:
                chractorSelect.SetActive(true);
                break;

            case 1:
                break;

            case 2:
                GameManager.instance.GameQuit();
                break;


        }
    }
}
