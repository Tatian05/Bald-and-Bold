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
        if (_eventSystemScript.CurrentSelectedGO)
            _lastSelectedObject = _eventSystemScript.CurrentSelectedGO;
    }
    void SetSelectedButton()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
            EventSystemScript.Instance.SetCurrentGameObjectSelected(_lastSelectedObject ? _lastSelectedObject : _firstSelectedButton);
    }
    public void SetLastSelectedButton(GameObject lastSelectedButton) { _lastSelectedObject = lastSelectedButton; }
}
