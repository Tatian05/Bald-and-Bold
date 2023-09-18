using UnityEngine;
using System.Linq;
public class ZonesManager : MonoBehaviour
{
    public static ZonesManager Instance;
    public Zone[] zones;
    public string[] lastLevelsZone;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        lastLevelsZone = new string[zones.Length - 1];
        for (int i = 0; i < lastLevelsZone.Length; i++)
            lastLevelsZone[i] = zones[i].levelsZone.Last();
    }
}

[System.Serializable]
public class Zone
{
    public string[] levelsZone;
    public int deathsNeeded;
    public int currentDeathsInZone;
    public int ID;
    public void SetCurrentDeaths()
    {
        currentDeathsInZone = 0;
        for (int i = 0; i < levelsZone.Length; i++)
        {
            int index = Helpers.PersistantData.gameData.levels.IndexOf(levelsZone[i]);
            int deathNum = index < 0 ? 0 : Helpers.PersistantData.gameData.deaths[index];
            currentDeathsInZone += deathNum;
        }
    }
}