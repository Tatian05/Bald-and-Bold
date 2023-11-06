using UnityEngine;
using System;
using DG.Tweening;
public class Enemy_Ground_Ak : Enemy_Shooters
{
    [SerializeField] float _viewRadius, _viewAngle;
    [SerializeField] float _speed = 1f;
    [SerializeField] int _bulletsAmountToShoot = 3;
    [SerializeField] float _bulletsDelay = .05f;
    float _currentAttackSpeed;
    enum AK_States { Patrol, Attack, Lost }
    EventFSM<AK_States> _myFSM;
    State<AK_States> PATROL;
    Vector3 _dir = Vector3.right;
    Tween _currentTween;
    public override void Start()
    {
        base.Start();

        PATROL = new State<AK_States>("PATROL");
        var ATTACK = new State<AK_States>("ATTACK");
        var LOST = new State<AK_States>("LOST");

        StateConfigurer.Create(PATROL).SetTransition(AK_States.Attack, ATTACK).Done();
        StateConfigurer.Create(ATTACK).SetTransition(AK_States.Patrol, PATROL).SetTransition(AK_States.Lost, LOST).Done();
        StateConfigurer.Create(LOST).SetTransition(AK_States.Patrol, PATROL).Done();

        Vector3 actualRot = Vector3.zero;
        Action OnPatrolStart = () =>
        {
            anim.SetBool("IsRunning", true);
            _armPivot.eulerAngles = transform.eulerAngles;
            transform.eulerAngles = actualRot;
        };

        #region PATROL

        PATROL.OnEnter += x => OnPatrolStart();
        PATROL.OnUpdate += () =>
        {
            transform.position += _dir * _speed * Time.deltaTime;
            if (Physics2D.Raycast(transform.position, transform.right, 1f, _groundLayer)) FlipEnemy();
            if (GetCanSeePlayer())
                _myFSM.SendInput(AK_States.Attack);
        };
        PATROL.OnExit += x => actualRot = transform.eulerAngles;

        #endregion

        #region ATTACK

        ATTACK.OnEnter += x =>
        {
            anim.SetBool("IsRunning", false);
            //Si no lo seteas a uno se rompe el brazo y apunta para atras por como esta seteado el flip del patrol
            transform.localScale = Vector3.one;
            SetSign(true, _agroSign);
        };

        ATTACK.OnUpdate += delegate
        {
            OnAttack();
            LookAtPlayer();
            if (!CanSeePlayer())
                _myFSM.SendInput(AK_States.Patrol);
        };

        ATTACK.OnExit += x => SetSign(false);

        #endregion

        #region LOST

        float lostTimer = 0;
        LOST.OnEnter += x => SetSign(true, _lostSign);
        LOST.OnUpdate += delegate
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= _enemyDataSO.lostTime) _myFSM.SendInput(AK_States.Patrol);
        };
        LOST.OnExit += x => SetSign(false);

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
    void Update()
    {
        _myFSM?.Update();
    }
    protected override void PlayerInvisibleConsumable(params object[] param)
    {
        base.PlayerInvisibleConsumable(param);
        _myFSM.SendInput((bool)param[0] ? AK_States.Lost : AK_States.Patrol);
    }
    bool GetCanSeePlayer()
    {
        if (DistanceToPlayer().magnitude > _viewRadius) return default;
        if (Vector3.Angle(transform.right, DistanceToPlayer().normalized) <= _viewAngle / 2)
            return CanSeePlayer();

        return default;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 1);
    }
    void OnAttack()
    {
        WeaponRot();

        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            InvokeRepeating(nameof(Shoot), 0, _bulletsDelay);
            _currentAttackSpeed = 0;
        }
    }

    int _currentBullets = 0;
    protected override void Shoot()
    {
        FRY_EnemyBullet.Instance.pool.GetObject().SetPosition(_bulletSpawnPosition.position)
                                            .SetDirection(_armPivot.right)
                                            .SetDmg(_bulletDamage)
                                            .SetSpeed(_bulletSpeed);

        DOTween.Rewind(_gunTransform);
        _gunTransform.DOLocalMove(-Vector2.right * .2f, .1f).OnComplete(() => _gunTransform.DOLocalMove(Vector3.zero, .3f));

        if (_currentBullets < _bulletsAmountToShoot - 1) _currentBullets++;
        else
        {
            CancelInvoke(nameof(Shoot));
            _currentBullets = 0;
        }
    }
    public override void ReturnObject()
    {
        _myFSM.SendInput(AK_States.Patrol);
        base.ReturnObject();
        FRY_Enemy_Ground_Ak.Instance.pool.ReturnObject(this);
    }
}
