using System;
using System.Collections.Generic;
using UnityEngine;
public class PersistantData : MonoBehaviour
{
    public GameData gameData;
    public PersistantDataSaved persistantDataSaved;
    public Settings settingsData = new Settings { generalVolume = 1, musicVolume = 1, sfxVolume = 1 };
    public ConsumablesValues consumablesData;

    public const string GAME_DATA = "Mu9BoZZfUB";
    public const string PERSISTANT_DATA = "jM8SuzEYoW";
    public const string SETTINGS_DATA = "pZasipgofy";
    public const string CONSUMABLES_DATA = "ygWPKikIvb";

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
    public void LoadPersistantData()
    {
        gameData = SaveLoadSystem.LoadData(GAME_DATA, true, () => new GameData());
        settingsData = SaveLoadSystem.LoadData(SETTINGS_DATA, true, () => settingsData);
        consumablesData = SaveLoadSystem.LoadData(CONSUMABLES_DATA, true, () => new ConsumablesValues());
        persistantDataSaved = SaveLoadSystem.LoadData(PERSISTANT_DATA, true, () => new PersistantDataSaved());

        persistantDataSaved.RemoveEmptySlot();
        persistantDataSaved.LoadUserBindingsDictionary();
        consumablesData.LoadActivesConsumables();
    }
    public void DeletePersistantData()
    {
        SaveLoadSystem.Delete(GAME_DATA);
        gameData = new GameData();
    }
    private void OnDestroy()
    {
        SaveLoadSystem.SaveData(GAME_DATA, gameData, true);
        SaveLoadSystem.SaveData(SETTINGS_DATA, settingsData, true);
        SaveLoadSystem.SaveData(PERSISTANT_DATA, persistantDataSaved, true);
        SaveLoadSystem.SaveData(CONSUMABLES_DATA, consumablesData, true);
    }
}

[Serializable]
public struct Settings
{
    public int currentLanguage;
    public float generalVolume, musicVolume, sfxVolume;
    public void SetLanguage(int languageIndex) { currentLanguage = languageIndex; }
    public void SetGeneralVolume(float volume) { generalVolume = volume; }
    public void SetMusicVolume(float volume) { musicVolume = volume; }
    public void SetSFXVolume(float volume) { sfxVolume = volume; }
}

[Serializable]
public class ConsumablesValues
{
    public float cadenceBoost = 1, bulletScaleBoost = 1, knifeBoost = 1;
    public bool recoil = true, boots, invisible, hasMinigun;
    public List<ConsumableData> consumablesActivated = new List<ConsumableData>();
    public List<float> consumablesActivatedTime = new List<float>();
    public Dictionary<ConsumableData, float> consumablesWithTime;
    public void SaveConsumable(ConsumableData consumableData, float time)
    {
        if (consumablesActivated.Contains(consumableData)) return;
        Debug.Log($"Save {consumableData.name}");
        consumablesActivated.Add(consumableData);
        consumablesActivatedTime.Add(time);
    }
    public void RemoveConsumable(ConsumableData consumableData)
    {
        if (!consumablesActivated.Contains(consumableData)) return;

        var index = consumablesActivated.IndexOf(consumableData);
        consumablesActivated.RemoveAt(index);
        consumablesActivatedTime.RemoveAt(index);
    }
    public void LoadActivesConsumables() { consumablesWithTime = consumablesActivated.DictioraryFromTwoLists(consumablesActivatedTime); }
}

[Serializable]
public class PersistantDataSaved
{
    [Header("Coins")]
    public int presiCoins, goldenBaldCoins;

    #region Cosmetics
    [Header("Cosmetics")]
    public CosmeticData playerCosmeticEquiped;
    public CosmeticData presidentCosmeticEquiped;
    public List<CosmeticData> playerCosmeticCollection = new List<CosmeticData>();
    public List<CosmeticData> presidentCosmeticCollection = new List<CosmeticData>();
    #endregion 

    #region Consumables 

    public List<ConsumableData> consumablesInCollection = new List<ConsumableData>();

    #endregion

    #region Bindings
    [Header("Bindings")]
    public List<string> userBindingKeys = new List<string>();
    public List<string> userBindingValues = new List<string>();
    public Dictionary<string, string> userBindings = new Dictionary<string, string>();
    #endregion

    #region Quests
    public Mission[] misions = new Mission[0];
    #endregion

    public void RemoveEmptySlot()
    {
        for (int i = 0; i < playerCosmeticCollection.Count; i++)
            if (!playerCosmeticCollection[i]) playerCosmeticCollection.Remove(playerCosmeticCollection[i]);

        for (int i = 0; i < presidentCosmeticCollection.Count; i++)
            if (!presidentCosmeticCollection[i]) presidentCosmeticCollection.Remove(presidentCosmeticCollection[i]);
    }

    public void AddPresiCoins(int amount) { presiCoins += amount; }
    public void AddGoldenBaldCoins(int amount) { goldenBaldCoins += amount; }
    public void Buy(int amount) { presiCoins -= amount; }
    public void Gacha(int amount) { goldenBaldCoins -= amount; }
    public void AddCosmetic(CosmeticType cosmeticType, CosmeticData cosmetic)
    {
        if (cosmeticType is CosmeticType.Player && !playerCosmeticCollection.Contains(cosmetic)) playerCosmeticCollection.Add(cosmetic);
        else if (!presidentCosmeticCollection.Contains(cosmetic)) presidentCosmeticCollection.Add(cosmetic);
    }
    public void AddConsumable(ConsumableData consumableData) { consumablesInCollection.Add(consumableData); }
    public void RemoveConsumable(ConsumableData consumableData) { consumablesInCollection.Remove(consumableData); }
    public void AddBinding(string key, string value)
    {
        if (userBindings.ContainsKey(key))
        {
            userBindings[key] = value;
            int index = userBindingKeys.IndexOf(key);
            userBindingValues[index] = value;
            return;
        }

        userBindingKeys.Add(key);
        userBindingValues.Add(value);
        userBindings.Add(key, value);
    }
    public string GetBind(string key) => userBindings.ContainsKey(key) ? userBindings[key] : string.Empty;
    public void LoadUserBindingsDictionary() { userBindings = userBindingKeys.DictioraryFromTwoLists(userBindingValues); }
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