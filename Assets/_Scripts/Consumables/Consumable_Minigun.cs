using BaldAndBold.Consumables;
public class Consumable_Minigun : Consumables
{
    public override void ConsumableAction(bool activate)
    {
        base.ConsumableAction(activate);
        EventManager.TriggerEvent(Contains.CONSUMABLE_MINIGUN, activate);
    }
}
