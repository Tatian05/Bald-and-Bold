using UnityEngine;
public class PresidentSetSkin : MonoBehaviour
{
    [SerializeField] SpriteRenderer _headSprite, _torsoSprite, _rightLegSprite, _leftLegSprite, _rightHandSprite, _leftHandSprite, _tailSprite;
    void Start()
    {
        SetSkin();
        Helpers.GameManager.SetPresidentSkin += SetSkin;
    }
    public void SetSkin()
    {
        if (!Helpers.PersistantData.persistantDataSaved.presidentCosmeticEquiped) return;

        CosmeticData cosmetic = Helpers.PersistantData.persistantDataSaved.presidentCosmeticEquiped;

        _headSprite.sprite = cosmetic.headSprite;
        _torsoSprite.sprite = cosmetic.torsoSprite;
        _rightLegSprite.sprite = cosmetic.rightLegSprite;
        _leftLegSprite.sprite = cosmetic.leftLegSprite;
        _rightHandSprite.sprite = cosmetic.rightHandSprite;
        _leftHandSprite.sprite = cosmetic.leftHandSprite;
        _tailSprite.sprite = cosmetic.tailSprite ? cosmetic.tailSprite : null;
    }
}
