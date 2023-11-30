using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopItem : MonoBehaviour
{
    public ShoppableSO ShoppableSO { get { return _shoppableSO; } }

    [SerializeField] ShoppableSO _shoppableSO;
    [SerializeField] Image _cosmeticImg;
    [SerializeField] TextMeshProUGUI _shoppableSONameTxt;
    [SerializeField] TextMeshProUGUI _inCollectionTxt;
    public void SetInCollection(bool inCollection)
    {
        if (!inCollection) _shoppableSO.Buy();
        if (_shoppableSO is ConsumableData) return;

        GetComponent<Button>().interactable = false;
        GetComponent<Button>().image.color = Color.clear;
        _inCollectionTxt.gameObject.SetActive(true);
        _inCollectionTxt.rectTransform.eulerAngles = new Vector3(0, 0, Random.Range(-45, 46));
    }
    public ShopItem SetCosmeticData(ShoppableSO shoppableSO)
    {
        _shoppableSO = shoppableSO;
        _shoppableSO.OnStart();
        _cosmeticImg.sprite = _shoppableSO.shopSprite();
        _shoppableSONameTxt.text = _shoppableSO.shoppableName();
        transform.localScale = Vector3.one;
        return this;
    }

    public ShopItem SetParent(Transform parent)
    {
        transform.SetParent(parent, true);
        return this;
    }
}
