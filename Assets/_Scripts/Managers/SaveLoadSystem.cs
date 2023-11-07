using UnityEngine;
using System.IO;
public class SaveLoadSystem
{
    static readonly string KEY = "sWdhYIPkM6dseGgeuk6mm7ZTO";

    const string DIRECTORY = "/data/";

    #region ENCRYTED SERIALIZE
    public static void SaveData<T>(string fileName, T data, bool encypted)
    {
        string path = Application.persistentDataPath + DIRECTORY + $"{fileName}.json";
        string dir = Application.persistentDataPath + DIRECTORY;

        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        string json = JsonUtility.ToJson(data, true);

        if (encypted)
        {
            var encryptedData = EncryptDecrypt(json);
            File.WriteAllText(path, encryptedData);
        }
        else
            File.WriteAllText(path, json);
    }

    public static T LoadData<T>(string fileName, bool encrypted)
    {
        string path = Application.persistentDataPath + DIRECTORY + $"{fileName}.json";

        if (!FileExist(fileName))
        {
            Debug.Log("JAJA");
            //Debug.LogError($"Cannot load file at {path}. File does not exist!");
            //throw new FileNotFoundException($"{path} does not exist!");
            return default(T);
        }
        string json = File.ReadAllText(path);
        var obj = JsonUtility.FromJson<T>(encrypted ? EncryptDecrypt(json) : json);
        return obj;
    }
    static string EncryptDecrypt(string data)
    {
        string result = string.Empty;
        for (int i = 0; i < data.Length; i++)
            result += (char)(data[i] ^ KEY[i % KEY.Length]);

        return result;
    }

    #endregion

    #region NORMAL JSON SERIALIZE
    public static void SaveData<T>(T data, string fileName)
    {
        string fullPath = Application.persistentDataPath + DIRECTORY + $"{fileName}.json";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
    }
    public static T LoadData<T>(string fileName)
    {
        string fullPath = Application.persistentDataPath + DIRECTORY + $"{fileName}.json";
        if (FileExist(fileName))
        {
            string json = File.ReadAllText(fullPath);
            var obj = JsonUtility.FromJson<T>(json);
            return obj;
        }
        else
            return default(T);
    }
    public static void Delete(string fileName)
    {
        string path = Application.persistentDataPath + DIRECTORY + $"{fileName}.json";
        if (File.Exists(path))
            File.Delete(path);
    }
    public static bool FileExist(string fileName) => File.Exists(Application.persistentDataPath + DIRECTORY + $"{fileName}.json");

    #endregion
}