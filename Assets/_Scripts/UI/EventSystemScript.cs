using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class EventSystemScript : MonoBehaviour
{
    public static EventSystemScript Instance { get; private set; }

    EventSystem _eventSystem;
    GameObject _lastSelectedGO;
    DefaultInputActions _uiNavigateActions;
    public DefaultInputActions UIInputs { get { return _uiNavigateActions; } private set { } }
    public GameObject CurrentSelectedGO
    {
        get
        {
            if (!_eventSystem.currentSelectedGameObject) return null;

            return _eventSystem.currentSelectedGameObject;
        }
    }
    private void Awake()
    {
        Instance = this;
        _uiNavigateActions = new DefaultInputActions();
        _eventSystem = EventSystem.current;
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OnControlsChanged;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= OnControlsChanged;
    }
    public void SetCurrentGameObjectSelected(GameObject go)
    {
        if (!go) _lastSelectedGO = _eventSystem.currentSelectedGameObject;
        _eventSystem.SetSelectedGameObject(go);
    }
    void OnControlsChanged()
    {
        if (NewInputManager.activeDevice == DeviceType.Keyboard) return;
        var go = _lastSelectedGO ? _lastSelectedGO : _eventSystem.firstSelectedGameObject;
        _eventSystem.SetSelectedGameObject(go);
    }

    public void CancelUINavigate()
    {
        _uiNavigateActions.Disable();
        _eventSystem.SetSelectedGameObject(null);
    }
}
