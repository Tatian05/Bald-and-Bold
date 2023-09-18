using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_EnemyFeedback : MonoBehaviour
{
    static FRY_EnemyFeedback _instance;
    public static FRY_EnemyFeedback Instance { get { return _instance; } }


    [SerializeField] EnemyFeedback _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<EnemyFeedback> pool;


    void Start()
    {
        _instance = this;
        pool = new ObjectPool<EnemyFeedback>(ObjectCreator, EnemyFeedback.TurnOn, EnemyFeedback.TurnOff, _stock);
    }

    public EnemyFeedback ObjectCreator()
    {
        var enemy = Instantiate(_prefab);
        enemy.transform.SetParent(transform);
        return enemy;
    }

    public void ReturnObject(EnemyFeedback b)
    {
        pool.ReturnObject(b);
    }
}
