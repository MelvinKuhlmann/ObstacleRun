using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSkills : MonoBehaviour, IDataPersister
{
     public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlockedUI;
     public class OnSkillUnlockedEventArgs : EventArgs
     {
         public Skill skill;
     }

    private List<SkillType> unlockedSkillTypeList = new List<SkillType>();

    [Serializable]
    public class SKillUnlockedEvent : UnityEvent<Skill>
    { }

    public DataSettings dataSettings;
    public SKillUnlockedEvent OnSkillUnlocked;
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

    void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }

    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    private void UnlockSkill(Skill skill)
    {
        if (!IsSkillUnlocked(skill.skillType))
        {
            unlockedSkillTypeList.Add(skill.skillType);
            OnSkillUnlocked.Invoke(skill);
            OnSkillUnlockedUI?.Invoke(this, new OnSkillUnlockedEventArgs { skill = skill });
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
            UnlockSkill(skill);
            return true;
        }
        else
        {
            return false;
        }
    }

    //Debug function useful in editor during play mode to print in console all objects in that PlayerSkills
    [ContextMenu("Dump")]
    void Dump()
    {
        foreach (SkillType skill in unlockedSkillTypeList)
        {
            Debug.Log(skill);
        }
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    public SData SaveData()
    {
        return new SData<List<SkillType>>(unlockedSkillTypeList);
    }

    public void LoadData(SData data)
    {
        SData<List<SkillType>> unlockedSkills = (SData<List<SkillType>>)data;
        unlockedSkillTypeList = unlockedSkills.value;
    }
}
