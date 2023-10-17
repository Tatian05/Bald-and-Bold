using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class EventSystemScript : MonoBehaviour
{
    public static EventSystemScript Instance { get; private set; }

    Stack<GameObject> _stack = new Stack<GameObject>();
    EventSystem _eventSystem;
    GameObject _lastSelectedGO;
    private void Awake()
    {
        Instance = this;
        _eventSystem = FindObjectOfType<EventSystem>();
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(Contains.ON_CONTROLS_CHANGED, OnControlsChanged);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_CONTROLS_CHANGED, OnControlsChanged);
    }
    public void AddToStack(GameObject go)
    {
        if (_stack.Contains(go)) return;
        _stack.Push(_eventSystem.currentSelectedGameObject);
        _stack.Push(go);
    }
    public void RemoveToStack(GameObject go = null)
    {
        if (!_stack.Contains(go)) return;
        _stack.Peek();
        _eventSystem.SetSelectedGameObject(_stack.Last());
    }
    public void SetCurrentGameObjectSelected(GameObject go)
    {
        if (!go) _lastSelectedGO = _eventSystem.currentSelectedGameObject;
        _eventSystem.SetSelectedGameObject(go);
    }

    void OnControlsChanged(params object[] param)
    {
        if (_eventSystem.currentSelectedGameObject) return;
        var go = _lastSelectedGO ? _lastSelectedGO : _eventSystem.firstSelectedGameObject;
        _eventSystem.SetSelectedGameObject(go);
    }
}
