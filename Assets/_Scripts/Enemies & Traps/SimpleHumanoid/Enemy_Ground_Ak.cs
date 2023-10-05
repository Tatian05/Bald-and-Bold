using UnityEngine;
using DG.Tweening;
public class Enemy_Ground_Ak : Enemy
{
    [SerializeField] GameObject _agroSign;
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _armPivot, _gunTransform;
    [SerializeField] float _bulletDamage = 1f, _bulletSpeed = 10f, _attackSpeed = 2f;
    [SerializeField] float _viewRadius, _viewAngle;
    [SerializeField] float _speed = 1f;
    [SerializeField] int _bulletsAmountToShoot = 3;
    [SerializeField] float _bulletsDelay = .05f;
    float _currentAttackSpeed;
    enum AK_States { Patrol, Attack }
    EventFSM<AK_States> _myFSM;
    State<AK_States> PATROL;
    Vector3 _dir = Vector3.right;
    Tween _currentTween;
    public override void Start()
    {
        base.Start();
        LayerMask invisibleWall = gameManager.InvisibleWallLayer;

        PATROL = new State<AK_States>("PATROL");
        var ATTACK = new State<AK_States>("ATTACK");

        StateConfigurer.Create(PATROL).SetTransition(AK_States.Attack, ATTACK).Done();
        StateConfigurer.Create(ATTACK).SetTransition(AK_States.Patrol, PATROL).Done();

        System.Action<bool> agroSign = x => _agroSign.SetActive(x);

        #region PATROL

        PATROL.OnEnter += x => OnPatrolStart();
        PATROL.OnUpdate += () =>
        {
            transform.position += _dir * _speed * Time.deltaTime;
            if (Physics2D.Raycast(transform.position, transform.right, 1f, invisibleWall)) FlipEnemy();
            if (GetCanSeePlayer())
                _myFSM.SendInput(AK_States.Attack);
        };

        #endregion

        #region ATTACK

        ATTACK.OnEnter += x =>
        {
            OnAttackStart();
            //Si no lo seteas a uno se rompe el brazo y apunta para atras por como esta seteado el flip del patrol
            transform.localScale = Vector3.one;
            agroSign(true);
        };

        ATTACK.OnUpdate += delegate
        {
            OnAttack();

            if (!GetCanSeePlayer())
                _myFSM.SendInput(AK_States.Patrol);
        };

        ATTACK.OnExit += x => agroSign(false);

        #endregion

        if (Helpers.LevelTimerManager.LevelStarted) _myFSM = new EventFSM<AK_States>(PATROL);
        else EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
    }
    void StartFSM(params object[] param) { _myFSM = new EventFSM<AK_States>(PATROL); }
    protected override void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
        base.OnDestroy();
    }
    public override void Update()
    {
        _myFSM?.Update();
    }
    bool GetCanSeePlayer()
    {
        if (DistanceToPlayer().magnitude > _viewRadius) return default;
        if (Vector3.Angle(transform.right, DistanceToPlayer().normalized) <= _viewAngle / 2)
            return CanSeePlayer();

        return default;
    }
    void FlipEnemy()
    {
        _dir *= -1;
        float angle = transform.eulerAngles.y == 0 ? 180 : 0;

        transform.eulerAngles = new Vector3(0, angle, 0);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 1);
    }

    void OnPatrolStart()
    {
        anim.SetBool("IsRunning", true);
        _armPivot.eulerAngles = transform.eulerAngles;
    }

    void OnAttackStart()
    {
        anim.SetBool("IsRunning", false);
    }
    void OnAttack()
    {
        LookAtPlayer();

        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            InvokeRepeating(nameof(Shoot), 0, _bulletsDelay);
            _currentAttackSpeed = 0;
        }
    }

    int _currentBullets = 0;
    void Shoot()
    {
        FRY_EnemyBullet.Instance.pool.GetObject().SetPosition(_bulletSpawnPosition.position)
                                            .SetDirection(_armPivot.right)
                                            .SetDmg(_bulletDamage)
                                            .SetSpeed(_bulletSpeed);

        DOTween.Rewind(_gunTransform);
        float recoilBack = .1f * 3f;
        _gunTransform.DOLocalMove(-Vector2.right * .2f, .1f).OnComplete(() => _gunTransform.DOLocalMove(Vector3.zero, recoilBack));

        if (_currentBullets < _bulletsAmountToShoot - 1) _currentBullets++;
        else
        {
            CancelInvoke(nameof(Shoot));
            _currentBullets = 0;
        }
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
        _myFSM.SendInput(AK_States.Patrol);
        base.ReturnObject();
        FRY_Enemy_Ground_Ak.Instance.pool.ReturnObject(this);
    }
}
