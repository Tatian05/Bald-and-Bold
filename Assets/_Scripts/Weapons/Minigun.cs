using UnityEngine;
using UnityEngine.InputSystem;
public class Minigun : FireWeapon
{
    [SerializeField] float _maxOverheating = 10;
    [SerializeField] float _minOverheating = 1;
    float _overheatingValue;

    float _minFireRate = 5;
    System.Action _overheating;
    [SerializeField] Material _minigunMaterial;
    protected override void Start()
    {
        base.Start();
        _weaponData.currentCadence = _minFireRate;
        _minigunMaterial = GetComponent<SpriteRenderer>().material;
    }
    private void Update()
    {
        _overheating?.Invoke();
    }
    public override void WeaponAction()
    {
        if (_overheating.Method.Name.Equals("LessOverheating") || _overheatingValue < _minOverheating) return;
        base.WeaponAction();
    }
    void AddOverheating()
    {
        if (_overheatingValue >= _maxOverheating)
        {
            _overheating = LessOverheating;
            return;
        }
        _weaponData.currentCadence += CustomTime.DeltaTime;
        if (_weaponData.currentCadence >= _weaponData.fireRate) _weaponData.currentCadence = _weaponData.fireRate;

        _overheatingValue += CustomTime.DeltaTime;
        _animator.SetFloat("Overheating", _overheatingValue);

        if (_overheatingValue >= _minOverheating)
            _minigunMaterial.SetFloat("_ChangeColor", (_overheatingValue - _minOverheating) / _maxOverheating);
    }
    void LessOverheating()
    {
        _weaponData.currentCadence -= CustomTime.DeltaTime;
        if (_weaponData.currentCadence <= _minFireRate) _weaponData.currentCadence = _minFireRate;

        _overheatingValue -= CustomTime.DeltaTime * 1.5f;
        _animator.SetFloat("Overheating", _overheatingValue);

        if (_overheatingValue >= _minOverheating)
            _minigunMaterial.SetFloat("_ChangeColor", (_overheatingValue - _minOverheating) / _maxOverheating);

        if (_overheatingValue <= 0) _overheating = null;
    }

    public override void OnStartShoot(InputAction.CallbackContext obj) { _overheating = AddOverheating; }
    public override void OnCanceledShoot(InputAction.CallbackContext obj) { _overheating = LessOverheating; }
}
