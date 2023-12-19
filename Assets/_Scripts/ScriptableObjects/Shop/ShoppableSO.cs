using UnityEngine;
public enum ShoppableType { Cosmetic, Consumable, Bullet, Weapon }
public enum ShoppableQuality { Normal, Rare, Epic }
public class ShoppableSO : ScriptableObject
{
    [Header("Base Shoppable Variables")]
    public ShoppableType shoppableType;
    public ShoppableQuality shoppableQuality;
    public int ID, cost;
    public string localizationID;
    public Sprite shopSprite;
    [HideInInspector] public string shoppableName;
    public virtual void OnStart()
    {
        shoppableName = LanguageManager.Instance.GetTranslate(localizationID);
    }
    public void Buy()
    {
        Helpers.PersistantData.persistantDataSaved.Buy(cost);
        Helpers.PersistantData.AddShoppableToCollection(this);
    }

#if UNITY_EDITOR

    [ContextMenu("ForceSave")]

    private void ForceSave()

    {

        UnityEditor.EditorUtility.SetDirty(this);

        UnityEditor.AssetDatabase.SaveAssets();

    }

#endif
}