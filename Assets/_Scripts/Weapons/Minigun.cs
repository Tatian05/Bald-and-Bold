using UnityEngine;
using UnityEngine.InputSystem;
public class Minigun : FireWeapon
{
    [SerializeField] float _maxOverheating = 10;
    float _overheatingValue;
    System.Action _overheating;
    private void Update()
    {
        _overheating?.Invoke();
    }
    public override void WeaponAction()
    {
        if (_overheatingValue >= _maxOverheating || _overheating.Method.Name.Equals("LessOverheating")) return;
        base.WeaponAction();
    }
    void AddOverheating()
    {
        if (_overheatingValue >= _maxOverheating)
        {
            _overheating = LessOverheating;
            return;
        }

        _overheatingValue += CustomTime.DeltaTime;
        _animator.SetFloat("Overheating", _overheatingValue);
    }
    void LessOverheating()
    {
        _overheatingValue -= CustomTime.DeltaTime * 1.5f;
        _animator.SetFloat("Overheating", _overheatingValue);
        if (_overheatingValue <= 0) _overheating = null;
    }

    public override void OnStartShoot(InputAction.CallbackContext obj) { _overheating = AddOverheating; }
    public override void OnCanceledShoot(InputAction.CallbackContext obj) { _overheating = LessOverheating; }
}
