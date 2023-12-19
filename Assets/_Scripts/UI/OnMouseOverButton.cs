using UnityEngine.EventSystems;
using UnityEngine.UI;
public class OnMouseOverButton : EventTrigger
{
    EventSystemScript _eventSystem;
    private void Start()
    {
        if (!GetComponent<Selectable>().interactable) enabled = false;
        _eventSystem = EventSystemScript.Instance;
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        Helpers.AudioManager.PlaySFX("ButtonSound");
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        _eventSystem.SetCurrentGameObjectSelected(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard) return;
        _eventSystem.SetCurrentGameObjectSelected(null);
    }
}
