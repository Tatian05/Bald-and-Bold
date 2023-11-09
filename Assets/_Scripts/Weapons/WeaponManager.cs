using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;
public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon _currentMainWeapon, _currentSecundaryWeapon;
    [SerializeField] GameObject _rightHand, _leftHand;

    Transform _mainWeaponContainer, _secundaryWeaponContainer;
    Transform _secundaryWeaponTransform;
    PlayerInputs _playerInputs;
    InputAction _knife, _interact, _shoot;
    Action _lookAtMouse = delegate { };
    Camera _mainCamera;
    bool _onWeaponTrigger;
    Func<Vector2> _cursorPosition;
    [SerializeField] Weapon _weaponBeforeMinigun;
    private void Start()
    {
        _mainCamera = Helpers.MainCamera;
        _mainWeaponContainer = transform.GetChild(0);
        _secundaryWeaponContainer = transform.GetChild(1);
        _currentSecundaryWeapon = _secundaryWeaponContainer.GetComponentInChildren<Weapon>();
        _secundaryWeaponTransform = _currentSecundaryWeapon.transform;
        _currentSecundaryWeapon.PickUp(true);
        _playerInputs = NewInputManager.PlayerInputs;
        _lookAtMouse += SecundaryWeapon;
        _knife = _playerInputs.Player.Knife;
        _interact = _playerInputs.Player.Interact;
        _shoot = _playerInputs.Player.Shoot;

        _interact.performed += OnInteract;

        _knife.Enable();
        _interact.Enable();
        _shoot.Enable();

        //OnControlChanged();
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OnControlChanged;
        EventManager.SubscribeToEvent(Contains.PAUSE_GAME, PauseActions);
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_MINIGUN, MinigunConsumable);
    }
    private void Update()
    {
        _lookAtMouse?.Invoke();
        if (_shoot.ReadValue<float>() > .1f && _currentMainWeapon) _currentMainWeapon.Attack();
        if (_knife.ReadValue<float>() > .1f && _currentSecundaryWeapon) _currentSecundaryWeapon.Attack();
    }
    void OnInteract(InputAction.CallbackContext obj) { if (_onWeaponTrigger) SetWeapon(); }

    Minigun _minigun;
    void MinigunConsumable(params object[] param)
    {
        if ((bool)param[0])
        {
            _minigun = Instantiate((Minigun)param[1]);
            _weaponBeforeMinigun = _currentMainWeapon;
            SetWeapon(true, _minigun);
        }
        else
        {
            SetWeapon(false, _weaponBeforeMinigun);
            Destroy(_minigun.gameObject);
        }
    }

    #region Weapon Funcs
    public void SetWeapon(bool minigun = false, Weapon newWeapon = null)
    {
        if (_currentMainWeapon)
            ThrowWeapon(minigun);

        if (newWeapon == null)
        {
            var col = Physics2D.OverlapCircle(transform.position, 2f, Helpers.GameManager.WeaponLayer);
            newWeapon = col ? col.GetComponent<Weapon>() : null;
        }

        if (newWeapon == null) return;

        //if (!newWeapon.CanPickUp) return;

        EquipWeapon(newWeapon);
    }
    void EquipWeapon(Weapon newWeapon)
    {
        _currentMainWeapon = newWeapon;
        _mainWeaponContainer.localEulerAngles = transform.eulerAngles;
        _lookAtMouse += MainWeapon;
        _currentMainWeapon.PickUp().SetParent(_mainWeaponContainer).SetPosition(_mainWeaponContainer.position);
        _rightHand.SetActive(false);
        _leftHand.SetActive(false);

        _shoot.performed += (_currentMainWeapon as FireWeapon).OnStartShoot;
        _shoot.canceled += (_currentMainWeapon as FireWeapon).OnCanceledShoot;
    }
    private void ThrowWeapon(bool minigun)
    {
        //RaycastHit2D raycast = Physics2D.Raycast(_mainWeaponContainer.position, GetMousePosition(), 1f, LayerMask.GetMask("Border"));
        //if (raycast)
        //{
        //    _currentMainWeapon.transform.position += (Vector3)raycast.normal / 2;
        //    _currentMainWeapon?.ThrowOut(raycast.normal).SetParent(null);
        //}
        //else
        //    _currentMainWeapon?.ThrowOut(GetMouseDirectionMain()).SetParent(null);

        _shoot.performed -= (_currentMainWeapon as FireWeapon).OnStartShoot;
        _shoot.canceled -= (_currentMainWeapon as FireWeapon).OnCanceledShoot;
        _lookAtMouse -= MainWeapon;

        _weaponBeforeMinigun?.gameObject.SetActive(!minigun);
        _currentMainWeapon = null;
        _mainWeaponContainer.eulerAngles = Vector2.zero;
        _rightHand.SetActive(true);
        _leftHand.SetActive(true);
    }

    Vector2 primaryWeaponRotation;
    void MainWeapon()
    {
        if (Vector3.Dot(transform.right, _cursorPosition()) < Mathf.Cos(90) * Mathf.Deg2Rad || _cursorPosition() == Vector2.zero) return;
        _mainWeaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());

        primaryWeaponRotation = new Vector2(_currentMainWeapon.transform.localScale.x, Mathf.Sign(_cursorPosition().x));
        _currentMainWeapon.transform.localScale = primaryWeaponRotation;
    }

    Vector2 secondaryWeaponSize;
    void SecundaryWeapon()
    {
        if (Vector3.Dot(transform.right, _cursorPosition()) < Mathf.Cos(90) * Mathf.Deg2Rad) return;

        _secundaryWeaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());
        secondaryWeaponSize = new Vector2(_secundaryWeaponTransform.localScale.x, Mathf.Sign(_cursorPosition().x));
        _secundaryWeaponTransform.localScale = secondaryWeaponSize;
    }

    #endregion

    #region Mouse Funcs
    public float GetAngle() => Mathf.Atan2(_cursorPosition().y, _cursorPosition().x) * Mathf.Rad2Deg;
    public Vector2 GetMousePosition() => _mainCamera.ScreenToWorldPoint(new Vector3(MouseCursorPosition().x, MouseCursorPosition().y, _mainCamera.nearClipPlane));
    Vector2 GetMouseDirectionMain() => (GetMousePosition() - (Vector2)_mainWeaponContainer.position).normalized;
    Vector2 GetMouseDirectionSecundary() => (GetMousePosition() - (Vector2)_secundaryWeaponContainer.position).normalized;

    #endregion
    Vector2 GamepadCursorPosition() => Gamepad.current.rightStick.ReadValue();
    Vector2 MouseCursorPosition() => Mouse.current.position.ReadValue();

    void OnControlChanged()
    {
        _cursorPosition = NewInputManager.activeDevice == DeviceType.Keyboard ? (Func<Vector2>)GetMouseDirectionMain : (Func<Vector2>)GamepadCursorPosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>()) _onWeaponTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>()) _onWeaponTrigger = false;
    }
    void PauseActions(params object[] param)
    {
        if ((bool)param[0])
        {
            _interact.Disable();
            _knife.Disable();
            _shoot.Disable();
        }
        else
        {
            _interact.Enable();
            _knife.Enable();
            _shoot.Enable();
        }
    }
    private void OnDestroy()
    {
        _lookAtMouse -= SecundaryWeapon;
        _lookAtMouse -= MainWeapon;

        _interact.performed -= OnInteract;

        if (_currentMainWeapon)
        {
            _shoot.performed -= (_currentMainWeapon as FireWeapon).OnStartShoot;
            _shoot.canceled -= (_currentMainWeapon as FireWeapon).OnCanceledShoot;
        }

        _knife.Disable();
        _interact.Disable();
        _shoot.Disable();

        NewInputManager.ActiveDeviceChangeEvent -= OnControlChanged;
        EventManager.UnSubscribeToEvent(Contains.PAUSE_GAME, PauseActions);
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_MINIGUN, MinigunConsumable);
    }
}
