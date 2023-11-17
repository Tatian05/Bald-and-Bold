using UnityEngine;
using UnityEngine.UI;
public enum CosmeticType { Player, President }
[CreateAssetMenu(fileName = "New Cosmetic", menuName = "New Cosmetic")]
public class CosmeticData : ShoppableSO
{
    public CosmeticType cosmeticType;
    public string torsoSpriteName, rightLegSpriteName, leftLegSpriteName, tailSpriteName;
    [System.NonSerialized] public string rightHandSpriteName = "Mano", leftHandSpriteName = "Mano";
    Sprite _torsoSprite, _rightLegSprite, _leftLegSprite, _rightHandSprite, _leftHandSprite, _tailSprite;
    public Sprite HeadSprite => shopSprite;
    public Sprite TorsoSprite => _torsoSprite;
    public Sprite RightLegSprite => _rightLegSprite;
    public Sprite LeftLegSprite => _leftLegSprite;
    public Sprite RightHandSprite => _rightHandSprite;
    public Sprite LeftHandSprite => _leftHandSprite;
    public Sprite TailSprite => _tailSprite;
    override protected void OnEnable()
    {
        base.OnEnable();
        _torsoSprite = Resources.Load<Sprite>($"ShopSprites/Cosmetics/{torsoSpriteName}");
        _rightLegSprite = Resources.Load<Sprite>($"ShopSprites/Cosmetics/{rightLegSpriteName}");
        _leftLegSprite = Resources.Load<Sprite>($"ShopSprites/Cosmetics/{leftLegSpriteName}");
        _rightHandSprite = Resources.Load<Sprite>($"ShopSprites/Cosmetics/{rightHandSpriteName}");
        _leftHandSprite = Resources.Load<Sprite>($"ShopSprites/Cosmetics/{leftHandSpriteName}");
        if (tailSpriteName != string.Empty) _tailSprite = Resources.Load<Sprite>($"ShopSprites/Cosmetics/{tailSpriteName}");
    }
    public void SetCosmetic(Image headSprite, Image torsoSprite, Image rightLegSprite, Image leftLegSprite, Image rightHandSprite, Image leftHandSprite, Image tailSprite)
    {
        headSprite.sprite = HeadSprite;
        torsoSprite.sprite = _torsoSprite;
        rightLegSprite.sprite = _rightLegSprite;
        leftLegSprite.sprite = _leftLegSprite;
        rightHandSprite.sprite = _rightHandSprite;
        leftHandSprite.sprite = _leftHandSprite;
        if (_tailSprite)
        {
            tailSprite.gameObject.SetActive(true);
            tailSprite.sprite = _tailSprite;
        }
        else
        {
            tailSprite.sprite = null;
            tailSprite.gameObject.SetActive(false);
        }
    }
    public override void Buy()
    {
        base.Buy();
        Helpers.PersistantData.persistantDataSaved.AddCosmetic(cosmeticType, this);
    }
}
