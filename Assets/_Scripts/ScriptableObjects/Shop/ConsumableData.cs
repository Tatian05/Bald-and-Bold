using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BaldAndBold.Consumables;

[CreateAssetMenu(fileName = "New Consumable Data", menuName = "New Consumable")]
public class ConsumableData : ShoppableSO
{
    [Header("Consumable Variables")]
    [HideInInspector] public string description;
    public float consumableDuration;
    public string descriptionLocalizationID;
    public Consumables consumable;
    public override void OnStart()
    {
        base.OnStart();
        description = LanguageManager.Instance.GetTranslate(descriptionLocalizationID);
    }
    public void SetShopConsumable(TextMeshProUGUI descriptionTxt, TextMeshProUGUI durationTxt, Image image) {
        descriptionTxt.text = description;
        durationTxt.text = $"{consumableDuration}s";
        image.sprite = shopSprite;
    }
}
