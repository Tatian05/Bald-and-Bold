using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LevelSelectButtons : EventTrigger
{
    Image _backgroundImg;
    private void Start()
    {
        _backgroundImg = gameObject.GetComponentInParent<Image>(false, true);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        _backgroundImg.color = Color.yellow;
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        _backgroundImg.color = Color.green;
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        _backgroundImg.color = Color.yellow;
    }
}
