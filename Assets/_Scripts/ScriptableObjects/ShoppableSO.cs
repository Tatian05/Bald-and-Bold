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
    [System.NonSerialized] public string shoppableName;
    public string shopSpriteName;

    Sprite _shopSprite;
    public Sprite shopSprite => _shopSprite;
    protected virtual void OnEnable()
    {
        _shopSprite = Resources.Load<Sprite>($"ShopSprites/{shopSpriteName}");
    }
    public virtual void OnStart() { shoppableName = LanguageManager.Instance.GetTranslate(localizationID); }
    public virtual void Buy()
    {
        Helpers.PersistantData.persistantDataSaved.Buy(cost);
    }
}