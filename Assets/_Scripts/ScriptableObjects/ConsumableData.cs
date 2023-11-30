using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BaldAndBold.Consumables;

[CreateAssetMenu(fileName = "New Consumable Data", menuName = "New Consumable")]
[System.Serializable]
public class ConsumableData : ShoppableSO
{
    [Header("Consumable Variables")]
    [HideInInspector] public string description;
    public float consumableDuration;
    public string descriptionLocalizationID;

    Consumables _consumable;
    public Consumables Consumable() => _consumable;
    protected override void OnEnable()
    {
        base.OnEnable();
        _consumable = Resources.Load<Consumables>($"ConsumablePrefabs/Consumable_{name}");
    }
    public override void OnStart()
    {
        base.OnStart();
        description = LanguageManager.Instance.GetTranslate(descriptionLocalizationID);
    }
    public void SetShopConsumable(TextMeshProUGUI descriptionTxt, TextMeshProUGUI durationTxt, Image image) {
        descriptionTxt.text = description;
        durationTxt.text = $"{consumableDuration}s";
        image.sprite = shopSprite();
    }
    public override void Buy()
    {
        base.Buy();
        Helpers.PersistantData.persistantDataSaved.AddConsumable(this);
    }
}
