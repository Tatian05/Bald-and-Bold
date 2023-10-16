using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class EventSystemScript : MonoBehaviour
{
    public static EventSystemScript Instance { get; private set; }

    Stack<GameObject> _stack = new Stack<GameObject>();
    EventSystem _eventSystem;
    private void Awake()
    {
        Instance = this;
        _eventSystem = FindObjectOfType<EventSystem>();
    }
    public void AddToStack(GameObject go)
    {
        _stack.Push(_eventSystem.currentSelectedGameObject);
        _stack.Push(go);
        _eventSystem.SetSelectedGameObject(go);
    }
    public void RemoveToStack()
    {
        _stack.Peek();
        _eventSystem.SetSelectedGameObject(_stack.Last());
    }
    public void SetCurrentGameObjectSelected(GameObject go)
    {
        _eventSystem.SetSelectedGameObject(go);
    }
}
