using UnityEngine;
using BaldAndBold.Consumables;
public class ConsumablesCanvasContainer : MonoBehaviour
{
    [SerializeField] ConsumablesUI _consumableUIPrefab;
    [SerializeField] Transform _consumablesContainer;
    private void Start()
    {
        EventManager.SubscribeToEvent(Contains.CONSUMABLE, ActivateConsumableUI);
    }
    public void ActivateConsumableUI(params object[] param)
    {
        var consumableUI = Instantiate(_consumableUIPrefab);
        consumableUI.SetConsumable((Consumables)param[0]).SetParent(_consumablesContainer);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE, ActivateConsumableUI);
    }
}
