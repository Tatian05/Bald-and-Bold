using System.Collections;
using UnityEngine;
public class EnemyManager : BaseEnemyManager
{
    public override void Start()
    {
        _gameManager = Helpers.GameManager;
        LevelTimerManager levelTimer = Helpers.LevelTimerManager;
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);

        StartCoroutine(CheckForEmptyLevel());
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
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
        if (_maxEnemies == 0) EventManager.TriggerEvent(Contains.ON_ROOM_WON);
    }

    public override void RemoveEnemy(Enemy enemy)
    {
        if (!_allEnemies.Contains(enemy)) return;

        EnemyKilled();
        _allEnemies.Remove(enemy);

        Helpers.AudioManager.PlaySFX("Enemy_Dead");

        if (_allEnemies.Count == 0) EventManager.TriggerEvent(Contains.ON_ROOM_WON);
    }

    public override string EnemyCountString()
    {
        return Mathf.Abs(_allEnemies.Count - _maxEnemies).ToString() + "/ " + _maxEnemies.ToString();
    }

    void ResetLevel()
    {
        _allEnemies.Clear();
    }
}
