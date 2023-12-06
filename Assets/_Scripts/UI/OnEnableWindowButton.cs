using UnityEngine;
public class OnEnableWindowButton : MonoBehaviour
{
    [SerializeField] GameObject _onEnableSelectedButton;
    EventSystemScript _eventSystem;
    private void Awake()
    {
        _eventSystem = EventSystemScript.Instance;     
    }
    private void OnEnable()
    {
        SetCurrentButton();
    }
    public void SetCurrentButton()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
            _eventSystem.SetCurrentGameObjectSelected(_onEnableSelectedButton);
    }
}
