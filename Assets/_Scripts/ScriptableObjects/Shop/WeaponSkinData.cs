using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum FireWeaponType { Pistol, Rifle, Sniper, GrenadeLauncher }

[CreateAssetMenu(menuName = "New Weapon Skin", fileName = "New Weapon Skin")]
public class WeaponSkinData : ShoppableSO
{
    public Sprite mainSprite, selectedWeaponSprite, handWeaponSprite;
    public FireWeaponType fireWeaponType;
    [HideInInspector] public string weaponTypeText;
    public override void OnStart()
    {
        base.OnStart();
        weaponTypeText = LanguageManager.Instance.GetTranslate($"ID_{fireWeaponType}");
    }
    public void SetWeaponCosmetic(Image previewImage, TextMeshProUGUI weaponTypeTxt)
    {
        previewImage.sprite = shopSprite;
        weaponTypeTxt.text = weaponTypeText;
    }
}