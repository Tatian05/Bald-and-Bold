using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseEnemyManager : MonoBehaviour
{
    protected GameManager _gameManager;
    [SerializeField] protected List<Enemy> _allEnemies = new List<Enemy>();
    protected int _maxEnemies;
    protected int _enemiesInLevel;

    public event Action OnEnemyKilled;
    public event Action OnHeavyAttack;

    public abstract void Start();

    public virtual void AddEnemy(Enemy enemy)
    {
        if (_allEnemies.Contains(enemy)) return;

        _allEnemies.Add(enemy);
        _enemiesInLevel = _allEnemies.Count;
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
