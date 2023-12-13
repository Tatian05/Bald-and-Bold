using UnityEngine;
public abstract class Enemy_Shooters : Enemy
{
    [SerializeField] protected Transform _bulletSpawnPosition;
    [SerializeField] protected Transform _armPivot, _gunTransform;
    [SerializeField] protected float _bulletDamage = 1f, _bulletSpeed = 10f, _attackSpeed = 2f;
    protected Vector3 _dirToLook;

    Vector3 _weaponDir;
    float _r, _angle, _smoothAngle;
    protected void WeaponRot()
    {
        _weaponDir = (_enemyDataSO.playerPivot.position - _armPivot.position).normalized;
        _angle = CanSeePlayer() ? Mathf.Atan2(_weaponDir.y, _weaponDir.x * Mathf.Sign(_weaponDir.x)) * Mathf.Rad2Deg : 0;
        _smoothAngle = Mathf.SmoothDampAngle(_armPivot.localEulerAngles.z, _angle, ref _r, .1f);
        _armPivot.localEulerAngles = new Vector3(0, 0, _smoothAngle);
    }

    Vector3 newEnemyRot = Vector3.zero;
    protected void LookAtPlayer()
    {
        newEnemyRot.y = Mathf.Sign(DistanceToPlayer().x) < 0 ? 180 : 0;
        transform.eulerAngles = newEnemyRot;
    }
    protected void FlipEnemy()
    {
        _dirToLook *= -1;
        float angle = Mathf.Sign(_dirToLook.x) < 0 ? 180 : 0;

        transform.eulerAngles = new Vector3(0, angle, 0);
    }
    protected abstract void Shoot();
}
