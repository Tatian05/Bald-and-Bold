using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_KeysUI : MonoBehaviour
{
    static FRY_KeysUI _instance;
    public static FRY_KeysUI Instance { get { return _instance; } }


    [SerializeField] KeysUI _prefab;
    [SerializeField] int _stock = 3;

    public ObjectPool<KeysUI> pool;


    void Start()
    {
        _instance = this;
        pool = new ObjectPool<KeysUI>(ObjectCreator, KeysUI.TurnOn, KeysUI.TurnOff, _stock);
    }

    public KeysUI ObjectCreator()
    {
        var enemy = Instantiate(_prefab);
        enemy.transform.SetParent(transform);
        return enemy;
    }

    public void ReturnObject(KeysUI b)
    {
        pool.ReturnObject(b);
    }
}
