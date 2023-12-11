using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
public class PersistantData : SingletonPersistent<PersistantData>
{
    public TaskSO[] tasksSO;
    public ShoppableSO[] allShoppables;

    #region SHOPPABLES LISTS

    [Header("Equiped")]
    public CosmeticData playerCosmeticEquiped;
    public CosmeticData presidentCosmeticEquiped;
    public BulletData bulletEquiped;
    public BulletData grenadeEquiped;
    public WeaponSkinData pistolEquiped, rifleEquiped, sniperEquiped, grenadeLauncherEquiped;

    [Header("Collection")]
    public List<ShoppableSO> shoppablesInCollection;

    [Header("Consumables Activated")]
    public List<ConsumableData> consumablesActivated;
    public List<float> consumablesActivatedTime;
    public List<ShoppableSO> CosmeticsInCollection => shoppablesInCollection.Except(shoppablesInCollection.OfType<ConsumableData>()).ToList();

    public void AddShoppableToCollection(ShoppableSO shoppable)
    {
        if (shoppable.shoppableType != ShoppableType.Consumable && shoppablesInCollection.Contains(shoppable)) return;

        shoppablesInCollection.Add(shoppable);
    }
    public void RemoveShoppableToCollection(ShoppableSO shoppable)
    {
        if (!shoppablesInCollection.Contains(shoppable)) return;

        shoppablesInCollection.Remove(shoppable);
    }
    void LoadLists()
    {
        foreach (var item in persistantDataSaved.shoppablesInCollectionIDs)
        {
            if (item < 0) continue;
            AddShoppableToCollection(allShoppables[item]);
        }

        if (persistantDataSaved.playerCosmeticEquipedID >= 0) playerCosmeticEquiped = allShoppables[persistantDataSaved.playerCosmeticEquipedID] as CosmeticData;
        if (persistantDataSaved.presidentCosmeticEquipedID >= 0) presidentCosmeticEquiped = allShoppables[persistantDataSaved.presidentCosmeticEquipedID] as CosmeticData;
        if (persistantDataSaved.bulletEquipedID >= 0) bulletEquiped = allShoppables[persistantDataSaved.bulletEquipedID] as BulletData;
        if (persistantDataSaved.grenadeEquipedID >= 0) grenadeEquiped = allShoppables[persistantDataSaved.grenadeEquipedID] as BulletData;
        if(persistantDataSaved.pistolEquipedID >= 0) pistolEquiped = allShoppables[persistantDataSaved.pistolEquipedID] as WeaponSkinData;
        if(persistantDataSaved.rifleEquipedID >= 0) rifleEquiped = allShoppables[persistantDataSaved.rifleEquipedID] as WeaponSkinData;
        if(persistantDataSaved.sniperEquipedID >= 0) sniperEquiped = allShoppables[persistantDataSaved.sniperEquipedID] as WeaponSkinData;
        if(persistantDataSaved.grenadeLauncherID >= 0) grenadeLauncherEquiped = allShoppables[persistantDataSaved.grenadeLauncherID] as WeaponSkinData;

        //CONSUMABLES ACTIVADOS
        consumablesActivated = allShoppables.OfType<ConsumableData>().Where(x => consumablesData.consumablesActivatedIDs.Contains(x.ID)).ToList();
        consumablesActivatedTime = consumablesData.consumablesActivatedTime.ToList();
    }
    public void SaveConsumableActivated(ConsumableData consumable, float time)
    {
        if (consumablesActivated.Contains(consumable))
        {
            consumablesActivatedTime[consumablesActivated.IndexOf(consumable)] = time;
            return;
        }

        consumablesActivated.Add(consumable);
        consumablesActivatedTime.Add(time);
    }
    public void RemoveConsumableActivated(ConsumableData consumable)
    {
        if (!consumablesActivated.Contains(consumable)) return;

        var index = consumablesActivated.IndexOf(consumable);

        if (index != -1)
        {
            consumablesActivated.RemoveAt(index);
            consumablesActivatedTime.RemoveAt(index);
        }
    }

    public WeaponSkinData GetWeaponSkin(FireWeaponType fireWeaponType) => fireWeaponType switch
    {
        FireWeaponType.Pistol => pistolEquiped,
        FireWeaponType.Rifle => rifleEquiped,
        FireWeaponType.Sniper => sniperEquiped,
        FireWeaponType.GrenadeLauncher => grenadeLauncherEquiped,
        _ => throw new ArgumentNullException($"{fireWeaponType} not exist")
    };
    #endregion

    public GameData gameData;
    public PersistantDataSaved persistantDataSaved;
    public Settings settingsData = new Settings { generalVolume = 1, musicVolume = 1, sfxVolume = 1 };
    public Tasks tasks;
    public ConsumablesValues consumablesData;

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
        persistantDataSaved = SaveLoadSystem.LoadData(PERSISTANT_DATA, true, new PersistantDataSaved());
        tasks = SaveLoadSystem.LoadData(TASKS, true, new Tasks(tasksSO.Length));

