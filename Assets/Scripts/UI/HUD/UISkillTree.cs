using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class UISkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;
    private List<SkillButton> skillButtonList;

    [SerializeField]
    private Material skillLockedMaterial;
    [SerializeField]
    private Material skillUnlockableMaterial;

    #region singleton
    private static UISkillTree instance;
    public static UISkillTree Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;

        skillButtonList = new List<SkillButton>();
        skillButtonList.Add(new SkillButton(transform.Find("SkillBtn - MaxHealth 1"), playerSkills, PlayerSkills.SkillType.HealthMax_1, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("SkillBtn - MaxHealth 2"), playerSkills, PlayerSkills.SkillType.HealthMax_2, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("SkillBtn - Dash"), playerSkills, PlayerSkills.SkillType.Dash, skillLockedMaterial, skillUnlockableMaterial));

        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        UpdateVisuals();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        foreach (SkillButton skillButton in skillButtonList)
        {
            skillButton.UpdateVisual();
        }
    }

    /*
     * Represents a single Skill Button
     */
    private class SkillButton
    {
        private Transform transform;
        private Image image;
        private Image backgroundImage;
        private PlayerSkills playerSkills;
        private PlayerSkills.SkillType skillType;
        private Material skillLockedMaterial;
        private Material skillUnlockableMaterial;

        public SkillButton(Transform transform, PlayerSkills playerSkills, PlayerSkills.SkillType skillType, Material skillLockedMaterial, Material skillUnlockableMaterial)
        {
            this.transform = transform;
            this.playerSkills = playerSkills;
            this.skillType = skillType;
            this.skillLockedMaterial = skillLockedMaterial;
            this.skillUnlockableMaterial = skillUnlockableMaterial;

            image = transform.Find("Image").GetComponent<Image>();
            backgroundImage = transform.Find("Background").GetComponent<Image>();

            transform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                if (!playerSkills.TryUnlockSkill(skillType))
                {
                    Debug.LogWarning("requiremnts not met");
                }
            };
        }

        public void UpdateVisual()
        {
            if (playerSkills.IsSkillUnlocked(skillType))
            {
                image.material = null;
                backgroundImage.material = null;
            }
            else
            {
                if (playerSkills.CanUnlock(skillType))
                {
                   image.material = skillUnlockableMaterial;
                   backgroundImage.material = skillUnlockableMaterial;
                }
                else
                {
                    image.material = skillLockedMaterial;
                    backgroundImage.material = skillLockedMaterial;
                }
            }
        }
    }
}
