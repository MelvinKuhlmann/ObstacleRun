using UnityEngine;
using TMPro;
using System;

public class InventoryManager : MonoBehaviour, IDataPersister
{
    public DataSettings dataSettings;
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

    void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }

    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    public void AddCollectedSouls(int numberOfSouls)
    {
        numberOfSoulsCollected += numberOfSouls;
        soulsLabel.text = numberOfSoulsCollected.ToString();
        soulsAddedLabel.text = "+" + numberOfSouls;
        soulsAddedLabel.enabled = true;
        addedTime = addedLabelDuration;
        OnSoulsChanged?.Invoke(this, numberOfSoulsCollected);

      //  SaveSystem.SaveInventory(this);
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

      //  SaveSystem.SaveInventory(this);
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    public SData SaveData()
    {
        Debug.LogError("Save " + numberOfSoulsCollected);
        return new SData<int>(numberOfSoulsCollected);
    }

    public void LoadData(SData data)
    {
        SData<int> inventoryData = (SData<int>)data;

        Debug.LogError("load " + inventoryData.value);
        numberOfSoulsCollected = inventoryData.value;
        OnSoulsChanged?.Invoke(this, numberOfSoulsCollected);
    }
}
