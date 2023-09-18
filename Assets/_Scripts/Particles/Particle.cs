using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    float _maxLife = 1;
    float _currentLife;

    private void Update()
    {
        _currentLife += Time.deltaTime;
        if (_currentLife >= _maxLife) ReturnObject();
    }

    public Particle SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    private void Reset()
    {
        _currentLife = 0;
    }

    public static void TurnOn(Particle b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Particle b)
    {
        b.gameObject.SetActive(false);
    }
    public virtual void ReturnObject()
    {
    }
}
