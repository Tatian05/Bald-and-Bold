using UnityEngine;
using UnityEngine.UI;
public class OnEnableSelectedButton : MonoBehaviour
{
    [SerializeField] GameObject _onEnableSelectedButton;
    [SerializeField] Button _backButton;
    EventSystemScript _eventSystem;
    [SerializeField] GameObject _lastSelectedObject;
    bool _exit;
    private void OnEnable()
    {
        if (_eventSystem) SetCurrentButton();
        _exit = false;
    }

    private void Start()
    {
        _eventSystem = EventSystemScript.Instance;
        SetCurrentButton();
        if (_backButton) _backButton.onClick.AddListener(BackButton);
    }
    private void OnDisable()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard && !_exit)
        {
            _lastSelectedObject = _eventSystem.CurrentSelectedGO;
            _eventSystem.AddToStack();
        }
    }
    public void SetCurrentButton()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
            _eventSystem.SetCurrentGameObjectSelected(_exit ? _onEnableSelectedButton : _lastSelectedObject);
    }
    void BackButton()
    {
        _lastSelectedObject = null;
        _eventSystem.RemoveToStack();
        _exit = true;
    }
}
