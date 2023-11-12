using BaldAndBold.Consumables;
public class Consumable_Minigun : Consumables
{
    public override void ConsumableAction(bool activate)
    {
        EventManager.TriggerEvent(Contains.CONSUMABLE_MINIGUN, activate);
    }
}
