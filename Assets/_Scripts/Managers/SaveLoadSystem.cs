using UnityEngine;
using System.IO;
public class SaveLoadSystem
{
    public static void SaveGameData(GameData data, string fileName)
    {
        string fullPath = Application.persistentDataPath + $"/{fileName}.json";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
    }
    public static GameData LoadGameData(string fileName)
    {
        string fullPath = Application.persistentDataPath + $"/{fileName}.json";
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            var obj = JsonUtility.FromJson<GameData>(json);
            return obj;
        }
        else
            return default;
    }
    public static void SavePersistantData(PersistantDataSaved data, string fileName)
    {
        string fullPath = Application.persistentDataPath + $"/{fileName}.json";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
    }
    public static PersistantDataSaved LoadPersistantData(string fileName)
    {
        string fullPath = Application.persistentDataPath + $"/{fileName}.json";
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            var obj = JsonUtility.FromJson<PersistantDataSaved>(json);
            return obj;
        }
        else
            return default;
    }
    public static void Delete(string file)
    {
        File.Delete(Application.persistentDataPath + $"/{file}.json");
    }
}