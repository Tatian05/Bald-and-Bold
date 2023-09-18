using System;
using UnityEngine;
using Weapons;
public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon _currentMainWeapon, _currentSecundaryWeapon;
    [SerializeField] GameObject _rightHand, _leftHand;

    Transform _mainWeaponContainer, _secundaryWeaponContainer;
    Transform _secundaryWeaponTransform;
    InputManager _inputManager;
    Action _lookAtMouse = delegate { };
    bool _onWeaponTrigger;
    public Transform SecundaryWeaponContainer { get { return _secundaryWeaponContainer; } }
    public Transform MainWeaponContainer { get { return _mainWeaponContainer; } }
    public FireWeapon GetMainWeapon { get { return _currentMainWeapon?.GetComponent<FireWeapon>(); } }
    private void Start()
    {
        _inputManager = InputManager.Instance;
        _mainWeaponContainer = transform.GetChild(0);
        _secundaryWeaponContainer = transform.GetChild(1);
        _currentSecundaryWeapon = _secundaryWeaponContainer.GetComponentInChildren<Weapon>();
        _secundaryWeaponTransform = _currentSecundaryWeapon.transform;
        _currentSecundaryWeapon.PickUp(true);
        _lookAtMouse += SecundaryWeapon;
    }
    private void Update()
    {
        _lookAtMouse?.Invoke();

        if (_inputManager.GetButton("Knife") && _currentSecundaryWeapon)
        {
            Helpers.LevelTimerManager.StartLevelTimer();
            _currentSecundaryWeapon.Attack();
        }

        //if (_inputManager.GetButtonDown("Throw Weapon")) ThrowWeapon();

        if (_inputManager.GetButtonDown("Interact") && _onWeaponTrigger) SetWeapon();
    }

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
        _lookAtMouse += () => { if (_inputManager.GetButton("Shoot") && _currentMainWeapon) _currentMainWeapon.Attack(); };
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
        _mainWeaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());
        primaryWeaponRotation = new Vector2(_currentMainWeapon.transform.localScale.x, Mathf.Sign(GetMouseDirectionMain().x));
        _currentMainWeapon.transform.localScale = primaryWeaponRotation;
    }

    Vector2 secondaryWeaponSize;
    void SecundaryWeapon()
    {
        _secundaryWeaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());
        secondaryWeaponSize = new Vector2(_secundaryWeaponTransform.localScale.x, Mathf.Sign(GetMouseDirectionSecundary().x));
        _secundaryWeaponTransform.localScale = secondaryWeaponSize;
    }

    #endregion

    #region Mouse Funcs
    public float GetAngle() => Mathf.Atan2(GetMouseDirectionMain().y, GetMouseDirectionMain().x) * Mathf.Rad2Deg;
    public Vector2 GetMousePosition() => Helpers.MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Helpers.MainCamera.nearClipPlane));
    Vector2 GetMouseDirectionMain() => (GetMousePosition() - (Vector2)_mainWeaponContainer.position).normalized;
    Vector2 GetMouseDirectionSecundary() => (GetMousePosition() - (Vector2)_secundaryWeaponContainer.position).normalized;

    #endregion

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
        _lookAtMouse = delegate { };
    }
}
