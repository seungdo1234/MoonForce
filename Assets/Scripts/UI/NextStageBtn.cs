using UnityEngine;
using UnityEngine.UI;

public class NextStageBtn : MonoBehaviour // �������������� ���� ��ư (���⸦ �������� �ʾҰų�, �κ��丮�� �� á�ٸ� Ȱ��ȭ X)
{
    public bool isActive;
    private Button btn;
    private Inventory inventory;
    public Text[] btnTexts;
    private void Start()
    {
        btn = GetComponent<Button>();
        inventory = GameManager.instance.inventory;
        btnTexts = GetComponentsInChildren<Text>(true);
    }

    private void Update()
    {
        if (!isActive && inventory.mainEqquipment.item.itemSprite != null)
        {
            int waitItemNum = 0;
            for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
            {
                if (!ItemDatabase.instance.Set(i).isEquip)
                {
                    waitItemNum++;
                }
            }
            if (waitItemNum < GameManager.instance.inventory.waitEqquipments.Length)
            {
                isActive = true;
                btn.interactable = true;
                btnTexts[0].gameObject.SetActive(true);
                btnTexts[1].gameObject.SetActive(false);
            }
        }
        else if (isActive)
        {
            int waitItemNum = 0;
            for (int i = 0; i < ItemDatabase.instance.itemCount(); i++)
            {
                if (!ItemDatabase.instance.Set(i).isEquip)
                {
                    waitItemNum++;
                }
            }
            if (waitItemNum >= GameManager.instance.inventory.waitEqquipments.Length)
            {
                isActive = false;
                btn.interactable = false;
                btnTexts[0].gameObject.SetActive(false);
                btnTexts[1].gameObject.SetActive(true);
            }

        }
    }
}
