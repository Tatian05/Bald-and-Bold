using BaldAndBold.Consumables;
using System.Collections.Generic;
using UnityEngine;
public class ConsumablesCanvasContainer : MonoBehaviour
{
    [SerializeField] ConsumablesUI _consumableUIPrefab;
    [SerializeField] Transform _consumablesContainer;

    List<ConsumablesUI> _consumablesActivated = new List<ConsumablesUI>();
    void Start()
    {
        EventManager.SubscribeToEvent(Contains.CONSUMABLE, ActivateConsumableUI);
        
        var consumablesActive = Helpers.PersistantData.consumablesActivated.DictioraryFromTwoLists(Helpers.PersistantData.consumablesActivatedTime);

        if (consumablesActive.Count > 0)
            foreach (var item in consumablesActive)
                ActivateConsumableUI(item.Key.consumable, item.Value);
    }
    public void ActivateConsumableUI(params object[] param)
    {
        var consumableUI = Instantiate(_consumableUIPrefab);
        _consumablesActivated.Add(consumableUI);
        consumableUI.SetConsumable((Consumables)param[0]).SetTime((float)param[1]).SetParent(_consumablesContainer);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE, ActivateConsumableUI);
    }
}
