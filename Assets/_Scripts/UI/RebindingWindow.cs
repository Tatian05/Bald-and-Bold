using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
public class RebindingWindow : MonoBehaviour
{
    [SerializeField] Button _resetDefaultButton;
    [SerializeField] GameObject[] _devicesWindows;
    [SerializeField] GameObject _currentWindowActive;

    [Header("Gamepad Settings")]
    [SerializeField] string[] _tmpSpritesBackText;
    [SerializeField] string[] _tmpSpritesResetBindText;
    [SerializeField] TextMeshProUGUI _tmpBackSprite, _tmpResetBindSprite;
    [SerializeField] GameObject _backButtonGO, _backTMPSpriteGO;
    [SerializeField] GameObject _resetBindButtonGO, _resetBindTMPSpriteGO;

    InputAction _resetBindAction;

    private void Awake()
    {
        _resetDefaultButton.onClick.AddListener(ResetDefault);
        OpenCurrentWindow();
        _resetBindAction = NewInputManager.PlayerInputs.UI.SpecialAction;
        GamepadBack();
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OpenCurrentWindow;
        NewInputManager.ActiveDeviceChangeEvent += GamepadBack;

        NewInputManager.rebindStarted += (x,y) => DisableAction();
        NewInputManager.RebindCanceled += OnRebindFinish;
        NewInputManager.RebindComplete += OnRebindFinish;

        _resetBindAction.performed += ResetDefault;
        _resetBindAction.Enable();
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= OpenCurrentWindow;
        NewInputManager.ActiveDeviceChangeEvent -= GamepadBack;

        NewInputManager.rebindStarted -= (x, y) => DisableAction();
        NewInputManager.RebindCanceled -= OnRebindFinish;
        NewInputManager.RebindComplete -= OnRebindFinish;

        _resetBindAction.performed -= ResetDefault;
        _resetBindAction.Disable();
    }
    void OnRebindFinish() { Invoke(nameof(EnableAction), .05f); }
    void EnableAction() { _resetBindAction.Enable(); }
    void DisableAction() { _resetBindAction.Disable(); }
    void OpenCurrentWindow()
    {
        foreach (var item in _devicesWindows) item.SetActive(false);

        _currentWindowActive = _devicesWindows[(int)NewInputManager.activeDevice];
        _currentWindowActive.SetActive(true);
        _currentWindowActive.GetComponent<OnEnableWindowButton>().SetCurrentButton();
    }
    void ResetDefault()
    {
        foreach (RebindUI item in _currentWindowActive.GetComponentsInChildren<RebindUI>(false))
        {
            if (!item || !item.enabled) continue;
            item.ResetBinding();
        }
    }
    void ResetDefault(InputAction.CallbackContext obj)
    {
        foreach (RebindUI item in _currentWindowActive.GetComponentsInChildren<RebindUI>(false))
        {
            if (!item || !item.enabled) continue;
            item.ResetBinding();
        }
    }

    void GamepadBack()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _backTMPSpriteGO.SetActive(true);
            _resetBindTMPSpriteGO.SetActive(true);

            _tmpBackSprite.text = _tmpSpritesBackText[(int)NewInputManager.activeDevice];
            _tmpResetBindSprite.text = _tmpSpritesResetBindText[(int)NewInputManager.activeDevice];

            _backButtonGO.SetActive(false);
            _resetBindButtonGO.SetActive(false);
        }
        else
        {
            _backTMPSpriteGO.SetActive(false);
            _resetBindTMPSpriteGO.SetActive(false);

            _backButtonGO.SetActive(true);
            _resetBindButtonGO.SetActive(true);
        }
    }
}
