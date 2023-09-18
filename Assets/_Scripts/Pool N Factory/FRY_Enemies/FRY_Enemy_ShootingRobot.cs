using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_Enemy_ShootingRobot : MonoBehaviour
{
    static FRY_Enemy_ShootingRobot _instance;
    public static FRY_Enemy_ShootingRobot Instance { get { return _instance; } }


    [SerializeField] Enemy_ShootingRobot _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<Enemy_ShootingRobot> pool;


    void Awake()
    {
        _instance = this;
        pool = new ObjectPool<Enemy_ShootingRobot>(ObjectCreator, Enemy_ShootingRobot.TurnOn, Enemy_ShootingRobot.TurnOff, _stock);
    }

    public Enemy_ShootingRobot ObjectCreator()
    {
        var o = Instantiate(_prefab);
        o.transform.SetParent(GameObject.Find("MainGame").transform);
        return o;
    }

    public void ReturnObject(Enemy_ShootingRobot b)
    {
        pool.ReturnObject(b);
    }
}
