using UnityEngine.EventSystems;
using UnityEngine;
public class OnMouseOverButton : EventTrigger
{
    EventSystemScript _eventSystem;
    private void Start()
    {
        _eventSystem = EventSystemScript.Instance;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        _eventSystem.SetCurrentGameObjectSelected(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (OnControlsChange.Instance.CurrentControl == "Gamepad") return;
        _eventSystem.SetCurrentGameObjectSelected(null);
    }
}
