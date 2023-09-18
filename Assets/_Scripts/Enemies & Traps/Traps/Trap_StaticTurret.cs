using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trap_StaticTurret : MonoBehaviour
{
    Action OnUpdate;

    [SerializeField] Transform _shootingPoint;
    [SerializeField] float _damage = 1;
    [SerializeField] float _bulletSpeed = 5;
    [SerializeField] float _startDelay;
    [SerializeField] float _attackSpeed;
    float _currentAttackSpeed;

    private void Start()
    {
        StartCoroutine(StartShooting());
    }
    private void Update()
    {
        OnUpdate?.Invoke();
    }
    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(_startDelay);

        OnUpdate += Shoot;
    }
    void Shoot()
    {
        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            FRY_EnemyBullet.Instance.pool.GetObject().SetPosition(_shootingPoint.position)
                                                .SetDirection(_shootingPoint.right)
                                                .SetDmg(_damage)
                                                .SetSpeed(_bulletSpeed);
            _currentAttackSpeed = 0;
        }
    }
}
