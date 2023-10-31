using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class RebindingWindow : MonoBehaviour
{
    [SerializeField] Button _resetDefaultButton, _okButton;
    [SerializeField] GameObject[] _devicesWindows;
    [SerializeField] GameObject _currentWindowActive;

    InputAction _cancelUI;
    private void Awake()
    {
        _resetDefaultButton.onClick.AddListener(ResetDefault);
        OpenCurrentGamepadWindow();
        _cancelUI = EventSystemScript.Instance.UIInputs.UI.Cancel;
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OpenCurrentGamepadWindow;
        EventSystemScript.Instance.UIInputs.UI.Cancel.performed += CancelBinding;
        _cancelUI.Enable();
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OpenCurrentGamepadWindow;
        EventSystemScript.Instance.UIInputs.UI.Cancel.performed -= CancelBinding;
        _cancelUI.Disable();
    }
    void OpenCurrentGamepadWindow()
    {
        foreach (var item in _devicesWindows) item.SetActive(false);

        _currentWindowActive = _devicesWindows[(int)NewInputManager.activeDevice];
        _currentWindowActive.SetActive(true);
        _currentWindowActive.GetComponent<OnEnableSelectedButton>().SetCurrentButton();
    }
    void ResetDefault()
    {
        foreach (Transform item in _currentWindowActive.transform)
        {
            if (!item.TryGetComponent(out RebindUI rebindUI)) continue;
            rebindUI.ResetBinding();
        }
    }
    private void CancelBinding(InputAction.CallbackContext obj)
    {
        Debug.Log("asd");
        EventSystemScript.Instance.SetCurrentGameObjectSelected(_okButton.gameObject);
    }
}
