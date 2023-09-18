using System.Collections.Generic;
using UnityEngine;
public class SaveDataManager : MonoBehaviour
{
    public void SaveFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }
    public void SaveString(string name, string value)
    {
        PlayerPrefs.SetString(name, value);
    }
    public float GetFloat(string name, float defaultValue)
    {
        return PlayerPrefs.HasKey(name) ? PlayerPrefs.GetFloat(name, defaultValue) : defaultValue;
    }

    public void SaveBool(string name, bool value)
    {
        int newValue = value ? 1 : 0;

        PlayerPrefs.SetInt(name, newValue);
    }
    public bool GetBool(string name, bool defaultValue)
    {
        if (!PlayerPrefs.HasKey(name)) return defaultValue;

        bool newValue = PlayerPrefs.GetInt(name) == 1 ? true : false;

        return newValue;     
    }

    public void SaveInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }
    public int GetInt(string name, int defaultValue)
    {
        return PlayerPrefs.HasKey(name) ? PlayerPrefs.GetInt(name, defaultValue) : defaultValue;
    }

    public void SaveLevels(string name ,int[] array)
    {
        var count = 0;
        foreach (var item in array)
        {
            count++;

            PlayerPrefs.SetInt(name + count.ToString(), item);
        }
    }
    public int[] Getrray(string name, int size)
    {
        var list = new List<int>();
        var count = 0;

        for (int i = 0; i < size; i++)
        {
            count++;

            list.Add(PlayerPrefs.GetInt(name + count));
        }

        return list.ToArray();
    }

    public bool CheckForInt(string name)
    {
        return PlayerPrefs.HasKey(name);
    }
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}
