[System.Serializable]
public class InventoryData
{
    public int collectedSouls;

    public InventoryData(InventoryManager inventoryManager)
    {
        collectedSouls = inventoryManager.GetCurrentSouls();
    }

}