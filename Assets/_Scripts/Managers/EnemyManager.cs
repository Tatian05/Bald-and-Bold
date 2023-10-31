using System.Collections;
using UnityEngine;
public class EnemyManager : BaseEnemyManager
{
    public override void Start()
    {
        _gameManager = Helpers.GameManager;
        LevelTimerManager levelTimer = Helpers.LevelTimerManager;
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
        EventManager.SubscribeToEvent(Contains.ON_ROOM_WON, SetProgressEnemyMision);

        StartCoroutine(CheckForEmptyLevel());
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
        EventManager.UnSubscribeToEvent(Contains.ON_ROOM_WON, SetProgressEnemyMision);
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

    public override string EnemyCountString()
    {
        return Mathf.Abs(_allEnemies.Count - _maxEnemies).ToString() + "/ " + _maxEnemies.ToString();
    }

    void ResetLevel()
    {
        _allEnemies.Clear();
    }

    void SetProgressEnemyMision(params object[] param) { EventManager.TriggerEvent(Contains.MISSION_PROGRESS, "Kill Enemies", _enemiesInLevel); }
}
