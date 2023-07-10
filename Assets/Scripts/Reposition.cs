using UnityEngine;

public class Reposition : MonoBehaviour
{

    // ��� �ݶ��̴��� �ƿ츣�� Ŭ����
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        // �Ÿ� Ȯ�� (X������ ������� Y������ �������)
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":

                float differX = playerPos.x - myPos.x;
                float differY = playerPos.y - myPos.y;

                float dirX = differX < 0 ? -1 : 1;
                float dirY = differY < 0 ? -1 : 1;

                differX = Mathf.Abs(differX);
                differY = Mathf.Abs(differY);

                if (differX > differY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (differX < differY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                else
                {
                    transform.Translate(Vector3.right * dirX * 40);
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                if (col.enabled) // ���� �ݶ��̴��� ����ִٸ�
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 rand = new Vector3(Random.Range(-3, 4), Random.Range(-3, 4), 0); // ����

                    transform.Translate(rand + dist * 2);
                }
                break;
        }

    }
}
