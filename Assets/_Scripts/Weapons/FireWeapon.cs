using UnityEngine;
using Weapons;
using DG.Tweening;
using UnityEngine.InputSystem;
public class FireWeapon : Weapon
{
    protected Transform _bulletSpawn;
    protected int _currentAmmo;
    protected Animator _muzzleFlashAnimator;
    LayerMask _borderMask;
    Tween _currentTween;
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
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, ResetRecoil);
    }
    public override void WeaponAction() { }
    public override void WeaponAction(float bulletScale, float cadenceBoost, bool recoil)
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
            FireWeaponShoot(bulletScale);
            if (recoil && _weaponData.recoilForce > 0) FireWeaponRecoil();
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
        transform.DOLocalMove(-Vector2.right * _weaponData.recoilForce, _weaponData.recoilDuration).OnComplete(() => transform.DOLocalMove(_equipedWeaponOffset, recoilBack));
        transform.DOLocalRotate(new Vector3(0, 0, z + (_weaponData.recoilWeaponRot * transform.localScale.y)), _weaponData.recoilDuration).SetLoops(1, LoopType.Yoyo).SetEase(Ease.Linear).
        OnComplete(() => _currentTween = transform.DOLocalRotate(Vector2.zero, _weaponData.recoilWeaponRotDuration));
    }
    protected virtual void FireWeaponShoot(float bulletScale)
    {
        Helpers.AudioManager.PlaySFX(_weaponData.weaponSoundName);
        FRY_PlayerBullet.Instance.pool.GetObject().
                                            SetDmg(_weaponData.damage).
                                            SetSpeed(_weaponData.bulletSpeed).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(transform.right).
                                            SetScale(bulletScale);
    }
    public virtual void OnCanceledShoot(InputAction.CallbackContext obj) { }
    public virtual void OnStartShoot(InputAction.CallbackContext obj) { }
}