        persistantDataSaved.Load();
        LoadLists();

        foreach (var item in tasksSO) item.taskProgress = tasks.GetTaskProgress(item.ID);
    }
    public void ResetGame()
    {
        SaveLoadSystem.Delete(GAME_DATA);
        gameData = new GameData();
    }
    public void SaveConsumablesData() => SaveLoadSystem.SaveData(CONSUMABLES_DATA, consumablesData, true);
    protected override async void OnApplicationQuit()
    {
        await Task.Yield();
        persistantDataSaved.Save(shoppablesInCollection, playerCosmeticEquiped, presidentCosmeticEquiped, bulletEquiped, grenadeEquiped, pistolEquiped, rifleEquiped, sniperEquiped, grenadeLauncherEquiped);
        consumablesData.Save(consumablesActivated, consumablesActivatedTime);
        tasks.SaveTasks(tasksSO);

        SaveLoadSystem.SaveData(GAME_DATA, gameData, true);
        SaveLoadSystem.SaveData(SETTINGS_DATA, settingsData, true);
        SaveLoadSystem.SaveData(PERSISTANT_DATA, persistantDataSaved, true);
        SaveLoadSystem.SaveData(TASKS, tasks, true);
        SaveLoadSystem.SaveData(CONSUMABLES_DATA, consumablesData, true);
   
        base.OnApplicationQuit();
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
    #region Tasks
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
    public void SaveTasks(TaskSO[] currentTasks)
    {
        tasksProgress = currentTasks.Select(x => x.taskProgress).ToArray();
    }
}

[Serializable]
public class ConsumablesValues
{
    public float cadenceBoost = 1, bulletScaleBoost = 1, knifeBoost = 1;
    public bool recoil = true, boots, invisible, hasMinigun;
    public int[] consumablesActivatedIDs;
    public float[] consumablesActivatedTime;
    public ConsumablesValues()
    {
        consumablesActivatedIDs = new int[0];
        consumablesActivatedTime = new float[0];
    }
    public void Save(List<ConsumableData> consumablesActivated, List<float> consumablesTime)
    {
        consumablesActivatedIDs = consumablesActivated.Select(x => x.ID).ToArray();
        consumablesActivatedTime = consumablesTime.ToArray();
    }
}

[Serializable]
public class PersistantDataSaved
{
    [Header("Coins")]
    public int presiCoins, goldenBaldCoins;

    #region Cosmetics
    [Header("Cosmetics")]
    public int playerCosmeticEquipedID;
    public int presidentCosmeticEquipedID;
    public int bulletEquipedID, grenadeEquipedID;
    public int pistolEquipedID, rifleEquipedID, sniperEquipedID, grenadeLauncherID;
    public int[] shoppablesInCollectionIDs;
    #endregion 

    #region Bindings
    [Header("Bindings")]
    public List<string> userBindingKeys;
    public List<string> userBindingValues;
    public Dictionary<string, string> userBindings;
    #endregion

    public PersistantDataSaved()
    {
        playerCosmeticEquipedID = -1;
        presidentCosmeticEquipedID = -2;
        bulletEquipedID = -3;
        grenadeEquipedID = -4;
        pistolEquipedID = -5;
        rifleEquipedID = -6;
        sniperEquipedID = -7;
        grenadeLauncherID = -8;
        shoppablesInCollectionIDs = new int[0];
        userBindingKeys = new List<string>();
        userBindingValues = new List<string>();
        userBindings = new Dictionary<string, string>();
    }
    public void Load()
    {
        LoadUserBindingsDictionary();
    }
    public void AddPresiCoins(int amount) { presiCoins += amount; }
    public void AddGoldenBaldCoins(int amount) { goldenBaldCoins += amount; }
    public void Buy(int amount) { presiCoins -= amount; }
    public void Gacha(int amount) { goldenBaldCoins -= amount; }
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
    public void Save(List<ShoppableSO> shoppablesInCollection, CosmeticData playerEquipedCosmetic, CosmeticData presidentEquipedCosmetic,
        BulletData bulletEquiped, BulletData grenadeEquiped, WeaponSkinData pistolEquiped, WeaponSkinData rifleEquiped, WeaponSkinData sniperEquiped,
        WeaponSkinData grenadeLauncherEquiped)
    {
        shoppablesInCollectionIDs = shoppablesInCollection.Select(x => x.ID).ToArray();
        playerCosmeticEquipedID = playerEquipedCosmetic.ID;
        presidentCosmeticEquipedID = presidentEquipedCosmetic.ID;
        bulletEquipedID = bulletEquiped.ID;
        grenadeEquipedID = grenadeEquiped.ID;
        pistolEquipedID = pistolEquiped.ID;
        rifleEquipedID = rifleEquiped.ID;
        sniperEquipedID = sniperEquiped.ID;
        grenadeLauncherID = grenadeLauncherEquiped.ID;
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
    public List<string> levels;
    public List<int> deaths;

    public GameData()
    {
        levels = new List<string>();
        deaths = new List<int>();
    }
}