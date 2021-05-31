using System;
using System.Collections;
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
        Skill skillRequirement = GetSkillRequirement(skill);
        if (skillRequirement != null)
        {
            if (IsSkillUnlocked(skillRequirement.skillType) && skill.unlockValue <= InventoryManager.instance.GetCurrentSouls())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return skill.unlockValue <= InventoryManager.instance.GetCurrentSouls();
        }
    }

    public Skill GetSkillRequirement(Skill skill)
    {
        if(skill.requirement != null )
        {
            return skill.requirement;
        }
        return null;
        /*switch (skillType)
        {
            case SkillType.HealthMax_3:     return SkillType.HealthMax_2;
            case SkillType.HealthMax_2:     return SkillType.HealthMax_1;
            case SkillType.MoveSpeed_2:     return SkillType.MoveSpeed_1;
            case SkillType.EarthShatter:    return SkillType.Dash;
        }
        return SkillType.None;*/
    }

    public bool TryUnlockSkill(Skill skill)
    {
        if (CanUnlock(skill))
        {
            //if (InventoryManager.instance.GetCurrentSouls() > 0)
           // {
                InventoryManager.instance.SubtractCollectedSouls(skill.unlockValue);
                UnlockSkill(skill);
                return true;
            //} else
            //{
            //    return false;
           // }
        }
        else
        {
            return false;
        }
    }
}
