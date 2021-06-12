using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SoulUI : MonoBehaviour
{
    public int numberOfSoulsCollected = 0;
    public TMP_Text soulsLabel;
    public TMP_Text soulsAddedLabel;
    private float addedLabelDuration = 3f;
    private float addedTime;


    private void Start()
    {
        soulsLabel.text = numberOfSoulsCollected.ToString();
    }

    /* #region Singleton
     public static SoulUI instance;

     private void Awake()
     {
        // InventoryData data = SaveSystem.LoadInventory();
        // numberOfSoulsCollected = data.collectedSouls;
         if (instance != null)
         {
             return;
         }

         instance = this;
         soulsLabel.text = numberOfSoulsCollected.ToString();
         soulsAddedLabel.enabled = false;
         addedTime = addedLabelDuration;
     }
     #endregion*/

    private void Update()
    {
        if (addedTime <= 0)
        {
            soulsAddedLabel.enabled = false;
        }
        else
        {
            addedTime -= Time.deltaTime;
        }
    }

    public void AddItem(string key, int amount)
    {
        if (key.Equals("Soul"))
        {
            numberOfSoulsCollected += amount;
            soulsLabel.text = numberOfSoulsCollected.ToString();
            soulsAddedLabel.text = "+" + amount;
            soulsAddedLabel.enabled = true;
            addedTime = addedLabelDuration;
        }
    }

    public void Initialize(Dictionary<string, int> items)
    {
        if (items.TryGetValue("Soul", out int val))
        {
            numberOfSoulsCollected = val;
            soulsLabel.text = numberOfSoulsCollected.ToString();
        }
    }

    public void SubtractCollectedSouls(int numberOfSouls)
    {
        numberOfSoulsCollected -= numberOfSouls;
        soulsLabel.text = numberOfSoulsCollected.ToString();
      //  OnSoulsChanged?.Invoke(this, numberOfSoulsCollected);

      //  SaveSystem.SaveInventory(this);
    }
}
