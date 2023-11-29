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

        _headSprite.sprite = cosmetic.HeadSprite();
        _torsoSprite.sprite = cosmetic.TorsoSprite();
        _rightLegSprite.sprite = cosmetic.RightLegSprite();
        _leftLegSprite.sprite = cosmetic.LeftLegSprite();
        _rightHandSprite.sprite = cosmetic.RightHandSprite();
        _leftHandSprite.sprite = cosmetic.LeftHandSprite();
        _tailSprite.sprite = cosmetic.TailSprite() ? cosmetic.TailSprite() : null;
    }
}
