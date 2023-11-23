using BaldAndBold.Consumables;
using UnityEngine;
public class ConsumablesCanvasContainer : MonoBehaviour
{
    [SerializeField] ConsumablesUI _consumableUIPrefab;
    [SerializeField] Transform _consumablesContainer;
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(Contains.CONSUMABLE, ActivateConsumableUI);        
    }
    void Start()
    {
        var consumablesActive = Helpers.PersistantData.consumablesData.LoadActivesConsumables();

        if (consumablesActive.Count > 0)
            foreach (var item in consumablesActive)
                ActivateConsumableUI(item.Key.Consumable, item.Value);
    }
    public void ActivateConsumableUI(params object[] param)
    {
        var consumableUI = Instantiate(_consumableUIPrefab);
        consumableUI.SetConsumable((Consumables)param[0]).SetTime((float)param[1]).SetParent(_consumablesContainer);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE, ActivateConsumableUI);
    }
}
