using UnityEngine;
public enum ShoppableType { Cosmetic, Consumable, BulletTrail }
public enum ShoppableQuality { Normal, Rare, Epic }

[System.Serializable]
public abstract class ShoppableSO : ScriptableObject, IShoppable
{
    [Header("Base Shoppable Variables")]
    public ShoppableData shoppableData;
    Sprite _shopSprite;
    public Sprite shopSprite => _shopSprite;
    protected virtual void OnEnable()
    {
        _shopSprite = Resources.Load<Sprite>($"ShopSprites/{shoppableData.shopSpriteName}");
    }
    public virtual void OnStart() { shoppableData.shoppableName = LanguageManager.Instance.GetTranslate(shoppableData.localizationID); }
    public virtual void Buy()
    {
        Helpers.PersistantData.persistantDataSaved.Buy(shoppableData.cost);
    }
}
[System.Serializable]
public struct ShoppableData
{
    public ShoppableType shoppableType;
    public ShoppableQuality shoppableQuality;
    public int cost;
    public string localizationID;
    [System.NonSerialized] public string shoppableName;
    public string shopSpriteName;
}