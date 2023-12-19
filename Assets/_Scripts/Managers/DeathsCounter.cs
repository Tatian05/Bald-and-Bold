using UnityEngine;
using System.Linq;
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
            persistantDataSaved.levelsCompleted.Add(false);
        }
        int levelIndex = persistantDataSaved.levels.IndexOf(levelName);

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);

        Helpers.LevelTimerManager.RedButton += () =>
        {
            persistantDataSaved.deaths[levelIndex] = _deaths;
            persistantDataSaved.levelsCompleted[levelIndex] = true;
            persistantDataSaved.currentDeaths = persistantDataSaved.deaths.Any() ? persistantDataSaved.deaths.Sum() : default;
        };
        Helpers.LevelTimerManager.OnLevelDefeat += () => persistantDataSaved.deaths[levelIndex] = _deaths;
    }

    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
    }

    int _deaths = 0;
    void OnPlayerDead(params object[] param) { _deaths++; }
}
