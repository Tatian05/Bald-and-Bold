using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class PersistantData : MonoBehaviour
{
    public event Action savePersistantData = delegate { };
    public GameData gameData;
    public PersistantDataSaved persistantDataSaved;

    public const string GAME_DATA = "Game data";
    public const string PERSISTANT_DATA = "Persistant data";
    public static PersistantData Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        LoadPersistantData();
    }
    public void SavePersistantData()
    {
        savePersistantData();
        SaveLoadSystem.SaveGameData(gameData, GAME_DATA);
    }
    public void LoadPersistantData()
    {
        gameData = File.Exists(Application.persistentDataPath + $"/{GAME_DATA}.json") ? SaveLoadSystem.LoadGameData(GAME_DATA) : new GameData();
        persistantDataSaved = File.Exists(Application.persistentDataPath + $"/{GAME_DATA}.json") ? SaveLoadSystem.LoadPersistantData(PERSISTANT_DATA) : new PersistantDataSaved();
        persistantDataSaved.RemoveEmptySlot();
    }
    public void DeletePersistantData()
    {
        SaveLoadSystem.Delete(GAME_DATA);
        gameData = new GameData();
    }
    public void ResetCoins() { PlayerPrefs.DeleteKey("Coins"); }
    private void OnDestroy()
    {
        SavePersistantData();
        SaveLoadSystem.SavePersistantData(persistantDataSaved, PERSISTANT_DATA);
        ResetCoins();
    }
}

[Serializable]
public class PersistantDataSaved
{
    public int coins, currentLanguageIndex;
    public CosmeticData playerCosmeticEquiped;
    public CosmeticData presidentCosmeticEquiped;
    public List<CosmeticData> playerCosmeticCollection = new List<CosmeticData>();
    public List<CosmeticData> presidentCosmeticCollection = new List<CosmeticData>();
    public void RemoveEmptySlot()
    {
        for (int i = 0; i < playerCosmeticCollection.Count; i++)
            if (!playerCosmeticCollection[i]) playerCosmeticCollection.Remove(playerCosmeticCollection[i]);

        for (int i = 0; i < presidentCosmeticCollection.Count; i++)
            if (!presidentCosmeticCollection[i]) presidentCosmeticCollection.Remove(presidentCosmeticCollection[i]);
    }
    public void Buy(int amount) { coins -= amount; }
    public void AddPlayerCosmetic(CosmeticData cosmetic) { if (!playerCosmeticCollection.Contains(cosmetic)) playerCosmeticCollection.Add(cosmetic); }
    public void AddPresidentCosmetic(CosmeticData cosmetic) { if (!presidentCosmeticCollection.Contains(cosmetic)) presidentCosmeticCollection.Add(cosmetic); }
}

[Serializable]
public class GameData
{
    public int unlockedZones = 0;
    public int currentLevel = 1;
    public int gameMode = 0;
    public bool firstTime = true;
    public bool firstTimeLevelsMap = true;
    public int currentDeaths;

    //Deaths per level
    public List<string> levels = new List<string>();
    public List<int> deaths = new List<int>();
}