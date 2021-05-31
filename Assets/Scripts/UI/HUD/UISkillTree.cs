using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class UISkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;
    private List<SkillButtonController> skillButtonList;

    [SerializeField]
    private Material skillLockedMaterial;
    [SerializeField]
    private Material skillUnlockableMaterial;
    [SerializeField]
    public TMP_Text soulsLabel;

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

    private void Start()
    {
        soulsLabel.text = InventoryManager.instance.GetCurrentSouls().ToString();
        SetPlayerSkills(PlayerController.instance.GetPlayerSkills());
    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;

        skillButtonList = new List<SkillButtonController>();
        skillButtonList.Add(new SkillButtonController(transform.Find("SkillBtn - MaxHealth 1"), playerSkills, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButtonController(transform.Find("SkillBtn - MaxHealth 2"), playerSkills, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButtonController(transform.Find("SkillBtn - MaxHealth 3"), playerSkills, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButtonController(transform.Find("SkillBtn - Dash"), playerSkills, skillLockedMaterial, skillUnlockableMaterial));

        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        UpdateVisuals();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        soulsLabel.text = InventoryManager.instance.GetCurrentSouls().ToString();
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        foreach (SkillButtonController skillButton in skillButtonList)
        {
            skillButton.UpdateVisual();
        }
    }

    /*
     * Represents a single Skill Button
     */
    private class SkillButtonController
    {
        private Transform transform;
        private Image image;
        private Image backgroundImage;
        private PlayerSkills playerSkills;
        private Skill skill;
        private Material skillLockedMaterial;
        private Material skillUnlockableMaterial;

        public SkillButtonController(Transform transform, PlayerSkills playerSkills, Material skillLockedMaterial, Material skillUnlockableMaterial)
        {
            this.transform = transform;
            this.playerSkills = playerSkills;
            this.skill = transform.GetComponent<SkillButton>().skill;
            this.skillLockedMaterial = skillLockedMaterial;
            this.skillUnlockableMaterial = skillUnlockableMaterial;

            image = transform.Find("Image").GetComponent<Image>();
            image.sprite = skill.icon;
            backgroundImage = transform.Find("Background").GetComponent<Image>();

            transform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                if (!playerSkills.TryUnlockSkill(skill))
                {
                    Debug.LogWarning("requiremnts not met");
                }
            };
        }

        public void UpdateVisual()
        {
            if (playerSkills.IsSkillUnlocked(skill.skillType))
            {
                image.material = null;
                backgroundImage.color = new Color(1f, 1f, 1f);
                image.color = new Color(1f, 1f, 1f);
            }
            else
            {
                if (playerSkills.CanUnlock(skill))
                {
                   image.material = skillUnlockableMaterial;
                    transform.GetComponent<Button_UI>().enabled = true;
                    backgroundImage.color = new Color(.6f, .6f, .6f);
                    image.color = new Color(1f, 1f, 1f);
                }
                else
                {
                    image.material = skillLockedMaterial;
                    transform.GetComponent<Button_UI>().enabled = false;
                    backgroundImage.color = new Color(.2f, .2f, .2f);
                    image.color = new Color(.2f, .2f, .2f);
                }
            }
        }
    }
}
