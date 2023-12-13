using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyManager : BaseEnemyManager
{
    [SerializeField] EnemyData _enemyDataSO;
    Dictionary<string, int> _weaponEnemyDeath = new Dictionary<string, int>();
    public override void Start()
    {
        _gameManager = Helpers.GameManager;
        LevelTimerManager levelTimer = Helpers.LevelTimerManager;
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
        EventManager.SubscribeToEvent(Contains.ON_ROOM_WON, SetTasksProgress);
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, InvisibleConsumable);
        _enemyDataSO.playerPivot = _gameManager.Player.CenterPivot;

        StartCoroutine(CheckForEmptyLevel());
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
        EventManager.UnSubscribeToEvent(Contains.ON_ROOM_WON, SetTasksProgress);
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, InvisibleConsumable);
    }
    void OnPlayerDead(params object[] param)
    {
        ResetLevel();
        StartCoroutine(CheckForEmptyLevel());
    }
    IEnumerator CheckForEmptyLevel()
    {
        yield return new WaitForSeconds(.1f);
        _maxEnemies = _allEnemies.Count;
        if (_maxEnemies == 0) EventManager.TriggerEvent(Contains.ON_ENEMIES_KILLED);
    }

    public override void RemoveEnemy(Enemy enemy)
    {
        if (!_allEnemies.Contains(enemy)) return;

        EnemyKilled();
        _allEnemies.Remove(enemy);

        Helpers.AudioManager.PlaySFX("Enemy_Dead");

        if (_allEnemies.Count == 0) EventManager.TriggerEvent(Contains.ON_ENEMIES_KILLED);
    }

    int _enemiesDeathCounter = 0;
    public override string EnemyCountString()
    {
        _enemiesDeathCounter++;
        return $"{_enemiesDeathCounter}/ {_enemiesInLevel}"; ;
    }
    public void OnWeaponEnemyDeath(string weaponName)
    {
        if (!_weaponEnemyDeath.ContainsKey(weaponName)) _weaponEnemyDeath.Add(weaponName, 0);

        _weaponEnemyDeath[weaponName]++;
    }
    void ResetLevel()
    {
        _enemiesDeathCounter = 0;
        _allEnemies.Clear();
        _weaponEnemyDeath = new Dictionary<string, int>();
    }
    void SetTasksProgress(params object[] param)
    {
        EventManager.TriggerEvent(Contains.MISSION_PROGRESS, "Kill Enemies", _enemiesInLevel);

        foreach (var item in _weaponEnemyDeath)
        {
            Debug.Log($"{item.Key} -- {item.Value}");
            EventManager.TriggerEvent(Contains.MISSION_PROGRESS, item.Key, item.Value);
        }
    }
    void InvisibleConsumable(params object[] param) { _enemyDataSO.playerPivot = (bool)param[0] ? null : _gameManager.Player.CenterPivot; }
}
