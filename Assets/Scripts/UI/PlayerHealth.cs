using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public int maxHealth;

    public Image[] healthIcons;
    public Sprite fullIcon;
    public Sprite halfIcon;
    public Sprite emptyIcon;

    private void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        for (int currentHealthIcon = 0; currentHealthIcon < healthIcons.Length; currentHealthIcon++)
        {
            if (currentHealthIcon == Math.Floor(health) && currentHealthIcon < Math.Ceiling(health))
            {
                healthIcons[currentHealthIcon].sprite = halfIcon;
            } 
            else if (currentHealthIcon < Math.Floor(health))
            {
                healthIcons[currentHealthIcon].sprite = fullIcon;
            }
            else
            {
                healthIcons[currentHealthIcon].sprite = emptyIcon;
            }

            if (currentHealthIcon < maxHealth)
            {
                healthIcons[currentHealthIcon].enabled = true;
            } else
            {
                healthIcons[currentHealthIcon].enabled = false;
            }
        }
    }

    public void GetDamage(float damage)
    {
        health -= damage;
    }

    public void ReceiveHealth(float healthReceived)
    {
        health += healthReceived;
    }
}
