using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Skill/skill")]
public class Skill : ScriptableObject
{
    public PlayerSkills.SkillType skillType;
    public Sprite icon;
    public new string name;
    public string description;
    public Skill requirement;
    public int unlockValue;
}
