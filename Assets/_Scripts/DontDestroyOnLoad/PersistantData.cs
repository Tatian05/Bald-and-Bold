using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PersistantData : SingletonPersistent<PersistantData>
{
    public GameData gameData;
    public PersistantDataSaved persistantDataSaved;
    public Settings settingsData = new Settings { generalVolume = 1, musicVolume = 1, sfxVolume = 1 };
    public Tasks tasks;
    public ConsumablesValues consumablesData;
    public TaskSO[] tasksSO;

    const string GAME_DATA = "Mu9BoZZfUB";
    const string PERSISTANT_DATA = "jM8SuzEYoW";
    const string SETTINGS_DATA = "pZasipgofy";
    const string CONSUMABLES_DATA = "ygWPKikIvb";
    const string TASKS = "EYeLxnrVvi";

    private void Start()
    {
        LoadPersistantData();
    }
    public void LoadPersistantData()
    {
        gameData = SaveLoadSystem.LoadData(GAME_DATA, true, new GameData());
        settingsData = SaveLoadSystem.LoadData(SETTINGS_DATA, true, settingsData);
        consumablesData = SaveLoadSystem.LoadData(CONSUMABLES_DATA, true, new ConsumablesValues());
        persistantDataSaved = SaveLoadSystem.LoadData(PERSISTANT_DATA, true,
            new PersistantDataSaved(Resources.Load<CosmeticData>("Player Default"),
                                    Resources.Load<CosmeticData>("Presi Default"),
                                    Resources.Load<BulletData>("Bullet Default")));
        tasks = SaveLoadSystem.LoadData(TASKS, true, new Tasks(tasksSO.Length));

        persistantDataSaved.Init();

        foreach (var item in tasksSO) item.taskProgress = tasks.GetTaskProgress(item.ID);
    }
    public void ResetGame()
    {
        SaveLoadSystem.Delete(GAME_DATA);
        gameData = new GameData();
    }
    public void SaveConsumablesData() => SaveLoadSystem.SaveData(CONSUMABLES_DATA, consumablesData, true);
    private void OnDestroy()
    {
        tasks.SetTaskProgress(tasksSO);
        SaveLoadSystem.SaveData(GAME_DATA, gameData, true);
        SaveLoadSystem.SaveData(SETTINGS_DATA, settingsData, true);
        SaveLoadSystem.SaveData(PERSISTANT_DATA, persistantDataSaved, true);
        SaveLoadSystem.SaveData(TASKS, tasks, true);
    }

    public void BorrarProgresoDeNiveles()
    {
        SaveLoadSystem.Delete(GAME_DATA);
    }
    public void BorrarAjustes()
    {
        SaveLoadSystem.Delete(SETTINGS_DATA);
    }
    public void BorrarSkinsYWeas()
    {
        SaveLoadSystem.Delete(PERSISTANT_DATA);
    }
    public void BorrarConsumiblesActivados()
    {
        SaveLoadSystem.Delete(CONSUMABLES_DATA);
    }
    public void BorrarProgresoDeTareas()
    {
        SaveLoadSystem.Delete(TASKS);
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
public struct Tasks
{
    #region Quests
    public TaskProgress[] tasksProgress;
    public UI_TaskVariables[] UI_Task_Progress;
    #endregion
    public Tasks(int length)
    {
        tasksProgress = new TaskProgress[length];
        UI_Task_Progress = new UI_TaskVariables[length];
    }
    public UI_TaskVariables GetUITaskProgress(int index) => UI_Task_Progress[index];
    public TaskProgress GetTaskProgress(int index) => tasksProgress[index];
    public void SetUITaskProgress(int index, UI_TaskVariables progress) => UI_Task_Progress[index] = progress;
    public void SetTaskProgress(TaskSO[] taskSO) => tasksProgress = taskSO.Select(x => x.taskProgress).ToArray();
}

[Serializable]
public class ConsumablesValues
{
    public float cadenceBoost = 1, bulletScaleBoost = 1, knifeBoost = 1;
    public bool recoil = true, boots, invisible, hasMinigun;
    public List<ConsumableData> consumablesActivated;
    public List<float> consumablesActivatedTime;
    public Dictionary<ConsumableData, float> consumablesWithTime;
    public ConsumablesValues()
    {
        consumablesActivated = new List<ConsumableData>();
        consumablesActivatedTime = new List<float>();
    }
    public void SaveConsumable(ConsumableData consumableData, float time)
    {
        if (consumablesActivated.Contains(consumableData))
        {
            var index = consumablesActivated.IndexOf(consumableData);
            if (index != -1)
                consumablesActivatedTime[index] = time;
            return;
        }

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
    public Dictionary<ConsumableData, float> LoadActivesConsumables() => consumablesActivated.DictioraryFromTwoLists(consumablesActivatedTime);
}

[Serializable]
public class PersistantDataSaved
{
    [Header("Coins")]
    public int presiCoins, goldenBaldCoins;

    public List<ShoppableSO> cosmeticsInCollection;

    #region Cosmetics
    [Header("Cosmetics")]
    public CosmeticData playerCosmeticEquiped;
    public CosmeticData presidentCosmeticEquiped;
    public BulletData bulletEquiped;
    public List<CosmeticData> playerCosmeticCollection;
    public List<CosmeticData> presidentCosmeticCollection;
    #endregion 

    #region Consumables 

    public List<ConsumableData> consumablesInCollection;

    #endregion

    #region Bullets

    public List<BulletData> bulletsInCollection;

    #endregion

    #region Bindings
    [Header("Bindings")]
    public List<string> userBindingKeys;
    public List<string> userBindingValues;
    public Dictionary<string, string> userBindings;
    #endregion

    public PersistantDataSaved(CosmeticData playerDefault, CosmeticData presiDefault, BulletData bulletDefault)
    {
        playerCosmeticEquiped = playerDefault;
        presidentCosmeticEquiped = presiDefault;
        bulletEquiped = bulletDefault;
        playerCosmeticCollection = new List<CosmeticData>();
        presidentCosmeticCollection = new List<CosmeticData>();
        consumablesInCollection = new List<ConsumableData>();
        bulletsInCollection = new List<BulletData>();
        userBindingKeys = new List<string>();
        userBindingValues = new List<string>();
        userBindings = new Dictionary<string, string>();
    }
    public void Init()
    {
        playerCosmeticEquiped.OnStart();
        presidentCosmeticEquiped.OnStart();
        bulletEquiped.OnStart();
        RemoveEmptySlot();
        LoadUserBindingsDictionary();
        LoadCosmeticsCollection();
    }
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
        else if (cosmeticType is CosmeticType.President && !presidentCosmeticCollection.Contains(cosmetic)) presidentCosmeticCollection.Add(cosmetic);
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
    public void AddBullet(BulletData bulletData)
    {
        if (bulletsInCollection.Contains(bulletData)) return;
        bulletsInCollection.Add(bulletData);
    }
    public string GetBind(string key) => userBindings.ContainsKey(key) ? userBindings[key] : string.Empty;
    public void LoadUserBindingsDictionary() { userBindings = userBindingKeys.DictioraryFromTwoLists(userBindingValues); }
    public void LoadCosmeticsCollection() { cosmeticsInCollection = playerCosmeticCollection.OfType<ShoppableSO>().Concat(presidentCosmeticCollection).Concat(bulletsInCollection).ToList(); }
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
    public List<string> levels;
    public List<int> deaths;

    public GameData()
    {
        levels = new List<string>();
        deaths = new List<int>();
    }
}