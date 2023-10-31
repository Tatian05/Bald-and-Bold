using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseEnemyManager : MonoBehaviour
{
    protected GameManager _gameManager;
    protected List<Enemy> _allEnemies = new List<Enemy>();

    public event Action OnEnemyKilled;
    public event Action OnHeavyAttack;

    protected int _maxEnemies;
    protected int _enemiesInLevel;
    public abstract void Start();

    public virtual void AddEnemy(Enemy enemy)
    {
        if (_allEnemies.Contains(enemy)) return;

        _allEnemies.Add(enemy);
        _enemiesInLevel++;
    }

    public abstract void RemoveEnemy(Enemy enemy);

    public abstract string EnemyCountString();

    public virtual void HeavyAttack()
    {
        OnHeavyAttack?.Invoke();
    }

    public virtual void EnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }
}
