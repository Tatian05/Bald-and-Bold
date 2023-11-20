using System.Collections;
using UnityEngine;
public class Trap_Mine : Enemy
{
    [SerializeField] GameObject _damageOnTriggerGO;
    [SerializeField] GameObject _parentGO;
    private bool _isInCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isInCoroutine) Explode();

    }

    void Explode()
    {
        _isInCoroutine = true;
        _damageOnTriggerGO.SetActive(true);
    }

    protected override void PlayerInvisibleConsumable(params object[] param)
    {
        enabled = !(bool)param[0];
    }
}
