using UnityEngine;
using DG.Tweening;
public class Enemy_Sniper : Enemy
{
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _armPivot;
    [SerializeField] float _bulletDamage = 1f, _bulletSpeed = 10f, _attackSpeed = 2f;
    [SerializeField] Vector2 _laserSpeed;
    [SerializeField] LineRenderer _sniperLaser;
    enum SniperStates { Idle, LoadShoot, Shoot };
    EventFSM<SniperStates> _myFSM;
    Color[] _rayColors = new Color[3] { Color.green, Color.yellow, Color.red };
    State<SniperStates> IDLE;
    public override void Start()
    {
        base.Start();
        _sniperLaser = GetComponent<LineRenderer>();
        var sniperMat = _sniperLaser.material;
        sniperMat.SetVector("LaserSpeed", _laserSpeed);

        IDLE = new State<SniperStates>("IDLE");
        var LOAD_SHOOT = new State<SniperStates>("LOAD_SHOOT");
        var SHOOT = new State<SniperStates>("SHOOT");

        StateConfigurer.Create(IDLE).SetTransition(SniperStates.LoadShoot, LOAD_SHOOT).Done();
        StateConfigurer.Create(LOAD_SHOOT).SetTransition(SniperStates.Shoot, SHOOT).SetTransition(SniperStates.Idle, IDLE).Done();
        StateConfigurer.Create(SHOOT).SetTransition(SniperStates.LoadShoot, LOAD_SHOOT).Done();

        _sniperLaser.positionCount = 2;
        _sniperLaser.SetPosition(0, _bulletSpawnPosition.position);
        _sniperLaser.SetPosition(1, _bulletSpawnPosition.position + _bulletSpawnPosition.right * 100);
        sniperMat.SetColor("MainColor", _rayColors[0] * 5);

        #region IDLE 

        IDLE.OnUpdate += delegate { if (CanSeePlayer()) _myFSM.SendInput(SniperStates.LoadShoot); };

        #endregion

        #region LOAD_SHOOT

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
            _armPivot.DOLocalRotate(new Vector3(0, 0, _armPivot.localEulerAngles.z + 20), .1f).
            OnComplete(() =>
            {
                anim.Play("LoadAmmo");
                _armPivot.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.Linear).OnComplete(() => _myFSM.SendInput(SniperStates.LoadShoot));
            });
        };

        #endregion

        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
    }
    void StartFSM(params object[] param) { _myFSM = new EventFSM<SniperStates>(IDLE); }
    protected override void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
        base.OnDestroy();
    }
    RaycastHit2D _ray;
    public override void Update()
    {
        _myFSM.Update();

        _ray = Physics2D.Raycast(_bulletSpawnPosition.position, _bulletSpawnPosition.right, 100);
        _sniperLaser.SetPosition(0, _bulletSpawnPosition.position);
        _sniperLaser.SetPosition(1, _ray.point);
    }

    Vector3 _dir;
    float _r, _angle, _smoothAngle;
    void WeaponRot()
    {
        _dir = (_playerCenterPivot.position - _armPivot.position).normalized;
        _angle = CanSeePlayer() ? Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg : 0;
        _smoothAngle = Mathf.SmoothDampAngle(_armPivot.eulerAngles.z, _angle, ref _r, .1f);
        _armPivot.eulerAngles = new Vector3(0, 0, _smoothAngle);
    }

    Vector3 _dirToLookAt;
    void LookAtPlayer()
    {
        _dirToLookAt = (_playerCenterPivot.position - transform.position).normalized;
        float angle = Mathf.Atan2(_dirToLookAt.y, Mathf.Abs(_dirToLookAt.x)) * Mathf.Rad2Deg;

        Vector3 newScale = Vector3.one;

        if (angle > 90 || angle < -90) newScale.x = -1;
        else newScale.x = 1;

        sprite.localScale = newScale;
    }
    void Shoot()
    {
        FRY_EnemyBullet.Instance.pool.GetObject().SetPosition(_bulletSpawnPosition.position)
                                            .SetDirection(_armPivot.right)
                                            .SetDmg(_bulletDamage)
                                            .SetSpeed(_bulletSpeed);
    }

    public override void ReturnObject()
    {
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