﻿using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using CodeMonkey.Utils;

// https://www.youtube.com/watch?v=xWCJfE_sAXE : 23:00 
public class HealthVisual : MonoBehaviour
{
    [SerializeField]
    private Sprite health0Sprite;
    [SerializeField]
    private Sprite health1Sprite;
    [SerializeField]
    private Sprite health2Sprite;
    [SerializeField]
    private Sprite health3Sprite;
    [SerializeField]
    private Sprite health4Sprite;
    [SerializeField]
    private AnimationClip healthFullAnimationClip;

    private List<HealthImage> healthImageList;
    public HealthSystem healthSystem;
    private bool isHealing;

    public static HealthVisual instance;

    private void Awake()
    {
        healthImageList = new List<HealthImage>();
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    private void Start()
    {
        FunctionPeriodic.Create(HealingAnimatedPeriodic, .05f);
        HealthSystem healthSystem = new HealthSystem(4); // max health
        SetHealthSystem(healthSystem);
    }

    public void SetHealthSystem(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;

        List<HealthSystem.Health> healthList = healthSystem.GetHealthList();
        Vector2 healthAnchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < healthList.Count; i++)
        {
            HealthSystem.Health health = healthList[i];
            CreateHealthImage(healthAnchoredPosition).SetHealthFragments(health.GetFragmentAmount());
            healthAnchoredPosition += new Vector2(40, 0);
        }

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        // Health system was damaged
        RefreshAllHealth();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        // Health system was healed
        isHealing = true;
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        Debug.LogWarning("DEAD!!");
    }

    private void RefreshAllHealth()
    {
        List<HealthSystem.Health> healtList = healthSystem.GetHealthList();
        for (int i = 0; i < healthImageList.Count; i++)
        {
            HealthImage healthImage = healthImageList[i];
            HealthSystem.Health health = healtList[i];
            healthImage.SetHealthFragments(health.GetFragmentAmount());
        }
    }

    private void HealingAnimatedPeriodic()
    {
        if (isHealing)
        {
            bool fullyHealed = true;
            List<HealthSystem.Health> healtList = healthSystem.GetHealthList();
            for (int i = 0; i < healtList.Count; i++)
            {
                HealthImage healthImage = healthImageList[i];
                HealthSystem.Health health = healtList[i];
                if (healthImage.GetFragmentAmount() != health.GetFragmentAmount())
                {
                    // Visual is different from logic
                    healthImage.AddHealthVisualFragment();
                    if (healthImage.GetFragmentAmount() == HealthSystem.MAX_FRAGMENT_AMOUNT)
                    {
                        // This health was full healed
                        healthImage.PlayHealthFullAnimation();
                    }
                    fullyHealed = false;
                    break;
                }
            }
            if (fullyHealed)
            {
                isHealing = false;
            }
        }
    }

    private HealthImage CreateHealthImage(Vector2 anchoredPosition)
    {
        GameObject healthGameObject = new GameObject("Health", typeof(Image), typeof(Animation));
        // Set as child of this transform
        healthGameObject.transform.SetParent(transform);
        healthGameObject.transform.localPosition = Vector3.zero;

        // Locate and Size of object
        healthGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        healthGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);

        healthGameObject.GetComponent<Animation>().AddClip(healthFullAnimationClip, "health_full");

        // Set heart sprite
        Image heartImageUI = healthGameObject.GetComponent<Image>();
        heartImageUI.sprite = health0Sprite;

        HealthImage heartImage = new HealthImage(this, heartImageUI, healthGameObject.GetComponent<Animation>());
        healthImageList.Add(heartImage);
        return heartImage;
    }

    // Represents a single heart
    public class HealthImage
    {
        private int fragments;
        private HealthVisual healthVisual;
        private Image healthImage;
        private Animation animation;

        public HealthImage(HealthVisual healthVisual, Image healthImage, Animation animation)
        {
            this.healthVisual = healthVisual;
            this.healthImage = healthImage;
            this.animation = animation; 
        }

        public void SetHealthFragments(int fragments)
        {
            this.fragments = fragments;
            switch (fragments)
            {
                case 0:
                    healthImage.sprite = healthVisual.health0Sprite;
                    break;
                case 1:
                    healthImage.sprite = healthVisual.health1Sprite;
                    break;
                case 2:
                    healthImage.sprite = healthVisual.health2Sprite;
                    break;
                case 3:
                    healthImage.sprite = healthVisual.health3Sprite;
                    break;
                case 4:
                    healthImage.sprite = healthVisual.health4Sprite;
                    break;
            }
        }

        public int GetFragmentAmount()
        {
            return fragments;
        }

        public void AddHealthVisualFragment()
        {
            SetHealthFragments(fragments + 1);
        }

        public void PlayHealthFullAnimation()
        {
            animation.Play("health_full", PlayMode.StopAll);
        }
    }
}
