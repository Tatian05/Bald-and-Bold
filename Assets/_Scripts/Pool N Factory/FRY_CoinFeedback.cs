using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_CoinFeedback : MonoBehaviour
{
    static FRY_CoinFeedback _instance;
    public static FRY_CoinFeedback Instance { get { return _instance; } }


    [SerializeField] CoinFeedBack _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<CoinFeedBack> pool;


    void Start()
    {
        _instance = this;
        pool = new ObjectPool<CoinFeedBack>(ObjectCreator, CoinFeedBack.TurnOn, CoinFeedBack.TurnOff, _stock);
    }

    public CoinFeedBack ObjectCreator()
    {
        var enemy = Instantiate(_prefab);
        enemy.transform.SetParent(transform);
        return enemy;
    }

    public void ReturnObject(CoinFeedBack b)
    {
        pool.ReturnObject(b);
    }
}
