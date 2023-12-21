using UnityEngine;
using UnityEngine.UI;
public enum BulletType { Bullet, Grenade}

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "New Bullet Data")]
public class BulletData : ShoppableSO
{
    public BulletType bulletType;
    public override void OnStart()
    {
        base.OnStart();
        var bulletOrGranade = LanguageManager.Instance.GetTranslate(bulletType == BulletType.Bullet ? "ID_Bullet" : "ID_Grenade");
        shoppableTypeText = $"{LanguageManager.Instance.GetTranslate("ID_Cosmetic")} {bulletOrGranade}";
    }
    public void SetShopBullet(Image previewImage) => previewImage.sprite = shopSprite;
}
