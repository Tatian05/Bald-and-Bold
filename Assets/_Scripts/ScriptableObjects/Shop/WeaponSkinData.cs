using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum FireWeaponType { Pistol, Rifle, Sniper, GrenadeLauncher }

[CreateAssetMenu(menuName = "New Weapon Skin", fileName = "New Weapon Skin")]
public class WeaponSkinData : ShoppableSO
{
    public Material mainMaterial;
    public Sprite mainSprite, weaponMask, handWeaponSprite;
    public FireWeaponType fireWeaponType;
    [HideInInspector] public string weaponTypeText;
    public override void OnStart()
    {
        base.OnStart();
        shoppableTypeText = $"{LanguageManager.Instance.GetTranslate("ID_Cosmetic")} {GetWeaponType(fireWeaponType)}";
    }
    string GetWeaponType(FireWeaponType weaponType) => weaponType switch
    {
        FireWeaponType.Pistol => LanguageManager.Instance.GetTranslate("ID_Pistol"),
        FireWeaponType.Rifle => LanguageManager.Instance.GetTranslate("ID_Rifle"),
        FireWeaponType.Sniper => LanguageManager.Instance.GetTranslate("ID_Sniper"),
        FireWeaponType.GrenadeLauncher => LanguageManager.Instance.GetTranslate("ID_GrenadeLauncher"),
        _ => throw new System.Exception($"{weaponType} not exist")
    };
    public void SetWeaponCosmetic(Image previewImage, TextMeshProUGUI weaponTypeTxt)
    {
        previewImage.sprite = shopSprite;
        weaponTypeTxt.text = weaponTypeText;
    }
}