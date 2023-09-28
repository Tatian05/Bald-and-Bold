using UnityEngine;
public class Enemy_Ground_Ak : Enemy_GroundHumanoid
{
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _armPivot;
    [SerializeField] float _bulletDamage = 1f;
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] float _attackSpeed = 2f;
    float _currentAttackSpeed;

    public override void OnPatrolStart()
    {
        anim.SetBool("IsRunning", true);
        _armPivot.eulerAngles = transform.eulerAngles;
    }

    public override void OnAttackStart()
    {
        anim.SetBool("IsRunning", false);
    }
    public override void OnAttack()
    {
        LookAtPlayer();

        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            Shoot();
            _currentAttackSpeed = 0;
        }
    }

    void Shoot()
    {
        FRY_EnemyBullet.Instance.pool.GetObject().SetPosition(_bulletSpawnPosition.position)
                                            .SetDirection(_armPivot.right)
                                            .SetDmg(_bulletDamage)
                                            .SetSpeed(_bulletSpeed);
    }

    Vector3 _dirLookAt;
    float _r;
    void LookAtPlayer()
    {
        _dirLookAt = (_playerCenterPivot.position - _armPivot.position).normalized;
        float angle = Mathf.Atan2(_dirLookAt.y, Mathf.Abs(_dirLookAt.x)) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(_armPivot.localEulerAngles.z, angle, ref _r, .1f);
        _armPivot.localEulerAngles = new Vector3(0, 0, smoothAngle);
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_Ground_Ak.Instance.pool.ReturnObject(this);
    }
}
