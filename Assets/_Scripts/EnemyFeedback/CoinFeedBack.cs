using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFeedBack : MonoBehaviour
{
    [SerializeField] float _maxLifeCount = 1;
    float _currentLifeCount;

    private void Update()
    {
        _currentLifeCount += Time.deltaTime;
        if (_currentLifeCount > _maxLifeCount) ReturnObject();
    }

    public CoinFeedBack SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    private void Reset()
    {
        _currentLifeCount = 0;
    }

    public static void TurnOn(CoinFeedBack b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(CoinFeedBack b)
    {
        b.gameObject.SetActive(false);
    }
    public virtual void ReturnObject()
    {
        FRY_CoinFeedback.Instance.ReturnObject(this);
    }
}
