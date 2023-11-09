using UnityEngine;
using BaldAndBold.Consumables;
public class Consumable_Minigun : Consumables
{
    [SerializeField] FireWeapon _minigunPrefab;
    protected override void ConsumableAction(bool activate)
    {
        EventManager.TriggerEvent(Contains.CONSUMABLE_MINIGUN, activate, _minigunPrefab);
    }
}
