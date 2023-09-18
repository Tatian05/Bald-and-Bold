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
        _anim.SetBool("IsRunning", true);
        _armPivot.eulerAngles = transform.eulerAngles;
    }

    public override void OnAttackStart()
    {
        _anim.SetBool("IsRunning", false);
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

    void LookAtPlayer()
    {
        Vector3 dirToLookAt = (gameManager.Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, Mathf.Abs(dirToLookAt.x)) * Mathf.Rad2Deg;

        _armPivot.localEulerAngles = new Vector3(0, 0, angle);
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_Ground_Ak.Instance.pool.ReturnObject(this);
    }
}
