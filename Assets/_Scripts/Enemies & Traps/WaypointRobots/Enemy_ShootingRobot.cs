using UnityEngine;
public enum ShootingRobot { Idle, Shoot }
public class Enemy_ShootingRobot : Enemy_Shooters
{
    IMovement _armRotation;
    float _currentAttackSpeed;
    EventFSM<ShootingRobot> _myFSM;
    State<ShootingRobot> IDLE;

    [SerializeField] float _speed;
    Vector3 _dir;
    public override void Start()
    {
        base.Start();
        _armRotation = new Movement_RotateObject(_armPivot, _playerCenterPivot, _armPivot);

        IDLE = new State<ShootingRobot>("IDLE");
        var SHOOT = new State<ShootingRobot>("SHOOT");

        StateConfigurer.Create(IDLE).SetTransition(ShootingRobot.Shoot, SHOOT).Done();
        StateConfigurer.Create(SHOOT).SetTransition(ShootingRobot.Idle, IDLE).Done();

        IDLE.OnEnter += x => AgroSign(false);

        SHOOT.OnEnter += x => AgroSign(true);
        SHOOT.OnUpdate += delegate
        {
            _currentAttackSpeed += Time.deltaTime;
            if (_currentAttackSpeed > _attackSpeed)
            {
                _currentAttackSpeed = 0;
                Shoot();
            }

            _armRotation.Move();
        };
    }

    public override void Update()
    {
        _myFSM?.Update();
        transform.position += _dir * _speed * Time.deltaTime;
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

    protected override void Shoot()
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InvisibleWall"))
            FlipEnemy();
    }
}
