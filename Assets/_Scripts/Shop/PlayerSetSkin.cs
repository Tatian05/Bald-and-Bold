using UnityEngine;
public class PlayerSetSkin : MonoBehaviour
{
    [SerializeField] SpriteRenderer _headSprite, _torsoSprite, _rightLegSprite, _leftLegSprite, _rightHandSprite, _leftHandSprite, _tailSprite;
    void Start()
    {
        SetSkin();
        Helpers.GameManager.SetPlayerSkin += SetSkin;
    }

    public void SetSkin()
    {
        if (!Helpers.PersistantData.persistantDataSaved.playerCosmeticEquiped) return;
        
        CosmeticData cosmetic = Helpers.PersistantData.persistantDataSaved.playerCosmeticEquiped;
        _headSprite.sprite = cosmetic.HeadSprite;
        _torsoSprite.sprite = cosmetic.TorsoSprite;
        _rightLegSprite.sprite = cosmetic.RightLegSprite;
        _leftLegSprite.sprite = cosmetic.LeftLegSprite;
        _rightHandSprite.sprite = cosmetic.RightHandSprite;
        _leftHandSprite.sprite = cosmetic.LeftHandSprite;
        _tailSprite.sprite = cosmetic.TailSprite ? cosmetic.TailSprite : null;
    }
}
