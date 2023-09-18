using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
public class DeathsCounter : MonoBehaviour
{
    void Start()
    {
        GameData persistantDataSaved = Helpers.PersistantData.gameData;

        var levelName = SceneManager.GetActiveScene().name;

        if (!persistantDataSaved.levels.Contains(levelName))
        {
            persistantDataSaved.levels.Add(levelName);
            persistantDataSaved.deaths.Add(0);
        }

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);

        Helpers.LevelTimerManager.RedButton += () =>
        {
            persistantDataSaved.deaths[persistantDataSaved.levels.IndexOf(levelName)] = _deaths;
            persistantDataSaved.currentDeaths = persistantDataSaved.deaths.Any() ? persistantDataSaved.deaths.Sum() : default;
        };
        Helpers.LevelTimerManager.OnLevelDefeat += () => persistantDataSaved.deaths[persistantDataSaved.levels.IndexOf(levelName)] = _deaths;
    }

    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);        
    }

    int _deaths = 0;
    void OnPlayerDead(params object[] param) { _deaths++; }
}
