using UnityEngine;
public class PlayerSetSkin : MonoBehaviour
{
    [SerializeField] SpriteRenderer _headSprite, _torsoSprite, _rightLegSprite, _leftLegSprite, _rightHandSprite, _leftHandSprite, _tailSprite;
    [SerializeField] Material _playerDashMaterial;
    void Start()
    {
        SetSkin();
        Helpers.GameManager.SetPlayerSkin += SetSkin;
    }

    public void SetSkin()
    {
        if (!Helpers.PersistantData.playerCosmeticEquiped) return;
        
        CosmeticData cosmetic = Helpers.PersistantData.playerCosmeticEquiped;
        _headSprite.sprite = cosmetic.shopSprite;
        _torsoSprite.sprite = cosmetic.torsoSprite;
        _rightLegSprite.sprite = cosmetic.rightLegSprite;
        _leftLegSprite.sprite = cosmetic.leftLegSprite;
        _rightHandSprite.sprite = cosmetic.rightHandSprite;
        _leftHandSprite.sprite = cosmetic.leftHandSprite;
        _tailSprite.sprite = cosmetic.tailSprite ? cosmetic.tailSprite : null;

        _playerDashMaterial.SetTexture("_MainTex", cosmetic.playerDashTexture.texture);
    }
}
