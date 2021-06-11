using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    //public float currentHealth;
  //  public int maxHealth;
    public float[] position;
   // public int collectedSouls;

    public PlayerData(PlayerController player)//, InventoryManager inventoryManager)
    {
      //  currentHealth = player.health;
      //  maxHealth = player.maxHealth;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        //collectedSouls = inventoryManager.GetCurrentSouls();
    }

}
