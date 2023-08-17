using UnityEngine;


[System.Serializable]
public class Magic
{
    public bool isMagicActive; // Ȱ��ȭ ����

    [Header("# BaseInfo")]
    public int magicNumber; // ���� ��ȣ
    public string magicName; // ���� �̸�
    public GameObject magicEffect; // ���� ����Ʈ 

    [Header("# Details")]
    public bool isFlying; // ���ư����� ex)  ȭ����
    public float damagePer; // ������ ex) 1.5��� ���� ���ݷ��� 1.5���� �������� ��
    public int magicCount; // �ѹ��� �߻� �Ǵ� ������ ����
    public float magicCoolTime; // ������ ��Ÿ��
    public int penetration; // �����

}
