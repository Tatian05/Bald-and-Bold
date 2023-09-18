using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_Enemy_Sniper : MonoBehaviour
{
    static FRY_Enemy_Sniper _instance;
    public static FRY_Enemy_Sniper Instance { get { return _instance; } }


    [SerializeField] Enemy_Sniper _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<Enemy_Sniper> pool;


    void Awake()
    {
        _instance = this;
        pool = new ObjectPool<Enemy_Sniper>(ObjectCreator, Enemy_Sniper.TurnOn, Enemy_Sniper.TurnOff, _stock);
    }

    public Enemy_Sniper ObjectCreator()
    {
        var o = Instantiate(_prefab);
        o.transform.SetParent(GameObject.Find("MainGame").transform);
        return o;
    }

    public void ReturnObject(Enemy_Sniper b)
    {
        pool.ReturnObject(b);
    }
}
