using UnityEngine;
public enum ShoppableType { Cosmetic, Consumable, Bullet }
public enum ShoppableQuality { Normal, Rare, Epic }
public class ShoppableSO : ScriptableObject
{
    [Header("Base Shoppable Variables")]
    public ShoppableType shoppableType;
    public ShoppableQuality shoppableQuality;
    public int cost;
    public string localizationID;
    public Sprite shopSprite;
    [HideInInspector] public string shoppableName;
    public virtual void OnStart()
    {
        shoppableName = LanguageManager.Instance.GetTranslate(localizationID);
    }
    public virtual void Buy()
    {
        Helpers.PersistantData.persistantDataSaved.Buy(cost);
    }
}