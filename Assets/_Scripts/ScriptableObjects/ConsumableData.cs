using UnityEngine;
using TMPro;
[CreateAssetMenu(fileName = "New Consumable Data", menuName = "New Consumable")]
public class ConsumableData : ShoppableSO
{
    [HideInInspector] public string description;
    public float consumableDuration;
    public string descriptionLocalizationID;
    public override void OnStart()
    {
        base.OnStart();
        description = LanguageManager.Instance.GetTranslate(descriptionLocalizationID);
    }

    public void SetConsumable(TextMeshProUGUI text) { text.text = description; }
    public override void Buy()
    {
        base.Buy();
        Helpers.PersistantData.persistantDataSaved.AddConsumable(this);
    }
}
