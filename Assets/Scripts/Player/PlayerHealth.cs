using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image[] healthIcons;
    public Sprite fullIcon;
    public Sprite halfIcon;
    public Sprite emptyIcon;

    private void Update()
    {
        if (PlayerController.instance.health > PlayerController.instance.maxHealth)
        {
            PlayerController.instance.health = PlayerController.instance.maxHealth;
        }

        for (int currentHealthIcon = 0; currentHealthIcon < healthIcons.Length; currentHealthIcon++)
        {
            if (currentHealthIcon == Math.Floor(PlayerController.instance.health) && currentHealthIcon < Math.Ceiling(PlayerController.instance.health))
            {
                healthIcons[currentHealthIcon].sprite = halfIcon;
            } 
            else if (currentHealthIcon < Math.Floor(PlayerController.instance.health))
            {
                healthIcons[currentHealthIcon].sprite = fullIcon;
            }
            else
            {
                healthIcons[currentHealthIcon].sprite = emptyIcon;
            }

            if (currentHealthIcon < PlayerController.instance.maxHealth)
            {
                healthIcons[currentHealthIcon].enabled = true;
            } else
            {
                healthIcons[currentHealthIcon].enabled = false;
            }
        }
    }
}
