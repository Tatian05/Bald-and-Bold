using UnityEngine;
using Weapons;
using DG.Tweening;
public class FireWeapon : Weapon
{
    [SerializeField, Range(0, 0.1f)] protected float _recoilForce;

    protected Transform _bulletSpawn;
    protected int _currentAmmo;
    protected Animator _muzzleFlashAnimator;
    LayerMask _borderMask;
    Tween _currentTween;
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
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, ResetRecoil);
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
        var z = transform.localEulerAngles.z;
        var euler = transform.localEulerAngles;
        if (!raycast)
        {
            FireWeaponShoot();

            if (_currentTween != null) _currentTween.Kill();
            DOTween.Rewind(transform);
            float recoilBack = _weaponData.recoilDuration * 3f;
            transform.DOLocalMove(-Vector2.right * _weaponData.recoilForce, _weaponData.recoilDuration).OnComplete(() => transform.DOLocalMove(Vector3.zero, recoilBack));
            transform.DOLocalRotate(new Vector3(0, 0, z + (_weaponData.recoilWeaponRot * transform.localScale.y)), _weaponData.recoilDuration).SetLoops(1, LoopType.Yoyo).SetEase(Ease.Linear).
            OnComplete(() => _currentTween = transform.DOLocalRotate(Vector2.zero, _weaponData.recoilWeaponRotDuration));
            _muzzleFlashAnimator.Play("MuzzleFlash");
        }
    }
    public void ResetRecoil(params object[] param)
    {
        DOTween.Restart(transform);
        transform.DOLocalRotate(Vector2.zero, _weaponData.recoilWeaponRotDuration);
    }
    protected virtual void FireWeaponShoot()
    {
        Helpers.AudioManager.PlaySFX(_weaponData.weaponSoundName);
        FRY_PlayerBullet.Instance.pool.GetObject().
                                            SetDmg(_weaponData.damage).
                                            SetSpeed(_weaponData.bulletSpeed).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(transform.right);
    }
}
