using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LevelSelectButtons : EventTrigger
{
    Image _backgroundImg;
    Color _currentColor = Color.green;

    public Color BackgroundColor { get { return _currentColor; } set { _currentColor = value; } }
    private void Start()
    {
        if (!GetComponent<Selectable>().interactable) enabled = false;
        _backgroundImg = gameObject.GetComponentInParent<Image>(false, true);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (_backgroundImg == null) _backgroundImg = gameObject.GetComponentInParent<Image>(false, true);
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
