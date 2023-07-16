using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")] // 에디터에서 애셋으로 만들 수 있게 하는 명령어
public class SkillData : ScriptableObject
{
    public enum SkillType { HandGun, Rifle, ShotGun, HealPack, Shoes, Glove, Bullet, Shovel, FireBall, Tornado, 
        Wind, Judgment, Explosion, Teleport, Lucky, Hoe, Fork }

    [Header("# Main Info")]
    public SkillType skillType;
    public string itemName; // 아이템 이름
    [TextArea] // 인스펙터에서 텍스트를 여러 줄 넣을 수 있게 TextArea 속성 부여
    public string itemDesc; // 아이템 설명

    [Header("# Level")]
    public int maxLevel;
    public int curLevel;

    [Header("# Level Data")]
    public float[] weaponNum; // 무기의 갯수 ex) 한번에 발사되는 총알 갯수
    public float[] damages; // 레벨당 플레이어의 공격력의 % 데미지 배열




}
