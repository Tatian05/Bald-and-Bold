using UnityEngine;
using DG.Tweening;
public class Enemy_Sniper : Enemy_Shooters
{
    [SerializeField] Vector2 _laserSpeed;
    [SerializeField] LineRenderer _sniperLaser;
    enum SniperStates { Idle, LoadShoot, Shoot, Lost };
    enum LaserStates { Off, On };
    EventFSM<SniperStates> _myFSM;
    EventFSM<LaserStates> _laserFSM;
    Color[] _rayColors = new Color[3] { Color.green, Color.yellow, Color.red };
    State<SniperStates> IDLE;
    LayerMask _laserMask;
    Vector2 _weaponStartPos;
    public override void Start()
    {
        base.Start();
        LayerMask borderMask = gameManager.BorderLayer;
        _laserMask = gameManager.EnemyBulletLayer;
        _sniperLaser = GetComponent<LineRenderer>();
        _weaponStartPos = _armPivot.localPosition;
        var sniperMat = _sniperLaser.material;
        sniperMat.SetVector("LaserSpeed", _laserSpeed);

        #region LASER

        var on = new State<LaserStates>("ON");
        var off = new State<LaserStates>("OFF");

        StateConfigurer.Create(on).SetTransition(LaserStates.Off, off).Done();
        StateConfigurer.Create(off).SetTransition(LaserStates.On, on).Done();

        on.OnEnter += x => _sniperLaser.positionCount = 2;
        on.OnUpdate += delegate
        {
            _sniperLaser.SetPosition(0, _bulletSpawnPosition.position);
            _sniperLaser.SetPosition(1, _ray.point);

            if (Physics2D.OverlapCircle(_bulletSpawnPosition.position, .1f, borderMask)) _laserFSM.SendInput(LaserStates.Off);
        };

        off.OnEnter += x => _sniperLaser.positionCount = 0;
        off.OnUpdate += () => { if (!Physics2D.OverlapCircle(_bulletSpawnPosition.position, .1f, borderMask)) _laserFSM.SendInput(LaserStates.On); };

        _laserFSM = new EventFSM<LaserStates>(off);

        #endregion

        #region SNIPER_ENEMY

        IDLE = new State<SniperStates>("IDLE");
        var LOAD_SHOOT = new State<SniperStates>("LOAD_SHOOT");
        var SHOOT = new State<SniperStates>("SHOOT");
        var LOST = new State<SniperStates>("LOST");

        StateConfigurer.Create(IDLE).SetTransition(SniperStates.LoadShoot, LOAD_SHOOT).Done();
        StateConfigurer.Create(LOAD_SHOOT).SetTransition(SniperStates.Shoot, SHOOT).
                                           SetTransition(SniperStates.Lost, LOST).
                                           SetTransition(SniperStates.Idle, IDLE).Done();
        StateConfigurer.Create(SHOOT).SetTransition(SniperStates.LoadShoot, LOAD_SHOOT).SetTransition(SniperStates.Lost, LOST).Done();
        StateConfigurer.Create(LOST).SetTransition(SniperStates.Idle, IDLE).Done();

        sniperMat.SetColor("MainColor", _rayColors[0] * 5);


        #region IDLE 

        IDLE.OnEnter += x => SetSign(false);
        IDLE.OnUpdate += delegate
        {
            if (!_enemyDataSO.playerPivot) return;
            if (CanSeePlayer()) _myFSM.SendInput(SniperStates.LoadShoot);
        };

        #endregion

        #region LOAD_SHOOT

        LOAD_SHOOT.OnEnter += x => SetSign(true, _agroSign);
        float loadingAmmoTimer = 0;
        LOAD_SHOOT.OnUpdate += delegate
        {
            sniperMat.SetColor("MainColor", MultiLerp(loadingAmmoTimer / _attackSpeed, _rayColors) * 5);
            loadingAmmoTimer += Time.deltaTime;
            WeaponRot();
            LookAtPlayer();
            if (!CanSeePlayer()) _myFSM.SendInput(SniperStates.Idle);
            if (loadingAmmoTimer >= _attackSpeed)
            {
                loadingAmmoTimer = 0;
                _myFSM.SendInput(SniperStates.Shoot);
            }
        };

        #endregion

        #region SHOOT 

        SHOOT.OnEnter += x =>
        {
            Shoot();
            sniperMat.SetColor("MainColor", _rayColors[0] * 5);
            _armPivot.DOLocalMove(-Vector2.right * .2f, .1f).OnComplete(() => _armPivot.DOLocalMove(_weaponStartPos, .3f));
            _armPivot.DOLocalRotate(new Vector3(0, 0, _armPivot.localEulerAngles.z + 20), .1f).SetEase(Ease.Linear).
            OnComplete(() =>
            {
                anim.Play("LoadAmmo");
                _armPivot.DOLocalRotate(Vector2.zero, 1f).OnComplete(() => _myFSM.SendInput(SniperStates.LoadShoot));
            });
        };

        #endregion

        #region LOST

        float lostTimer = 0;
        LOST.OnEnter += x => SetSign(true, _lostSign);
        LOST.OnUpdate += delegate
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= _enemyDataSO.lostTime) _myFSM.SendInput(SniperStates.Idle);
        };
        LOST.OnExit += x => SetSign(false);

        #endregion

        if (Helpers.LevelTimerManager.LevelStarted) _myFSM = new EventFSM<SniperStates>(IDLE);
        else EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);

        #endregion
    }
    void StartFSM(params object[] param) { _myFSM = new EventFSM<SniperStates>(IDLE); }
    protected override void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
        base.OnDestroy();
    }
    RaycastHit2D _ray;
    void Update()
    {
        _myFSM?.Update();
        _laserFSM?.Update();

        _ray = Physics2D.Raycast(_bulletSpawnPosition.position, _bulletSpawnPosition.right, 100, _laserMask);
    }
    protected override void Shoot()
    {
        FRY_EnemyBullet.Instance.pool.GetObject().SetPosition(_bulletSpawnPosition.position)
                                            .SetDirection(_armPivot.right)
                                            .SetDmg(_bulletDamage)
                                            .SetSpeed(_bulletSpeed);
    }
    protected override void PlayerInvisibleConsumable(params object[] param)
    {
        base.PlayerInvisibleConsumable(param);
        _myFSM.SendInput((bool)param[0] ? SniperStates.Lost : SniperStates.Idle);
    }
    public override void ReturnObject()
    {
        _myFSM.SendInput(SniperStates.Idle);
        base.ReturnObject();
        FRY_Enemy_Sniper.Instance.pool.ReturnObject(this);
    }
    Color MultiLerp(float time, Color[] points)
    {
        if (points.Length == 1)
            return points[0];
        else if (points.Length == 2)
            return Color.Lerp(points[0], points[1], time);

        if (time == 0)
            return points[0];

        if (time == 1)
            return points[points.Length - 1];

        float t = time * (points.Length - 1);

        Color pointA = Color.white;
        Color pointB = Color.white;

        for (int i = 0; i < points.Length; i++)
        {
            if (t < i)
            {
                pointA = points[i - 1];
                pointB = points[i];
                return Color.Lerp(pointA, pointB, t - (i - 1));
            }
            else if (t == (float)i)
                return points[i];
        }
        return Color.white;
    }
}