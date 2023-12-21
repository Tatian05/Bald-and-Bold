using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopItem : MonoBehaviour
{
    public ShoppableSO ShoppableSO { get { return _shoppableSO; } }
    public Button Button { get { return GetComponent<Button>(); } }

    [SerializeField] ShoppableSO _shoppableSO;
    [SerializeField] Image _cosmeticImg;
    [SerializeField] TextMeshProUGUI _shoppableSONameTxt, _shoppableTypeTxt;
    [SerializeField] TextMeshProUGUI _inCollectionTxt;
    [SerializeField] Image _qualityImg;

    public bool InCollection = false;
    public void SetInCollection(bool inCollection)
    {
        if (!inCollection) _shoppableSO.Buy();
        if (_shoppableSO is ConsumableData) return;

        InCollection = true;
        GetComponent<Button>().interactable = false;
        GetComponent<Button>().image.color = Color.clear;
        _inCollectionTxt.gameObject.SetActive(true);
        _inCollectionTxt.rectTransform.eulerAngles = new Vector3(0, 0, Random.Range(-45, 46));
    }
    public ShopItem SetCosmeticData(ShoppableSO shoppableSO, bool gachaball = false)
    {
        _shoppableSO = shoppableSO;
        _shoppableSO.OnStart();
        _qualityImg.color = GetQualityColor(_shoppableSO.shoppableQuality);
        _shoppableTypeTxt.text = gachaball ? shoppableSO.shoppableTypeText : string.Empty;
        _cosmeticImg.sprite = _shoppableSO.shopSprite;
        _shoppableSONameTxt.text = _shoppableSO.shoppableName;
        transform.localScale = Vector3.one;
        return this;
    }
    Color GetQualityColor(ShoppableQuality quality) => quality switch
    {
        ShoppableQuality.Normal => Color.grey,
        ShoppableQuality.Rare => Color.green,
        ShoppableQuality.Epic => Color.magenta,
        _ => throw new System.Exception($"{quality} not founded")
    };
    public void ResetGachaItemShop() { _shoppableSO = null; }
    public ShopItem SetParent(Transform parent)
    {
        transform.SetParent(parent, true);
        return this;
    }
}
