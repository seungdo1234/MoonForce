using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �ʿ��� �� (������ ����Ʈ�� ������ ������ �����ؾ���.)
    // �����յ��� ������ ���� 
    public GameObject[] prefabs;

    // �Ӽ��� ���� ����ź�� ��������Ʈ
    public Sprite[] bulletAttributes;
    private ItemAttribute attribute;
    // Ǯ ����� �ϴ� ����Ʈ��
    public List<GameObject>[] pools;

    private void Awake()
    {
        // EnemyPrefabs�� ���� ��ŭ ����Ʈ ũ�� �ʱ�ȭ
        // Pool�� ��� �迭 �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];

        // �迭 �ȿ� ����ִ� ������ ����Ʈ�鵵 �ʱ�ȭ
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    private void Update()
    {

        // ���� ����ź�� ������ ���� ���� �Ӽ��� �ٸ� �� ��������Ʈ ���� 
        if (GameManager.instance.attribute != ItemAttribute.Default && attribute != GameManager.instance.attribute)
        {
            attribute = GameManager.instance.attribute;
            prefabs[1].gameObject.GetComponent<SpriteRenderer>().sprite = bulletAttributes[(int)attribute - 1];

        }
    }
    // ���� �Լ�
    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ���� -> �߰��ϸ� select ������ �Ҵ�
        // �̹� ������ Enemy�� �׾��� �� Destroy���� �ʰ� ��Ȱ��
        foreach (GameObject item in pools[index])
        {
            // ���빰 ������Ʈ�� ��Ȱ��ȭ(��� ����)���� Ȯ��
            if (!item.activeSelf)
            {
                // ��� �ִ� ���ӿ�����Ʈ select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                if (index == 1)
                {
                    SpriteRenderer sprite = select.GetComponent<SpriteRenderer>();
                    if (sprite.sprite != bulletAttributes[(int)attribute - 1])
                    {
                        sprite.sprite = bulletAttributes[(int)attribute - 1];
                    }
                }
                break;
            }
        }
        // ���� ��ã�Ҵٸ� -> ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    public void StageClear()
    {
        // ���������� Ŭ�����߱⶧���� Ȥ�ó� Ȱ��ȭ�� �������� ��Ȱ��ȭ ��Ŵ 

        foreach (GameObject item in pools[2])
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
            }
        }

    }


}
