using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class GamepadCollectionNavigation : MonoBehaviour
{
    [SerializeField] Button[] _windowsButtons;
    [SerializeField] string[] _closeTMP, _equipTMP, _nextWindowTMP, _beforeWindowTMP;
    [SerializeField] TextMeshProUGUI _closeTMPText, _equipTMPText, _nextWindowTMPText, _beforeWindowTMPText;
    [SerializeField] GameObject _gamepadWindow, _closeButton;
    [SerializeField] Button _equipButton, _nextButton, _beforeButton;

    InputAction _equipAction, _nextWindowAction, _beforeWindowAction, _nextBeforeConsumableAction;
    float _passPageValue;
    private void Awake()
    {
        _equipAction = NewInputManager.PlayerInputs.UI.SpecialAction;
        _nextWindowAction = NewInputManager.PlayerInputs.UI.Next;
        _beforeWindowAction = NewInputManager.PlayerInputs.UI.Before;
        _nextBeforeConsumableAction = EventSystemScript.Instance.UIInputs.UI.Navigate;
    }
    private void Start()
    {
        GamepadNavigation();
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += GamepadNavigation;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= GamepadNavigation;
    }
    void GamepadNavigation()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _equipAction.performed += EquipButton;
            _equipAction.Enable();
            _nextWindowAction.performed += GamepadNextWindow;
            _nextWindowAction.Enable();
            _beforeWindowAction.performed += GamepadBeforeWindow;
            _beforeWindowAction.Enable();
            _nextBeforeConsumableAction.performed += PassCollectionItem;
            _nextBeforeConsumableAction.Enable();

            _closeTMPText.text = _closeTMP[(int)NewInputManager.activeDevice - 1];
            _equipTMPText.text = _equipTMP[(int)NewInputManager.activeDevice - 1];
            _nextWindowTMPText.text = _nextWindowTMP[(int)NewInputManager.activeDevice - 1];
            _beforeWindowTMPText.text = _beforeWindowTMP[(int)NewInputManager.activeDevice - 1];
            _gamepadWindow.SetActive(true);
            _closeButton.SetActive(false);
        }
        else
        {
            _equipAction.performed += EquipButton;
            _equipAction.Enable();
            _nextWindowAction.performed += GamepadNextWindow;
            _nextWindowAction.Enable();
            _beforeWindowAction.performed += GamepadBeforeWindow;
            _beforeWindowAction.Enable();
            _nextBeforeConsumableAction.performed += PassCollectionItem;
            _nextBeforeConsumableAction.Enable();

            _gamepadWindow.SetActive(false);
            _closeButton.SetActive(true);
        }
    }
    void PassCollectionItem(InputAction.CallbackContext obj)
    {
        _passPageValue = _nextBeforeConsumableAction.ReadValue<Vector2>().x;

        if (_passPageValue != 0)
        {
            if (_passPageValue >= 1) _nextButton.onClick.Invoke();
            else if(_passPageValue <= 1) _beforeButton.onClick.Invoke();
        }
    }

    void EquipButton(InputAction.CallbackContext obj)
    {
        if (_equipButton.interactable) _equipButton.onClick.Invoke();
    }

    int _windowIndex = 0;
    void GamepadNextWindow(InputAction.CallbackContext obj)
    {
        _windowIndex = (_windowIndex + 1) % _windowsButtons.Length;
        _windowsButtons[_windowIndex].onClick.Invoke();
    }
    void GamepadBeforeWindow(InputAction.CallbackContext obj)
    {
        _windowIndex--;
        if (_windowIndex < 0) _windowIndex += _windowsButtons.Length;
        _windowsButtons[_windowIndex].onClick.Invoke();
    }
}