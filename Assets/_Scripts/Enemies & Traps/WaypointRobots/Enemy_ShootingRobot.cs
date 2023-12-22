using UnityEngine;
using DG.Tweening;
public enum ShootingRobot { Idle, Shoot, Lost }
public class Enemy_ShootingRobot : Enemy_Shooters
{
    IMovement _armRotation;
    float _currentAttackSpeed;
    EventFSM<ShootingRobot> _myFSM;
    State<ShootingRobot> IDLE;
    LayerMask _invisibleWallMask;

    [SerializeField] float _speed;
    Vector3 _dir = Vector3.right;
    public override void Start()
    {
        base.Start();
        _armRotation = new Movement_RotateObject(_armPivot, _playerPosition, _armPivot);
        _invisibleWallMask = gameManager.InvisibleWallLayer;
        IDLE = new State<ShootingRobot>("IDLE");
        var SHOOT = new State<ShootingRobot>("SHOOT");
        var LOST = new State<ShootingRobot>("LOST");

        StateConfigurer.Create(IDLE).SetTransition(ShootingRobot.Shoot, SHOOT).Done();
        StateConfigurer.Create(SHOOT).SetTransition(ShootingRobot.Idle, IDLE).SetTransition(ShootingRobot.Lost, LOST).Done();
        StateConfigurer.Create(LOST).SetTransition(ShootingRobot.Idle, IDLE).Done();

        #region IDLE

        float idleTimer = 0;
        IDLE.OnEnter += x => SetSign(false);
        IDLE.OnUpdate += delegate
        {
            idleTimer += CustomTime.DeltaTime;
            if (idleTimer >= 3)
            {
                _armPivot.DOLocalRotate(new Vector3(0, 0, Random.Range(0, -180)), .1f).SetEase(Ease.Linear);
                idleTimer = 0;
            }
            if (!_playerPosition) return;
            if (CanSeePlayer()) _myFSM.SendInput(ShootingRobot.Shoot);
        };

        #endregion

        #region SHOOT

        SHOOT.OnEnter += x =>
        {
            SetSign(true, _agroSign);
            transform.DOScale(new Vector2(1.25f * Mathf.Sign(transform.localScale.x), 1.25f * Mathf.Sign(transform.localScale.y)), .1f).SetLoops(2, LoopType.Yoyo);
        };
        SHOOT.OnUpdate += delegate
        {
            if (!CanSeePlayer()) _myFSM.SendInput(ShootingRobot.Idle);

            CalculateAttack();

            _armRotation.Move();
        };

        #endregion

        #region LOST

        float lostTimer = 0;
        LOST.OnEnter += x => SetSign(true, _lostSign);
        LOST.OnUpdate += delegate
        {
            lostTimer += CustomTime.DeltaTime;
            if (lostTimer >= _lostTime) _myFSM.SendInput(ShootingRobot.Idle);
        };
        LOST.OnExit += x => SetSign(false);

        #endregion

        if (!Helpers.LevelTimerManager || Helpers.LevelTimerManager.LevelStarted) _myFSM = new EventFSM<ShootingRobot>(IDLE);
        else EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
    }
    void StartFSM(params object[] param) { _myFSM = new EventFSM<ShootingRobot>(IDLE); }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
    void Update()
    {
        _myFSM?.Update();
        transform.position += _dir * _speed * CustomTime.DeltaTime;
        if (Physics2D.OverlapCircle(transform.position, .5f, _invisibleWallMask)) _dir *= -1;
    }
    protected override void PlayerInvisibleConsumable(params object[] param)
    {
        _myFSM.SendInput((bool)param[0] ? ShootingRobot.Lost : ShootingRobot.Idle);
    }
    void CalculateAttack()
    {
        _currentAttackSpeed += CustomTime.DeltaTime;

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
    public Enemy Flip(bool flip)
    {
        if (!flip) return this;
        transform.GetChild(0).transform.localScale = new Vector3(1, -1, 1);
        return this;
    }
    protected override void OnReset()
    {
        base.OnReset();

        _currentAttackSpeed = 0;
        _myFSM?.SendInput(ShootingRobot.Idle);
    }
    protected override void Reset()
    {
        base.Reset();

        _currentAttackSpeed = 0;
        _myFSM?.SendInput(ShootingRobot.Idle);
    }
    public override void ReturnObject()
    {
        FRY_Enemy_ShootingRobot.Instance.pool.ReturnObject(this);
        base.ReturnObject();
    }
}
