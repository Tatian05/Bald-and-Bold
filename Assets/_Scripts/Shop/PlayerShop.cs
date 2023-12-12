using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerShop : MonoBehaviour
{
    [SerializeField] float _speedMovement;
    [SerializeField] GameObject _shopCanvas, _collectionCanvas;
    [SerializeField] Gachapon _gachapon;

    Animator _animator;
    bool _onShopTrigger, _onProbadorTrigger, _onGachaTrigger;
    InputAction _interact, _movement;
    Vector3 _movementInputs;
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _interact = NewInputManager.PlayerInputs.Player.Interact;
        _movement = NewInputManager.PlayerInputs.Player.Movement;

        _interact.performed += OpenShop;
        _interact.performed += OpenProbador;
        _interact.performed += PlayGachapon;

        _interact.Enable();
        _movement.Enable();

        GachaBallAnimationScrip.OnGachaEnd += EnablePlayer;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Helpers.PersistantData.persistantDataSaved.AddPresiCoins(200);
            Helpers.PersistantData.persistantDataSaved.AddGoldenBaldCoins(100);
        }
        Move();
    }
    void Move()
    {
        _movementInputs = new Vector3 { x = Mathf.RoundToInt(_movement.ReadValue<Vector2>().x), y = 0};
        transform.position += _movementInputs * _speedMovement * Time.deltaTime;

        PlayAnimation(_movementInputs.x);

        if (_movementInputs.magnitude != 0)
            transform.eulerAngles = _movementInputs.x < 0 ? new Vector3(0, 180, 0) : Vector3.zero;
    }
    void PlayGachapon(InputAction.CallbackContext obj)
    {
        if (!_onGachaTrigger || !_gachapon.CanGacha) return;

        _gachapon.Gacha();
        DisablePlayer();
    }
    void OpenShop(InputAction.CallbackContext obj)
    {
        if (!_onShopTrigger) return;

        if (_shopCanvas.activeSelf)
        {
            ResetState();
            _shopCanvas.GetComponent<WindowsAnimation>().OnWindowClose();
            return;
        }

        _shopCanvas.SetActive(true);
        //_movement.Disable();
        DisablePlayer();
        ActivateMouse();
    }
    void OpenProbador(InputAction.CallbackContext obj)
    {
        if (!_onProbadorTrigger) return;

        if (_collectionCanvas.activeSelf)
        {
            ResetState();
            _collectionCanvas.GetComponent<WindowsAnimation>().OnWindowClose();
            return;
        }

        _collectionCanvas.SetActive(true);
       //_movement.Disable();
        ActivateMouse();
    }
    void ActivateMouse()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard) return;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void ResetState()
    {
        EnablePlayer();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
    void PlayAnimation(float xAxis) => _animator.SetInteger("xAxis", (int)Mathf.Abs(xAxis));
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NextSceneOnTrigger>())
        {
            PlayAnimation(0);
            DisablePlayer();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.CompareTag("Seller")) _onShopTrigger = true;

        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.CompareTag("Probador")) _onProbadorTrigger = true;

        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.CompareTag("Gachapon")) _onGachaTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.CompareTag("Seller")) _onShopTrigger = false;

        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.CompareTag("Probador")) _onProbadorTrigger = false;

        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.CompareTag("Gachapon")) _onGachaTrigger = false;
    }
    private void OnDestroy()
    {
        _interact.performed -= OpenShop;
        _interact.performed -= OpenProbador;
        _interact.performed -= PlayGachapon;

        DisablePlayer();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void EnablePlayer() { NewInputManager.PlayerInputs.Player.Enable(); }
    void DisablePlayer() { NewInputManager.PlayerInputs.Player.Disable(); }
}
