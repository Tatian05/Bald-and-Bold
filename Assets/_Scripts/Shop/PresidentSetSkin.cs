using UnityEngine;
public class PresidentSetSkin : MonoBehaviour
{
    [SerializeField] SpriteRenderer _headSprite, _torsoSprite, _rightLegSprite, _leftLegSprite, _rightHandSprite, _leftHandSprite, _tailSprite;
    [SerializeField] Material _presidentLeftHeadMaterial, _presidentRightHeadMaterial, _presidentLeftChestMaterial, _presidentRightChestMaterial;
    void Start()
    {
        SetSkin();
        Helpers.GameManager.SetPresidentSkin += SetSkin;
    }
    public void SetSkin()
    {
        if (!Helpers.PersistantData.presidentCosmeticEquiped) return;

        CosmeticData cosmetic = Helpers.PersistantData.presidentCosmeticEquiped;

        _headSprite.sprite = cosmetic.shopSprite;
        _torsoSprite.sprite = cosmetic.torsoSprite;
        _rightLegSprite.sprite = cosmetic.rightLegSprite;
        _leftLegSprite.sprite = cosmetic.leftLegSprite;
        _rightHandSprite.sprite = cosmetic.rightHandSprite;
        _leftHandSprite.sprite = cosmetic.leftHandSprite;
        _tailSprite.sprite = cosmetic.tailSprite ? cosmetic.tailSprite : null;

        _presidentLeftHeadMaterial.SetTexture("_MainTex", cosmetic.shopSprite.texture);
        _presidentRightHeadMaterial.SetTexture("_MainTex", cosmetic.shopSprite.texture);
        _presidentLeftChestMaterial.SetTexture("_MainTex", cosmetic.torsoSprite.texture);
        _presidentRightChestMaterial.SetTexture("_MainTex", cosmetic.torsoSprite.texture);
    }
}
