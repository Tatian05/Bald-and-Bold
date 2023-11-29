using UnityEngine;
public enum ShoppableType { Cosmetic, Consumable, Bullet }
public enum ShoppableQuality { Normal, Rare, Epic }

[System.Serializable]
public class ShoppableSO : ScriptableObject
{
    [Header("Base Shoppable Variables")]
    public ShoppableType shoppableType;
    public ShoppableQuality shoppableQuality;
    public int cost;
    public string localizationID;
    public string shopSpriteName;

    string _shoppableName;
    Sprite _shopSprite;
    public Sprite shopSprite() => _shopSprite;
    public string shoppableName => _shoppableName;
    public virtual void OnStart()
    {
        _shoppableName = LanguageManager.Instance.GetTranslate(localizationID);
        if (_shopSprite != null) return;
        _shopSprite = Resources.Load<Sprite>($"ShopSprites/{shopSpriteName}");
    }
    public virtual void Buy()
    {
        Helpers.PersistantData.persistantDataSaved.Buy(cost);
    }
}