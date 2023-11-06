using UnityEngine;
public class GranadeLauncher : FireWeapon
{
    protected override void FireWeaponShoot()
    {
        var granade = FRY_Granades.Instance.pool.GetObject().
                                            SetDamage(_weaponData.damage).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(transform.right).
                                            SetScale(_bulletScale);
        granade.ThrowGranade();
    }
}
