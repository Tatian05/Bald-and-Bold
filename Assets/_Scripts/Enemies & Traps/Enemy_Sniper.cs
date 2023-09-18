using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sniper : Enemy_Shooting
{
    public override void Start()
    {
        base.Start();
        OnAttack += Shoot;
    }

    void Shoot()
    {
        FRY_EnemyBullet.Instance.pool.GetObject().SetPosition(bulletSpawnPosition.position)
                                            .SetDirection(armPivot.right)
                                            .SetDmg(bulletDamage)
                                            .SetSpeed(bulletSpeed);
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_Sniper.Instance.pool.ReturnObject(this);
    }
}