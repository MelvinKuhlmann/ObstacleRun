using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;
    // https://www.youtube.com/watch?v=_OQTTKkwZQY 16:00 uses different sprites/material for player based on specific unlocked skills

    private void Start()
    {
        playerSkills = PlayerController.instance.GetPlayerSkills();
    }

    public void UnLockDash()
    {
        if (!playerSkills.TryUnlockSkill(PlayerSkills.SkillType.Dash))
        {
            Debug.LogWarning("Cannot buy skill yet, requirements not met");
        }
    }

    public void UnLockMaxHealth1()
    {
        if (!playerSkills.TryUnlockSkill(PlayerSkills.SkillType.HealthMax_1))
        {
            Debug.LogWarning("Cannot buy skill yet, requirements not met");
        }
    }

    public void UnLockMaxHealth2()
    {
        if (!playerSkills.TryUnlockSkill(PlayerSkills.SkillType.HealthMax_2))
        {
            Debug.LogWarning("Cannot buy skill yet, requirements not met");
        }
    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        UpdateVisuals();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.HealthMax_2))
        {
            transform.Find("SkillBtn - MaxHealth 2").GetComponent<Button>().interactable = false;
        }  else
        {
            if (playerSkills.CanUnlock(PlayerSkills.SkillType.HealthMax_2))
            {
                transform.Find("SkillBtn - MaxHealth 2").GetComponent<Button>().interactable = false;
            } else
            {
                transform.Find("SkillBtn - MaxHealth 2").GetComponent<Button>().interactable = true;
            }
        }
    }

    // https://www.youtube.com/watch?v=_OQTTKkwZQY 18:10 ended
}
