using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public const int MAX_FRAGMENT_AMOUNT = 4;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    private List<Health> healthList;

    public HealthSystem(int healthAmount)
    {
        healthList = new List<Health>();
        for (int i = 0; i < healthAmount; i++)
        {
            Health health = new Health(4);
            healthList.Add(health);
        }
    }

    public List<Health> GetHealthList()
    {
        return healthList;
    }

    public void Damage(int damageAmount)
    {
        // Cycle through all the hearts starting from the end
        for (int i = healthList.Count  - 1; i >= 0; i--)
        {
            Health health = healthList[i];
            // Test if this health cannot absorb damageAmount
            if (damageAmount > health.GetFragmentAmount())
            {
                // Health cannot absorb full damageAmount, damage current health and keep going to the next
                damageAmount -= health.GetFragmentAmount();
                health.Damage(health.GetFragmentAmount());
            } else
            {
                // Health can absorb full damageAmount, absorb and break out of the cycle
                health.Damage(damageAmount);
                break;
            }
        }
        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);

        if(IsDead())
        {
            if (OnDead != null) OnDead(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        // Cycle through all the hearts starting from the beginning
        for (int i =0; i < healthList.Count; i++)
        {
            Health health = healthList[i];
            int missingFragments = MAX_FRAGMENT_AMOUNT - health.GetFragmentAmount();
            // Test if this health cannot absorb healAmount
            if (healAmount > missingFragments)
            {
                // Health cannot absorb full healAmount, heal current health and keep going to the next
                healAmount -= missingFragments;
                health.Heal(missingFragments);
            }
            else
            {
                // Health can absorb full healAmount, absorb and break out of the cycle
                health.Heal(healAmount);
                break;
            }
        }
        if (OnHealed != null) OnHealed(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return healthList[0].GetFragmentAmount() == 0;
    }

    // Represents a single heart
    public class Health
    {
        private int fragments;

        public Health(int fragments)
        {
            this.fragments = fragments;
        }

        public int GetFragmentAmount()
        {
            return fragments;
        }

        public void SetFragments(int fragments)
        {
            this.fragments = fragments;
        }

        public void Damage(int damageAmount)
        {
            if (damageAmount >= fragments)
            {
                fragments = 0;
            } else
            {
                fragments -= damageAmount;
            }
        }

        public void Heal(int healAmount)
        {
            if (fragments + healAmount > MAX_FRAGMENT_AMOUNT)
            {
                fragments = MAX_FRAGMENT_AMOUNT;
            } else
            {
                fragments += healAmount;
            }
        }
    }
}
