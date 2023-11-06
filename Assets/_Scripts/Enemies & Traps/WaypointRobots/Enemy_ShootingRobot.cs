using UnityEngine;
using DG.Tweening;
public enum ShootingRobot { Idle, Shoot }
public class Enemy_ShootingRobot : Enemy_Shooters
{
    IMovement _armRotation;
    float _currentAttackSpeed;
    EventFSM<ShootingRobot> _myFSM;
    State<ShootingRobot> IDLE;

    [SerializeField] float _speed;
    Vector3 _dir = Vector3.right;
    public override void Start()
    {
        base.Start();
        _armRotation = new Movement_RotateObject(_armPivot, _playerCenterPivot, _armPivot);

        IDLE = new State<ShootingRobot>("IDLE");
        var SHOOT = new State<ShootingRobot>("SHOOT");

        StateConfigurer.Create(IDLE).SetTransition(ShootingRobot.Shoot, SHOOT).Done();
        StateConfigurer.Create(SHOOT).SetTransition(ShootingRobot.Idle, IDLE).Done();

        float idleTimer = 0;
        IDLE.OnEnter += x => AgroSign(false);
        IDLE.OnUpdate += delegate
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 3)
            {
                _armPivot.DOLocalRotate(new Vector3(0, 0, Random.Range(0, -180)), .1f).SetEase(Ease.Linear);
                idleTimer = 0;
            }

            if (CanSeePlayer()) _myFSM.SendInput(ShootingRobot.Shoot);
        };

        SHOOT.OnEnter += x =>
        {
            AgroSign(true);
            transform.DOScale(new Vector2(1.25f, 1.25f), .1f).SetLoops(2, LoopType.Yoyo);
        };
        SHOOT.OnUpdate += delegate
        {
            if (!CanSeePlayer()) _myFSM.SendInput(ShootingRobot.Idle);

            CalculateAttack();

            _armRotation.Move();
        };

        if (Helpers.LevelTimerManager.LevelStarted) _myFSM = new EventFSM<ShootingRobot>(IDLE);
        else EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
    }
    void StartFSM(params object[] param) { _myFSM = new EventFSM<ShootingRobot>(IDLE); }

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
            _dir *= -1;
    }
}
