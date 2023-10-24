using UnityEngine;
public class OnEnableSelectedButton : MonoBehaviour
{
    [SerializeField] GameObject _onEnableSelectedButton;
    EventSystemScript _eventSystem;
    private void Awake()
    {
        _eventSystem = EventSystemScript.Instance;
    }
    private void OnEnable()
    {
        _eventSystem.AddToStack(_onEnableSelectedButton);
        if (NewInputManager.activeDevice == DeviceType.Keyboard) return;
        _eventSystem.SetCurrentGameObjectSelected(_onEnableSelectedButton);
    }
    private void OnDisable()
    {
        _eventSystem.RemoveToStack(_onEnableSelectedButton);
    }
}
