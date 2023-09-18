using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    GameManager _gameManager;

    int _currentDeathCount;
    string _saveDataName = "CurrentDeathCount";

    private void Start()
    {
        _gameManager = GameManager.instance;
        _currentDeathCount = _gameManager.SaveDataManager.GetInt(_saveDataName, 0);

        EventManager.SubscribeToEvent(Contains.ON_ROOM_WON, SaveData);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_ROOM_WON, SaveData);      
    }
    void SaveData(params object[] param)
    {
        _gameManager.SaveDataManager.SaveFloat(_saveDataName, _currentDeathCount);
    }

    public void AddDeath()
    {
        _currentDeathCount++;
    }
}
