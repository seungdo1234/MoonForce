using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")] // �����Ϳ��� �ּ����� ���� �� �ְ� �ϴ� ��ɾ�
public class SkillData : ScriptableObject
{
    public enum SkillType { HandGun, Rifle, ShotGun, HealPack, Shoes, Glove, Bullet, Shovel, FireBall, Tornado, 
        Wind, Judgment, Explosion, Teleport, Lucky, Hoe, Fork }

    [Header("# Main Info")]
    public SkillType skillType;
    public string itemName; // ������ �̸�
    [TextArea] // �ν����Ϳ��� �ؽ�Ʈ�� ���� �� ���� �� �ְ� TextArea �Ӽ� �ο�
    public string itemDesc; // ������ ����

    [Header("# Level")]
    public int maxLevel;
    public int curLevel;

    [Header("# Level Data")]
    public float[] weaponNum; // ������ ���� ex) �ѹ��� �߻�Ǵ� �Ѿ� ����
    public float[] damages; // ������ �÷��̾��� ���ݷ��� % ������ �迭




}
