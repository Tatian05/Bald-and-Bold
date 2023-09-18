using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_Enemy_Ground_Ak : MonoBehaviour
{
    static FRY_Enemy_Ground_Ak _instance;
    public static FRY_Enemy_Ground_Ak Instance { get { return _instance; } }


    [SerializeField] Enemy_Ground_Ak _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<Enemy_Ground_Ak> pool;


    void Awake()
    {
        _instance = this;
        pool = new ObjectPool<Enemy_Ground_Ak>(ObjectCreator, Enemy_Ground_Ak.TurnOn, Enemy_Ground_Ak.TurnOff, _stock);
    }

    public Enemy_Ground_Ak ObjectCreator()
    {
        var o = Instantiate(_prefab);
        o.transform.SetParent(GameObject.Find("MainGame").transform);
        return o;
    }

    public void ReturnObject(Enemy_Ground_Ak b)
    {
        pool.ReturnObject(b);
    }
}
