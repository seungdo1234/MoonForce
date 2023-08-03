using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextPool : MonoBehaviour
{
    // �ʿ��� �� (������ ����Ʈ�� ������ ������ �����ؾ���.)
    // �����յ��� ������ ���� 
    public GameObject prefabs;

    // Ǯ ����� �ϴ� ����Ʈ��
    private List<GameObject> pools;

    private void Awake()
    {
        // EnemyPrefabs�� ���� ��ŭ ����Ʈ ũ�� �ʱ�ȭ
        // Pool�� ��� �迭 �ʱ�ȭ
        pools = new List<GameObject>();

    }


    // ���� �Լ�
    public void Get(float damage, Vector3 target)
    {
        GameObject select = null;

        // ������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ���� -> �߰��ϸ� select ������ �Ҵ�
        // �̹� ������ Enemy�� �׾��� �� Destroy���� �ʰ� ��Ȱ��
        foreach (GameObject item in pools)
        {
            // ���빰 ������Ʈ�� ��Ȱ��ȭ(��� ����)���� Ȯ��
            if (!item.activeSelf)
            {
                // ��� �ִ� ���ӿ�����Ʈ select ������ �Ҵ�
                select = item;
                break;
            }
        }
        // ���� ��ã�Ҵٸ� -> ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(prefabs, transform);
            pools.Add(select);
        }

        select.GetComponent<DamageText>().Init(damage, target);

    }

    private void Update()
    {
        //Get(55, transform);
    }
}
