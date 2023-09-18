using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T>
{
    Func<T> creationLogic;

    List<T> currentStock;
    bool isDynamic;

    Action<T> turnOnCallback;
    Action<T> turnOffCallback;

    public ObjectPool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialStock = 0, bool isDynamic = true)
    {
        creationLogic = factoryMethod;
        this.turnOnCallback = turnOnCallback;
        this.turnOffCallback = turnOffCallback;

        this.isDynamic = isDynamic;

        currentStock = new List<T>();

        for (int i = 0; i < initialStock; i++)
        {
            T obj = creationLogic();

            this.turnOffCallback(obj);

            currentStock.Add(obj);
        }
    }
    public T GetObject()
    {
        var result = default(T);

        if (currentStock.Count > 0)
        {
            result = currentStock[0];
            currentStock.RemoveAt(0);
        }
        else if (isDynamic)
        {
            result = creationLogic();
        }

        turnOnCallback(result);

        return result;
    }
    public void ReturnObject(T obj)
    {
        turnOffCallback(obj);
        currentStock.Add(obj);
    }
}
