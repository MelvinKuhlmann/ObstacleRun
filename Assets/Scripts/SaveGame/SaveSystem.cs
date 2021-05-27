using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region SaveDataPaths
    private static string GetPlayerDataPath()
    {
        return Path.Combine(Application.persistentDataPath, "player.fun");
    }

    private static string GetInventoryDataPath()
    {
        return Path.Combine(Application.persistentDataPath, "inventory.fun");
    }
    #endregion

    #region PlayerData
    public static void SavePlayer(PlayerController player)
    {
        FileStream stream = null;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream = new FileStream(GetPlayerDataPath(), FileMode.Create);

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
        if (!File.Exists(GetPlayerDataPath()))
        {
            Debug.LogError("Save file not found in " + GetPlayerDataPath());
            return null;
        }

        FileStream stream = null;
        PlayerData data = null;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream = new FileStream(GetPlayerDataPath(), FileMode.Open);
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
            stream = new FileStream(GetInventoryDataPath(), FileMode.Create);

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
        if (!File.Exists(GetInventoryDataPath()))
        {
            Debug.LogError("Save file not found in " + GetInventoryDataPath());
            return null;
        }

        FileStream stream = null;
        InventoryData data = null;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream = new FileStream(GetInventoryDataPath(), FileMode.Open);
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
