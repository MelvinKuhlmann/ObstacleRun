using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public Skill skill;
    }

    public enum SkillType {
        None,
        Dash,
        EarthShatter,      
        MoveSpeed_1,
        MoveSpeed_2,
        HealthMax_1,
        HealthMax_2,
        HealthMax_3
    }

    private List<SkillType> unlockedSkillTypeList;

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }

    private void UnlockSkill(Skill skill)
    {
        if (!IsSkillUnlocked(skill.skillType))
        {
            unlockedSkillTypeList.Add(skill.skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skill = skill });
        }
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public bool CanUnlock(Skill skill)
    {
        if (IsSkillUnlocked(skill.skillType))
        {
            Debug.LogWarning("Skill is already unlocked");
            return false;
        }
        Skill skillRequirement = GetSkillRequirement(skill);
        if (skillRequirement != null)
        {
            return IsSkillUnlocked(skillRequirement.skillType) && skill.unlockValue <= PlayerController.instance.GetComponent<InventoryController>().GetAmount("Soul");
        }
        else
        {
            return skill.unlockValue <= PlayerController.instance.GetComponent<InventoryController>().GetAmount("Soul");
        }
    }

    public Skill GetSkillRequirement(Skill skill)
    {
        return skill.requirement != null ? skill.requirement : null;
    }

    public bool TryUnlockSkill(Skill skill)
    {
        if (CanUnlock(skill))
        {
            PlayerController.instance.GetComponent<InventoryController>().SubtractItem("Soul", skill.unlockValue);
          //  InventoryManager.instance.SubtractCollectedSouls(skill.unlockValue);
            UnlockSkill(skill);
            return true;
        }
        else
        {
            return false;
        }
    }
}
