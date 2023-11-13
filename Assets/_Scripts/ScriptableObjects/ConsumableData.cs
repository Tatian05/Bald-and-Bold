using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BaldAndBold.Consumables;

[CreateAssetMenu(fileName = "New Consumable Data", menuName = "New Consumable")]
public class ConsumableData : ShoppableSO
{
    [Header("Consumable Variables")]
    [HideInInspector] public string description;
    public Consumables consumablePrefab;
    public float consumableDuration;
    public string descriptionLocalizationID;
    public override void OnStart()
    {
        base.OnStart();
        description = LanguageManager.Instance.GetTranslate(descriptionLocalizationID);
    }
    public void SetConsumable(TextMeshProUGUI text, Image image) { text.text = description; image.sprite = shoppableData.shopSprite; }
    public override void Buy()
    {
        base.Buy();
        Helpers.PersistantData.persistantDataSaved.AddConsumable(this);
    }
}
