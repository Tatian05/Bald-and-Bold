using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ShootingRobot : Enemy_Waypoint
{
    IMovement _armRotation;
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _armPivot;

    [SerializeField] float _bulletSpeed = 10;
    [SerializeField] float _attackSpeed = 2f;
    float _currentAttackSpeed;

    public override void Start()
    {
        base.Start();
        _armRotation = new Movement_RotateObject(_armPivot, _armPivot);
       
        OnUpdate += CalculateAttack;
        OnUpdate += () => { if (CanSeePlayer()) _armRotation.Move(); };
    }

    public override bool CanSeePlayer()
    {
        return gameManager.Player ? !Physics2D.Raycast(transform.position, DistanceToPlayer().normalized, DistanceToPlayer().magnitude, gameManager.BorderLayer) : false;
    }
    void CalculateAttack()
    {
        if (!CanSeePlayer()) return;

        
        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            _currentAttackSpeed = 0;

            Shoot();
        }
    }

    void Shoot()
    {

        FRY_EnemyBullet.Instance.pool.GetObject().SetDmg(1f)
                                            .SetSpeed(_bulletSpeed)
                                            .SetPosition(_bulletSpawnPosition.position)
                                            .SetDirection(_armPivot.right);
     
    }
    public override void Reset()
    {
        _currentAttackSpeed = 0;

        base.Reset();
    }
    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_ShootingRobot.Instance.pool.ReturnObject(this);
    }
}
