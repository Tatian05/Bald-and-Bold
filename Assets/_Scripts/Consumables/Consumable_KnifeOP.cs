using UnityEngine;
using BaldAndBold.Consumables;
public class Consumable_KnifeOP : Consumables
{
    [SerializeField] float _knifeScaleMultiplier = 3;
    public override void ConsumableAction(bool activate)
    {
        EventManager.TriggerEvent(Contains.CONSUMABLE_MELEE, activate, _knifeScaleMultiplier);
    }
}
