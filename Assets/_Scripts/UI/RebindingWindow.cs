using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class RebindingWindow : MonoBehaviour
{
    [SerializeField] Button _resetDefaultButton, _okButton;
    [SerializeField] GameObject[] _devicesWindows;
    [SerializeField] GameObject _currentWindowActive;

    private void Awake()
    {
        _resetDefaultButton.onClick.AddListener(ResetDefault);
        OpenCurrentWindow();
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OpenCurrentWindow;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OpenCurrentWindow;
    }
    void OpenCurrentWindow()
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
}
