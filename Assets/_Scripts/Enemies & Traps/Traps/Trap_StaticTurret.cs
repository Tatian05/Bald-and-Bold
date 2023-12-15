using System.Collections;
using UnityEngine;
using System;

public class Trap_StaticTurret : MonoBehaviour
{
    [SerializeField] Transform _shootingPoint;
    [SerializeField] ParticleSystem _sparksShootPT;
    [SerializeField] float _damage = 1;
    [SerializeField] float _bulletSpeed = 5;
    [SerializeField] float _attackSpeed;
    float _currentAttackSpeed;

    Action onUpdate;
    private void Start()
    {
        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, OnStart);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, OnStart);
        onUpdate -= Shoot;
    }
    private void Update()
    {
        onUpdate?.Invoke();
    }
    void Shoot()
    {
        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            _sparksShootPT.Play();
            FRY_EnemyBullet.Instance.pool.GetObject().SetPosition(_shootingPoint.position)
                                                .SetDirection(_shootingPoint.right)
                                                .SetDmg(_damage)
                                                .SetSpeed(_bulletSpeed);
            _currentAttackSpeed = 0;
        }
    }
    void OnStart(params object[] param) { onUpdate += Shoot; }
}
