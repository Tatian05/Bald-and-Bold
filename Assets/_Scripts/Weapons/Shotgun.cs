using UnityEngine;
public class Shotgun : FireWeapon
{
    [SerializeField] Transform[] _directions;
    protected override void FireWeaponShoot(float bulletScale)
    {
        for (int i = 0; i <= _directions.Length - 1; i++)
        {
            FRY_PlayerBullet.Instance.pool.GetObject().
                                                SetDirection(_directions[i].position - _bulletSpawn.position).
                                                SetDmg(_weaponData.damage).
                                                SetPosition(_bulletSpawn.position).
                                                SetSpeed(_weaponData.bulletSpeed).
                                                SetScale(bulletScale);
        }
    }
}
