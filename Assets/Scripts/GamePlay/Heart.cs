using System.Collections;
using UnityEngine;

public class Heart : MonoBehaviour
{

    public Sprite[] heartSprite;
    public float[] healthUpValue;
    public float lerpTime;
    public float scanRange;
    public Vector3 textPos;
    private int healthType;
    private SpriteRenderer sprite;
    private Player player;
    private Animator anim;
    public void Init(int type)
    {
        healthType = type;

        if (sprite == null)
        {
            anim = GetComponent<Animator>();
            player = GameManager.instance.player;
            sprite = GetComponent<SpriteRenderer>();
        }

        anim.speed = 1f;
        sprite.sprite = heartSprite[healthType];

        StartCoroutine(PlayerScan());
    }

    // �÷��̾� ��ĵ
    private IEnumerator PlayerScan()
    {

        while (!GameManager.instance.gameStop)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if(distance <= scanRange)
            {
                ScanOn();
                break;
            }

            yield return null;
        }
    }

    // ��ĵ O
    public void ScanOn()
    {
        anim.speed = 0f;
        StartCoroutine(HeartMove());
    }

    // �÷��̾�� �̵�
    private IEnumerator HeartMove()
    {
        float timer = 0;

        while (timer < lerpTime)
        {
            timer += Time.deltaTime;

            float posX = Mathf.Lerp(transform.position.x, player.transform.position.x, timer / lerpTime);
            float posy = Mathf.Lerp(transform.position.y, player.transform.position.y, timer / lerpTime);

            transform.position = new Vector3(posX, posy, 0);

            yield return null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // ��
    {
        if ((!collision.CompareTag("Player") && !collision.CompareTag("PlayerDamaged")) || GameManager.instance.gameStop)
        {
            return;
        }

        GameManager.instance.statManager.curHealth = Mathf.Min(GameManager.instance.statManager.curHealth + healthUpValue[healthType], GameManager.instance.statManager.maxHealth);

        Vector3 pos = player.transform.position + textPos;
        GameManager.instance.damageTextPool.Get((int)TextType.Heal, (int)healthUpValue[healthType], pos);
        AudioManager.instance.PlayerSfx(Sfx.Heal);

        gameObject.SetActive(false);
    }
}
