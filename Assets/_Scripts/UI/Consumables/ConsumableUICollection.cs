using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ConsumableUICollection : MonoBehaviour
{
    [SerializeField] Image _consumableImg;
    [SerializeField] TextMeshProUGUI _countTxt; 
    public ConsumableUICollection SetParent(Transform parent)
    {
        transform.SetParent(parent);
        return this;
    }
    public ConsumableUICollection SetImage(Sprite sprite)
    {
        _consumableImg.sprite = sprite;
        return this;
    }
    public ConsumableUICollection SetCount(int count)
    {
        _countTxt.text = count.ToString();
        return this;
    }
}
