using UnityEngine;
public enum ShoppableType { Cosmetic, Consumable, BulletTrail }
public enum ShoppableQuality { Normal, Rare, Epic }
public abstract class ShoppableSO : ScriptableObject, IShoppable
{
    [Header("Base Shoppable Variables")]
    public ShoppableData shoppableData;
    Sprite _shopSprite;
    public Sprite shopSprite => _shopSprite;
    public virtual void OnStart()
    {
        shoppableData.shoppableName = LanguageManager.Instance.GetTranslate(shoppableData.localizationID);
        _shopSprite = Resources.Load<Sprite>($"ShopSprites/{shoppableData.shopSpriteName}");
    }
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