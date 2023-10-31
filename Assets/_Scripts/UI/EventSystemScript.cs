using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EventSystemScript : MonoBehaviour
{
    public static EventSystemScript Instance { get; private set; }

    Stack<GameObject> _stack = new Stack<GameObject>();
    EventSystem _eventSystem;
    GameObject _lastSelectedGO;
    DefaultInputActions _uiNavigateActions;
    public DefaultInputActions UIInputs { get { return _uiNavigateActions; } private set { } }
    public GameObject CurrentSelectedGO
    {
        get
        {
            Debug.Log(_eventSystem.currentSelectedGameObject == null);
            if (!_eventSystem.currentSelectedGameObject) return null;

            return _eventSystem.currentSelectedGameObject;
        }
    }
    private void Awake()
    {
        Instance = this;
        _uiNavigateActions = new DefaultInputActions();
        _eventSystem = FindObjectOfType<EventSystem>();
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OnControlsChanged;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= OnControlsChanged;
    }
    public void AddToStack()
    {
        _stack.Push(_eventSystem.currentSelectedGameObject);
    }
    public void RemoveToStack()
    {
        _eventSystem.SetSelectedGameObject(_stack.Pop());
    }
    public void SetCurrentGameObjectSelected(GameObject go)
    {
        if (!go) _lastSelectedGO = _eventSystem.currentSelectedGameObject;
        _eventSystem.SetSelectedGameObject(go);
    }
    void OnControlsChanged()
    {
        if (_eventSystem.currentSelectedGameObject) return;
        var go = _lastSelectedGO ? _lastSelectedGO : _eventSystem.firstSelectedGameObject;
        _eventSystem.SetSelectedGameObject(go);
    }

    public void CancelUINavigate()
    {
        _uiNavigateActions.Disable();
        _eventSystem.SetSelectedGameObject(null);
    }
}
