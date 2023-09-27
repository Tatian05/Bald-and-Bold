using UnityEngine;
using DG.Tweening;
public class Enemy_Sniper : Enemy
{
    IMovement _armRotation;

    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _armPivot;
    [SerializeField] Transform _weaponSprite;
    [SerializeField] float _bulletDamage = 1f, _bulletSpeed = 10f, _attackSpeed = 2f;
    [SerializeField] LineRenderer _sniperRay;
    enum SniperStates { Idle, LoadShoot, Shoot };
    EventFSM<SniperStates> _myFSM;
    Color[] _rayColors = new Color[3] { Color.green, Color.yellow, Color.red };
    public override void Start()
    {
        base.Start();
        _armRotation = new Movement_RotateObject(_armPivot, _playerCenterPivot, _weaponSprite, sprite);
        _sniperRay = GetComponent<LineRenderer>();

        var IDLE = new State<SniperStates>("IDLE");
        var LOAD_SHOOT = new State<SniperStates>("LOAD_SHOOT");
        var SHOOT = new State<SniperStates>("SHOOT");

        StateConfigurer.Create(IDLE).SetTransition(SniperStates.LoadShoot, LOAD_SHOOT).Done();
        StateConfigurer.Create(LOAD_SHOOT).SetTransition(SniperStates.Shoot, SHOOT).SetTransition(SniperStates.Idle, IDLE).Done();
        StateConfigurer.Create(SHOOT).SetTransition(SniperStates.LoadShoot, LOAD_SHOOT).Done();

        _sniperRay.positionCount = 2;
        _sniperRay.SetPosition(0, _bulletSpawnPosition.position);
        _sniperRay.SetPosition(1, _bulletSpawnPosition.position + _bulletSpawnPosition.right * 100);

        #region IDLE 

        IDLE.OnEnter += x => { _sniperRay.startColor = _rayColors[0]; _sniperRay.endColor = _rayColors[0]; };
        IDLE.OnUpdate += delegate { if (CanSeePlayer()) _myFSM.SendInput(SniperStates.LoadShoot); };

        #endregion

        #region LOAD_SHOOT

        float loadingAmmoTimer = 0;
        LOAD_SHOOT.OnUpdate += delegate
        {
            _sniperRay.startColor = MultiLerp(loadingAmmoTimer / _attackSpeed, _rayColors);
            _sniperRay.endColor = MultiLerp(loadingAmmoTimer / _attackSpeed, _rayColors);
            loadingAmmoTimer += Time.deltaTime;
            _armRotation.Move();
            LookAtPlayer();
            //HACER LASER
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
            _sniperRay.startColor = _rayColors[0];
            _sniperRay.endColor = _rayColors[0];
            _armPivot.DOLocalRotate(new Vector3(0, 0, _armPivot.localEulerAngles.z + 20), .1f).
            OnComplete(() =>
            {
                anim.Play("LoadAmmo");
                _armPivot.DOLocalRotate(Vector3.zero, 2.5f).OnComplete(() => _myFSM.SendInput(SniperStates.LoadShoot)); ;
            });
        };

        #endregion

        _myFSM = new EventFSM<SniperStates>(IDLE);
    }
    public override void Update()
    {
        _myFSM.Update();

        RaycastHit2D ray = Physics2D.Raycast(_bulletSpawnPosition.position, _bulletSpawnPosition.right, Mathf.Infinity);
        _sniperRay.SetPosition(0, _bulletSpawnPosition.position);
        _sniperRay.SetPosition(1, ray.point);
    }
    void LookAtPlayer()
    {
        Vector3 dirToLookAt = (_playerCenterPivot.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, Mathf.Abs(dirToLookAt.x)) * Mathf.Rad2Deg;

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