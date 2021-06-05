﻿using UnityEngine;
using TMPro;
using System;

public class InventoryManager : MonoBehaviour
{
    public int numberOfSoulsCollected = 0;
    public TMP_Text soulsLabel;
    public TMP_Text soulsAddedLabel;
    private float addedLabelDuration = 3f;
    private float addedTime;

    public event EventHandler<int> OnSoulsChanged;

    #region Singleton
    public static InventoryManager instance;

    private void Awake()
    {
        InventoryData data = SaveSystem.LoadInventory();
        numberOfSoulsCollected = data.collectedSouls;
        if (instance != null)
        {
            return;
        }

        instance = this;
        soulsLabel.text = numberOfSoulsCollected.ToString();
        soulsAddedLabel.enabled = false;
        addedTime = addedLabelDuration;
    }
    #endregion

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

    public void AddCollectedSouls(int numberOfSouls)
    {
        numberOfSoulsCollected += numberOfSouls;
        soulsLabel.text = numberOfSoulsCollected.ToString();
        soulsAddedLabel.text = "+" + numberOfSouls;
        soulsAddedLabel.enabled = true;
        addedTime = addedLabelDuration;
        OnSoulsChanged?.Invoke(this, numberOfSoulsCollected);

        SaveSystem.SaveInventory(this);
    }

    public int GetCurrentSouls()
    {
        return numberOfSoulsCollected;
    }

    public void SubtractCollectedSouls(int numberOfSouls)
    {
        numberOfSoulsCollected -= numberOfSouls;
        soulsLabel.text = numberOfSoulsCollected.ToString();
        OnSoulsChanged?.Invoke(this, numberOfSoulsCollected);

        SaveSystem.SaveInventory(this);
    }
}
