using UnityEngine;
using UnityEngine.UI;
public enum CosmeticType { Player, President }
[CreateAssetMenu(fileName = "New CosmeticData", menuName = "New Cosmetic")]
public class CosmeticData : ShoppableSO
{
    public CosmeticType cosmeticType;
    public Sprite headSprite, torsoSprite, rightLegSprite, leftLegSprite, rightHandSprite, leftHandSprite, tailSprite;
    public void SetCosmetic (Image headSprite, Image torsoSprite, Image rightLegSprite, Image leftLegSprite, Image rightHandSprite, Image leftHandSprite, Image tailSprite)
    {
        headSprite.sprite = this.headSprite;
        torsoSprite.sprite = this.torsoSprite;
        rightLegSprite.sprite = this.rightLegSprite;
        leftLegSprite.sprite = this.leftLegSprite;
        rightHandSprite.sprite = this.rightHandSprite;
        leftHandSprite.sprite = this.leftHandSprite;
        if (this.tailSprite)
        {
            tailSprite.gameObject.SetActive(true);
            tailSprite.sprite = this.tailSprite;
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
