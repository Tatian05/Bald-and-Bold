using UnityEngine;
public class OnEnableGridButtons : MonoBehaviour
{
    [SerializeField] GameObject _lastSelectedObject, _firstSelectedButton;
    EventSystemScript _eventSystemScript;
    private void OnEnable()
    {
        if (_eventSystemScript) SetSelectedButton();
    }
    private void Start()
    {
        _eventSystemScript = EventSystemScript.Instance;
        SetSelectedButton();
    }
    private void OnDisable()
    {
        _lastSelectedObject = _eventSystemScript.CurrentSelectedGO;
    }
    void SetSelectedButton()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
            EventSystemScript.Instance.SetCurrentGameObjectSelected(_lastSelectedObject ? _lastSelectedObject : _firstSelectedButton);
    }
}
