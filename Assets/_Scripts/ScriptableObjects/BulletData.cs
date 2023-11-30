using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "New Bullet Data")]
public class BulletData : ShoppableSO
{
    public override void Buy()
    {
        base.Buy();
        Helpers.PersistantData.persistantDataSaved.AddBullet(this);
    }
    public void SetShopBullet(Image previewImage) => previewImage.sprite = shopSprite;
}
