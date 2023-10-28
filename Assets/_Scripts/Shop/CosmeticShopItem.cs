using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CosmeticShopItem : MonoBehaviour
{
    public ShoppableSO ShoppableSO { get { return _shoppableSO; } }

    [SerializeField] ShoppableSO _shoppableSO;
    [SerializeField] Image _cosmeticImg;
    [SerializeField] TextMeshProUGUI _shoppableSONameTxt;
    [SerializeField] TextMeshProUGUI _inCollectionTxt;
    public void SetCosmeticData(ShoppableSO shoppableSO, Sprite headSprite)
    {
        _shoppableSO = shoppableSO;
        _shoppableSO.OnStart();
        _cosmeticImg.sprite = headSprite;
        _shoppableSONameTxt.text = _shoppableSO.shoppableData.shoppableName;
    }
    public void SetInCollection()
    {
        GetComponent<Button>().interactable = false;
        GetComponent<Button>().image.color = Color.clear;
        _inCollectionTxt.gameObject.SetActive(true);
        _inCollectionTxt.rectTransform.eulerAngles = new Vector3(0, 0, Random.Range(-45, 46));
    }
}
