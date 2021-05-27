using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string playerDataPath = Application.persistentDataPath + "/player.fun";
    private static string inventoryDataPath = Application.persistentDataPath + "/inventory.fun";

    #region PlayerData
    public static void SavePlayer(PlayerController player)
    {
        FileStream stream = null;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream = new FileStream(playerDataPath, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }

    public static PlayerData LoadPlayer()
    {
        if (!File.Exists(playerDataPath))
        {
            Debug.LogError("Save file not found in " + playerDataPath);
            return null;
        }

        FileStream stream = null;
        PlayerData data = null;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream = new FileStream(playerDataPath, FileMode.Open);
            data = formatter.Deserialize(stream) as PlayerData;
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
        return data;
    }
    #endregion

    #region InventoryData
    public static void SaveInventory(InventoryManager inventory)
    {
        FileStream stream = null;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream = new FileStream(inventoryDataPath, FileMode.Create);

            InventoryData data = new InventoryData(inventory);

            formatter.Serialize(stream, data);
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }

    public static InventoryData LoadInventory()
    {
        if (!File.Exists(inventoryDataPath))
        {
            Debug.LogError("Save file not found in " + inventoryDataPath);
            return null;
        }

        FileStream stream = null;
        InventoryData data = null;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream = new FileStream(inventoryDataPath, FileMode.Open);
            data = formatter.Deserialize(stream) as InventoryData;
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
        return data;
    }
    #endregion
}
