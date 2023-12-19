using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LevelSelectButtons : EventTrigger
{
    Image _backgroundImg;
    Color _currentColor = Color.green;

    public Color ButtonColor { get { return _currentColor; } set { _currentColor = value; } }
    private void Awake()
    {
        if (!GetComponent<Button>().interactable) enabled = false;
        _backgroundImg = gameObject.GetComponentInParent<Image>(false, true);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData); 

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
