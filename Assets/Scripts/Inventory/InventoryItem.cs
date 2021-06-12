using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class InventoryItem : MonoBehaviour, IDataPersister
{
    public string inventoryKey = "";
    public LayerMask layers;
    public bool disableOnEnter = false;
    public int amount = 1;

    [HideInInspector]
    new public CircleCollider2D collider;

    public AudioClip clip;
    public DataSettings dataSettings;

    void OnEnable()
    {
        collider = GetComponent<CircleCollider2D>();
        PersistentDataManager.RegisterPersister(this);
    }

    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        collider = GetComponent<CircleCollider2D>();
        collider.radius = 5;
        collider.isTrigger = true;
        dataSettings = new DataSettings();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMaskExtension.CheckLayerNameInMask(layers, other.gameObject.layer))
        {
            var ic = other.GetComponent<InventoryController>();
            ic.AddItem(inventoryKey, amount);
            if (disableOnEnter)
            {
                gameObject.SetActive(false);
                Save();
            }

            if (clip) AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }

    public void Save()
    {
        PersistentDataManager.SetDirty(this);
    }

    public void Remove()
    {
        PersistentDataManager.SetDirty(this);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "InventoryItem", false);
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
        return new SData<bool>(gameObject.activeSelf);
    }

    public void LoadData(SData data)
    {
        SData<bool> inventoryItemData = (SData<bool>)data;
        gameObject.SetActive(inventoryItemData.value);
    }
}
