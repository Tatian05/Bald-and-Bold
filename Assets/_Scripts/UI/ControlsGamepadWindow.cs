using UnityEngine;
using UnityEngine.UI;
public class ControlsGamepadWindow : MonoBehaviour
{
    [SerializeField] Button _resetDefaultButton;
    [SerializeField] GameObject _xboxWindow, _playstationWindow;

    Button _gamepadWindowButton;
    GameObject _currentWindowActive;
    private void Awake()
    {
        _gamepadWindowButton = GetComponent<Button>();
        _gamepadWindowButton.onClick.AddListener(OpenCurrentGamepadWindow);

        _resetDefaultButton.onClick.AddListener(ResetDefault);
    }
    void OpenCurrentGamepadWindow()
    {
        if(NewInputManager.activeDevice == DeviceType.xBoxGamepad)
        {
            _xboxWindow.SetActive(true);
            _currentWindowActive = _xboxWindow;
        }
        else
        {
            _playstationWindow.SetActive(true);
            _currentWindowActive = _playstationWindow;
        }
        _currentWindowActive.GetComponent<OnEnableSelectedButton>().SetCurrentButton();
    }
    void ResetDefault()
    {
        foreach (RebindUI item in _currentWindowActive.transform)
            item.ResetBinding();
    }
}
