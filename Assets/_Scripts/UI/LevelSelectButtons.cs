using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LevelSelectButtons : EventTrigger
{
    Image _backgroundImg;
    Color _currentColor;
    private void Start()
    {
        if (!GetComponent<Button>().interactable) enabled = false;

        _backgroundImg = gameObject.GetComponentInParent<Image>(false, true);
        _currentColor = _backgroundImg.color;
    }
    public override void OnSelect(BaseEventData eventData)
    {
        _backgroundImg.color = Color.yellow;
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        _backgroundImg.color = _currentColor;
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        _backgroundImg.color = Color.yellow;
    }
}
