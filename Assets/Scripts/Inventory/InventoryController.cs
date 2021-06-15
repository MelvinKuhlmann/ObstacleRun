using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour, IDataPersister
{
    [Serializable]
    public class InventoryEvent
    {
        public string key;
        public UnityEvent<int> OnRemove;
        public UnityEvent<int> OnAdd;
        public UnityEvent<int> OnSet;
    }

    [System.Serializable]
    public class InventoryChecker
    {
        public Dictionary<string, int> inventoryItems;
        public UnityEvent OnHasItem, OnDoesNotHaveItem;

        public bool CheckInventory(InventoryController inventory)
        {
            if (inventory != null)
            {
                foreach (KeyValuePair<string, int> kvp in inventoryItems)
                {
                    if (!inventory.HasItem(kvp.Key))
                    {
                        OnDoesNotHaveItem.Invoke();
                        return false;
                    }
                }
                OnHasItem.Invoke();
                return true;
            }
            return false;
        }
    }

    public InventoryEvent[] inventoryEvents;
    public event Action OnInventoryLoaded;

    public DataSettings dataSettings;

    Dictionary<string, int> m_InventoryItems = new Dictionary<string, int>();

    //Debug function useful in editor during play mode to print in console all objects in that InventoyController
    [ContextMenu("Dump")]
    void Dump()
    {
        foreach (KeyValuePair<string, int> kvp in m_InventoryItems)
        {
            Debug.Log(kvp.Key + " : " + kvp.Value);
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
    public void AddItem(string key, int amount)
    {
        if (!m_InventoryItems.ContainsKey(key))
        {
            m_InventoryItems.Add(key, amount);
        } else
        {
            m_InventoryItems[key] += amount;
        }
        var ev = GetInventoryEvent(key);
        if (ev != null) ev.OnAdd.Invoke(amount);
    }

    public void SubtractItem(string key, int amount)
    {
        if (m_InventoryItems.TryGetValue(key, out int val))
        {
            if (val < amount)
            {
                return;
            }
            m_InventoryItems[key] -= amount;
            var ev = GetInventoryEvent(key);
            if (ev != null) ev.OnRemove.Invoke(amount);
        }
    }
    public bool HasItem(string key)
    {
        return m_InventoryItems.ContainsKey(key);
    }

    public int GetAmount(string key)
    {
        if (m_InventoryItems.TryGetValue(key, out int val))
        {
            return val;
        }
        return 0;
    }

    public void Clear()
    {
        m_InventoryItems.Clear();
    }

    InventoryEvent GetInventoryEvent(string key)
    {
        foreach (var iv in inventoryEvents)
        {
            if (iv.key == key) return iv;
        }
        return null;
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
        return new SData<Dictionary<string, int>>(m_InventoryItems);
    }

    public void LoadData(SData data)
    {
        SData<Dictionary<string, int>> inventoryData = (SData<Dictionary<string, int>>)data;
        foreach (KeyValuePair<string, int> kvp in inventoryData.value)
        {
            AddItem(kvp.Key, kvp.Value);
            var ev = GetInventoryEvent(kvp.Key);
            if (ev != null) ev.OnSet.Invoke(kvp.Value);
        }
        if (OnInventoryLoaded != null) OnInventoryLoaded();
    }
}