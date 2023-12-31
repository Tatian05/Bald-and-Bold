using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GamepadOnSelectButton : MonoBehaviour, ISelectHandler
{
    Button _myButton;
    void Awake()
    {
        _myButton = GetComponent<Button>();
    }
    public void OnSelect(BaseEventData eventData)
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
            _myButton.onClick.Invoke();
    }
}
