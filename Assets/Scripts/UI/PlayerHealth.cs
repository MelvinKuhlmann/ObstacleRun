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

        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i == Math.Floor(health) && i < Math.Ceiling(health))
            {
                healthIcons[i].sprite = halfIcon;
            } 
            else if (i < Math.Floor(health))
            {
                healthIcons[i].sprite = fullIcon;
            }
            else
            {
                healthIcons[i].sprite = emptyIcon;
            }

            if (i < maxHealth)
            {
                healthIcons[i].enabled = true;
            } else
            {
                healthIcons[i].enabled = false;
            }
        }
    }

}
