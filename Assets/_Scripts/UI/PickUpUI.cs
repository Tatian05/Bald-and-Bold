using UnityEngine;
using TMPro;
public class PickUpUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _pickUpTxt;

    public PickUpUI SetActive(bool enable)
    {
        gameObject.SetActive(enable);
        return this;
    }
    public PickUpUI SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public PickUpUI SetCanvas(string txt)
    {
        _pickUpTxt.text = txt;
        return this;
    }
}
