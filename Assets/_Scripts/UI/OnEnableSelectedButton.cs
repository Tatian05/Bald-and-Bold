using UnityEngine;
using UnityEngine.UI;
public class OnEnableSelectedButton : MonoBehaviour
{
    [SerializeField] GameObject _onEnableSelectedButton;
    [SerializeField] Button _backButton;
    EventSystemScript _eventSystem;
    [SerializeField] GameObject _lastSelectedObject;
    bool _exit;
    private void Awake()
    {
        _eventSystem = EventSystemScript.Instance;     
    }
    private void OnEnable()
    {
        if (_eventSystem) SetCurrentButton();
    }

    private void Start()
    {
        SetCurrentButton();
        if (_backButton) _backButton.onClick.AddListener(() => { _lastSelectedObject = null; _exit = true; });
    }
    private void OnDisable()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard && !_exit)
        {
            if (!_eventSystem) _eventSystem = EventSystemScript.Instance;
            _lastSelectedObject = _eventSystem.CurrentSelectedGO;
        }
    }
    public void SetCurrentButton()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _eventSystem.SetCurrentGameObjectSelected(_lastSelectedObject ? _lastSelectedObject : _onEnableSelectedButton);
            _exit = false;
        }
    }
}
