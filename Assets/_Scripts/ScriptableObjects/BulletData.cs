using UnityEngine;
using UnityEngine.UI;
public enum BulletType { Bullet, Grenade}

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "New Bullet Data")]
public class BulletData : ShoppableSO
{
    public BulletType bulletType;
    public void SetShopBullet(Image previewImage) => previewImage.sprite = shopSprite;
}
