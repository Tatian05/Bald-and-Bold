using UnityEngine;
using UnityEngine.UI;
public class ItemShopQuality : MonoBehaviour
{
    [SerializeField] Image[] _imgEffect;
    public void SetColorEffect(Color color)
    {
        foreach (var item in _imgEffect)
            item.color = color;
    }
}
