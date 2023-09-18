using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_Enemy_ChargeDrone : MonoBehaviour
{
    static FRY_Enemy_ChargeDrone _instance;
    public static FRY_Enemy_ChargeDrone Instance { get { return _instance; } }


    [SerializeField] Enemy_ChargeDrone _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<Enemy_ChargeDrone> pool;


    void Awake()
    {
        _instance = this;
        pool = new ObjectPool<Enemy_ChargeDrone>(ObjectCreator, Enemy_ChargeDrone.TurnOn, Enemy_ChargeDrone.TurnOff, _stock);
    }

    public Enemy_ChargeDrone ObjectCreator()
    {
        var o = Instantiate(_prefab);
        o.transform.SetParent(GameObject.Find("MainGame").transform);
        return o;
    }

    public void ReturnObject(Enemy_ChargeDrone b)
    {
        pool.ReturnObject(b);
    }
}
