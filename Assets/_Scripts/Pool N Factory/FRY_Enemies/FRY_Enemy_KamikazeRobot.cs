using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_Enemy_KamikazeRobot : MonoBehaviour
{
    static FRY_Enemy_KamikazeRobot _instance;
    public static FRY_Enemy_KamikazeRobot Instance { get { return _instance; } }


    [SerializeField] Enemy_KamikazeRobot _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<Enemy_KamikazeRobot> pool;


    void Awake()
    {
        _instance = this;
        pool = new ObjectPool<Enemy_KamikazeRobot>(ObjectCreator, Enemy_KamikazeRobot.TurnOn, Enemy_KamikazeRobot.TurnOff, _stock);
    }

    public Enemy_KamikazeRobot ObjectCreator()
    {
        var o = Instantiate(_prefab);
        o.transform.SetParent(GameObject.Find("MainGame").transform);
        return o;
    }

    public void ReturnObject(Enemy_KamikazeRobot b)
    {
        pool.ReturnObject(b);
    }
}
