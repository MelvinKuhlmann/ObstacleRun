using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;

    private void Start()
    {
        playerSkills = PlayerController.instance.GetPlayerSkills();
    }

    public void UnLockEarthShatter()
    {
        playerSkills.UnlockSkill(PlayerSkills.SkillType.EarthShatter);
    }

    public void UnLockMaxHealth1()
    {
        playerSkills.UnlockSkill(PlayerSkills.SkillType.HealthMax_1);
    }

    public void UnLockMaxHealth2()
    {
        playerSkills.UnlockSkill(PlayerSkills.SkillType.HealthMax_2);
    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
    }
}
