using UnityEngine;
using BaldAndBold.Consumables;
public class Consumable_Cadence : Consumables
{
    [SerializeField] float _cadenceMultiplier = 2;
    public override void ConsumableAction(bool activate)
    {
        base.ConsumableAction(activate);
        EventManager.TriggerEvent(Contains.CONSUMABLE_CADENCE, activate, _cadenceMultiplier);
    }
}