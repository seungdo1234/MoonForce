using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    // enum : 열거형
    public enum InfoType { Goal, Level, Info, Time, Health }

    public InfoType type;

    [Header("# Heart Image ")]
    public Image[] heart;
    public Sprite[] heartSprites;

    private Text myText;
    private Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
        myText = GetComponent<Text>();

    }

    public void PlayerHit()
    {
        float heartCount = GameManager.instance.maxHealth / 2;
        float curHealth = GameManager.instance.curHealth;

        for (int i = 0; i < heartCount; i++)
        {
            if (curHealth > 1)
            {
                heart[i].sprite = heartSprites[0];
            }
            else if (curHealth == 1)
            {
                heart[i].sprite = heartSprites[1];
            }
            else
            {
                heart[i].sprite = heartSprites[2];
            }
            curHealth -= 2;
        }
    }

    private void Update()
    {
        if (GameManager.instance.gameStop)
        {
            return;
        }

        if (type == InfoType.Goal)
        {
            float curKill = GameManager.instance.kill;
            float maxKill = GameManager.instance.enemyMaxNum[GameManager.instance.level];
            mySlider.value = curKill / maxKill;
        }
    }
    private void LateUpdate()
    {
        if (GameManager.instance.gameStop)
        {
            return;
        }
        switch (type)
        {

            case InfoType.Level:
                // level은 int 형이므로 str으로 변환 -> string.Format을 활용하여 int형을 str 형으로 변환
                // Format ("문자열 + { 순번 : 나타내는 형태} ",형변환을 할 데이터)
                // F0, F1, F2... => 소수점 자릿수
                // D0, D1, D2... => 고정 자릿수
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level + 1);
                break;

            case InfoType.Info:
                break;

            case InfoType.Time:
                float remainTime = GameManager.instance.curGameTime;
                int min = Mathf.FloorToInt(remainTime / 60); // int형 소수점 버림
                int sec = Mathf.FloorToInt(remainTime % 60); // int형 소수점 버림
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case InfoType.Health:
 

                
                break;

        }
    }
}
