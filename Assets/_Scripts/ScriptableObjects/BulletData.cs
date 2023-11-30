using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "New Bullet Data")]
[System.Serializable]
public class BulletData : ShoppableSO
{
    Sprite _bulletX32;
    public Sprite bulletX32 => _bulletX32;
    protected override void OnEnable()
    {
        base.OnEnable();
        _bulletX32 = Sprite.Create(shopSprite.texture, shopSprite.rect, Vector2.one * .5f, 32);
    }
    public override void Buy()
    {
        base.Buy();
        Helpers.PersistantData.persistantDataSaved.AddBullet(this);
    }
    public void SetShopBullet(Image previewImage) => previewImage.sprite = shopSprite;
}
