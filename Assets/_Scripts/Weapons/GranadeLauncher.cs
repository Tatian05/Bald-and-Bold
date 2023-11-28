public class GranadeLauncher : FireWeapon
{
    protected override void FireWeaponShoot(float bulletScale)
    {
        var granade = FRY_Granades.Instance.pool.GetObject().
                                            SetDamage(_weaponData.damage).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(transform.right).
                                            SetScale(bulletScale).
                                            SetWeaponName(_weaponData.weaponName).
                                            SetSprite(_equipedBulletx16);
        granade.ThrowGranade();
    }
}
