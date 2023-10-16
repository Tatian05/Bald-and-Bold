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
    PlayerInput _playerInput;
    InputAction _knife, _interact, _shoot;
    Action _lookAtMouse = delegate { };
    Camera _mainCamera;
    bool _onWeaponTrigger;
    Func<Vector2> _cursorPosition;

    const string GAMEPAD_SCHEME = "Gamepad";
    const string KEYBOARD_MOUSE = "Keyboard&Mouse";
    string _previousControlScheme = string.Empty;
    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += OnControlChanged;
    }
    private void Start()
    {
        _mainCamera = Helpers.MainCamera;
        _mainWeaponContainer = transform.GetChild(0);
        _secundaryWeaponContainer = transform.GetChild(1);
        _currentSecundaryWeapon = _secundaryWeaponContainer.GetComponentInChildren<Weapon>();
        _secundaryWeaponTransform = _currentSecundaryWeapon.transform;
        _currentSecundaryWeapon.PickUp(true);
        _lookAtMouse += SecundaryWeapon;
        _playerInputs = Helpers.GameManager.PlayerInputs;
        _knife = _playerInputs.Player.Knife;
        _interact = _playerInputs.Player.Interact;
        _shoot = _playerInputs.Player.Shoot;

        _interact.performed += OnInteract;

        _knife.Enable();
        _interact.Enable();
        _shoot.Enable();
        EventManager.SubscribeToEvent(Contains.ON_CONTROLS_CHANGED, SetCursorPosition);
        EventManager.TriggerEvent(Contains.ON_CONTROLS_CHANGED, _playerInput.currentControlScheme);
    }
    private void Update()
    {
        _lookAtMouse?.Invoke();

        //if (_inputManager.GetButton("Knife") && _currentSecundaryWeapon)
        //    _currentSecundaryWeapon.Attack();

        //if (_inputManager.GetButtonDown("Throw Weapon")) ThrowWeapon();

        //if (_inputManager.GetButtonDown("Interact") && _onWeaponTrigger) SetWeapon();

        //if (_inputManager.GetButton("Shoot") && _currentMainWeapon) _currentMainWeapon.Attack();

        if (_shoot.ReadValue<float>() > .1f && _currentMainWeapon) _currentMainWeapon.Attack();
        if (_knife.ReadValue<float>() > .1f && _currentSecundaryWeapon) _currentSecundaryWeapon.Attack();
    }
    void OnInteract(InputAction.CallbackContext obj) { if (_onWeaponTrigger) SetWeapon(); }

    #region Weapon Funcs
    public void SetWeapon()
    {
        if (_currentMainWeapon) return;

        var col = Physics2D.OverlapCircle(transform.position, 2f, Helpers.GameManager.WeaponLayer);
        Weapon newWeapon = col ? col.GetComponent<Weapon>() : null;

        if (!newWeapon.CanPickUp) return;

        if (_currentMainWeapon)
            ThrowWeapon();

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
    }
    private void ThrowWeapon()
    {
        RaycastHit2D raycast = Physics2D.Raycast(_mainWeaponContainer.position, GetMousePosition(), 1f, LayerMask.GetMask("Border"));
        if (raycast)
        {
            _currentMainWeapon.transform.position += (Vector3)raycast.normal / 2;
            _currentMainWeapon?.ThrowOut(raycast.normal).SetParent(null);
        }
        else
            _currentMainWeapon?.ThrowOut(GetMouseDirectionMain()).SetParent(null);

        _currentMainWeapon = null;
        _mainWeaponContainer.eulerAngles = Vector2.zero;
        _lookAtMouse -= MainWeapon;
        _rightHand.SetActive(true);
        _leftHand.SetActive(true);
    }

    Vector2 primaryWeaponRotation;
    void MainWeapon()
    {
        if (Vector3.Dot(transform.right, _cursorPosition()) < Mathf.Cos(90) * Mathf.Deg2Rad) return;

        _mainWeaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());
        primaryWeaponRotation = new Vector2(_currentMainWeapon.transform.localScale.x, Mathf.Sign(GetMouseDirectionMain().x));
        _currentMainWeapon.transform.localScale = primaryWeaponRotation;
    }

    Vector2 secondaryWeaponSize;
    void SecundaryWeapon()
    {
        if (Vector3.Dot(transform.right, _cursorPosition()) < Mathf.Cos(90) * Mathf.Deg2Rad) return;

        _secundaryWeaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());
        secondaryWeaponSize = new Vector2(_secundaryWeaponTransform.localScale.x, Mathf.Sign(GetMouseDirectionSecundary().x));
        _secundaryWeaponTransform.localScale = secondaryWeaponSize;
    }

    #endregion

    #region Mouse Funcs
    public float GetAngle() => Mathf.Atan2(_cursorPosition().y, _cursorPosition().x) * Mathf.Rad2Deg;
    public Vector2 GetMousePosition() => _mainCamera.ScreenToWorldPoint(new Vector3(MouseCursorPosition().x, MouseCursorPosition().y, _mainCamera.nearClipPlane));
    Vector2 GetMouseDirectionMain() => (GetMousePosition() - (Vector2)_mainWeaponContainer.position).normalized;
    Vector2 GetMouseDirectionSecundary() => (GetMousePosition() - (Vector2)_secundaryWeaponContainer.position).normalized;

    #endregion
    void SetCursorPosition(params object[] param) => _cursorPosition = (string)param[0] == "Gamepad" ? (Func<Vector2>)GamepadCursorPosition : (Func<Vector2>)GetMouseDirectionMain;
    Vector2 GamepadCursorPosition() => Gamepad.current.rightStick.ReadValue();
    Vector2 MouseCursorPosition() => Mouse.current.position.ReadValue();

    void OnControlChanged(PlayerInput obj)
    {
        if (_playerInput.currentControlScheme == KEYBOARD_MOUSE && _previousControlScheme != KEYBOARD_MOUSE)
        {
            Cursor.visible = true;
            _previousControlScheme = KEYBOARD_MOUSE;
        }
        else if (_playerInput.currentControlScheme == GAMEPAD_SCHEME && _previousControlScheme != GAMEPAD_SCHEME)
        {
            Cursor.visible = false;
            _previousControlScheme = GAMEPAD_SCHEME;
        }

        EventManager.TriggerEvent(Contains.ON_CONTROLS_CHANGED, _playerInput.currentControlScheme);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>()) _onWeaponTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>()) _onWeaponTrigger = false;
    }
    private void OnDestroy()
    {
        _lookAtMouse -= SecundaryWeapon;
        _lookAtMouse -= MainWeapon;

        _interact.performed -= OnInteract;

        _knife.Disable();
        _interact.Disable();
        _shoot.Disable();

        EventManager.UnSubscribeToEvent(Contains.ON_CONTROLS_CHANGED, SetCursorPosition);
        _playerInput.onControlsChanged -= OnControlChanged;
    }
}
