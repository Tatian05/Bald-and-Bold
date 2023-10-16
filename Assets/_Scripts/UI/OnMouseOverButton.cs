using UnityEngine.EventSystems;
public class OnMouseOverButton : EventTrigger
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        EventSystemScript.Instance.SetCurrentGameObjectSelected(gameObject);
    }
}
