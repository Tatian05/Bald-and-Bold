using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CosmeticShopItem : MonoBehaviour
{
    public CosmeticData CosmeticData { get { return _cosmeticData; } }

    [SerializeField] CosmeticData _cosmeticData;
    [SerializeField] Image _cosmeticImg;
    [SerializeField] TextMeshProUGUI _cosmeticNameTxt;
    [SerializeField] TextMeshProUGUI _inCollectionTxt;
    public void SetCosmeticData(CosmeticData cosmeticData, Sprite headSprite)
    {
        _cosmeticData = cosmeticData;

        _cosmeticImg.sprite = headSprite;
        _cosmeticNameTxt.text = _cosmeticData.cosmeticName;
    }

    public void SetCosmetics(ref Image headSprite, ref Image torsoSprite, ref Image rightLegSprite, ref Image leftLegSprite, ref Image rightHandSprite, ref Image leftHandSprite, ref Image tailSprite)
    {
        headSprite.sprite = _cosmeticData.headSprite;
        torsoSprite.sprite = _cosmeticData.torsoSprite;
        rightLegSprite.sprite = _cosmeticData.rightLegSprite;
        leftLegSprite.sprite = _cosmeticData.leftLegSprite;
        rightHandSprite.sprite = _cosmeticData.rightHandSprite;
        leftHandSprite.sprite = _cosmeticData.leftHandSprite;
        if (_cosmeticData.tailSprite)
        {
            tailSprite.gameObject.SetActive(true);
            tailSprite.sprite = _cosmeticData.tailSprite;
        }
        else
        {
            tailSprite.sprite = null;
            tailSprite.gameObject.SetActive(false);
        }
    }

    public void SetInCollection()
    {
        GetComponent<Button>().interactable = false;
        GetComponent<Button>().image.color = Color.clear;
        _inCollectionTxt.gameObject.SetActive(true);
        _inCollectionTxt.rectTransform.eulerAngles = new Vector3(0, 0, Random.Range(-45, 46));
    }
}
