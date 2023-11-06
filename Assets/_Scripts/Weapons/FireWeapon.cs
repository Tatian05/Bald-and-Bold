using UnityEngine;
using Weapons;
using DG.Tweening;
using System;
public class FireWeapon : Weapon
{
    [SerializeField, Range(0, 0.1f)] protected float _recoilForce;

    protected Transform _bulletSpawn;
    protected int _currentAmmo;
    protected float _bulletScale = 1;
    protected Animator _muzzleFlashAnimator;
    LayerMask _borderMask;
    Tween _currentTween;
    event Action _recoilAction = delegate { };
    public int GetCurrentAmmo { get { return _currentAmmo; } set { _currentAmmo = value; } }
    protected override void Awake()
    {
        base.Awake();
        _borderMask = LayerMask.GetMask("Border");
        //_currentAmmo = _weaponData.initialAmmo;
    }
    protected override void Start()
    {
        base.Start();
        _bulletSpawn = transform.GetChild(0);
        _muzzleFlashAnimator = _bulletSpawn.GetChild(0).GetComponent<Animator>();
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, ResetRecoil);
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_NO_RECOIL, NoRecoilConsumable);
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_BIG_BULLET, BigBulletConsumable);
        _recoilAction = FireWeaponRecoil;
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, ResetRecoil);
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_NO_RECOIL, NoRecoilConsumable);
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_BIG_BULLET, BigBulletConsumable);
    }
    public override void WeaponAction()
    {
        //if (_currentAmmo <= 0)
        //{
        //    Helpers.AudioManager.PlaySFX("Not_Ammo");
        //    return;
        //}

        Helpers.LevelTimerManager.StartLevelTimer();
        //_currentAmmo--;
        //UpdateAmmoAmount();
        bool raycast = Physics2D.OverlapCircle(_bulletSpawn.position, .1f, _borderMask);
        
        if (!raycast)
        {
            FireWeaponShoot();
            _recoilAction();
            _muzzleFlashAnimator.Play("MuzzleFlash");
        }
    }
    void ResetRecoil(params object[] param)
    {
        DOTween.Restart(transform);
        transform.DOLocalRotate(Vector2.zero, _weaponData.recoilWeaponRotDuration);
    }
    void FireWeaponRecoil()
    {
        var z = transform.localEulerAngles.z;

        if (_currentTween != null) _currentTween.Kill();
        DOTween.Rewind(transform);
        float recoilBack = _weaponData.recoilDuration * 3f;
        transform.DOLocalMove(-Vector2.right * _weaponData.recoilForce, _weaponData.recoilDuration).OnComplete(() => transform.DOLocalMove(Vector3.zero, recoilBack));
        transform.DOLocalRotate(new Vector3(0, 0, z + (_weaponData.recoilWeaponRot * transform.localScale.y)), _weaponData.recoilDuration).SetLoops(1, LoopType.Yoyo).SetEase(Ease.Linear).
        OnComplete(() => _currentTween = transform.DOLocalRotate(Vector2.zero, _weaponData.recoilWeaponRotDuration));
    }
    void NoRecoilConsumable(params object[] param) => _recoilAction = (bool)param[0] ? (Action)delegate { } : (Action)FireWeaponRecoil;
    void BigBulletConsumable(params object[] param) => _bulletScale = (bool)param[0] ? (float)param[1] : 1;
    protected virtual void FireWeaponShoot()
    {
        Helpers.AudioManager.PlaySFX(_weaponData.weaponSoundName);
        FRY_PlayerBullet.Instance.pool.GetObject().
                                            SetDmg(_weaponData.damage).
                                            SetSpeed(_weaponData.bulletSpeed).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(transform.right).
                                            SetScale(_bulletScale);
    }
}
