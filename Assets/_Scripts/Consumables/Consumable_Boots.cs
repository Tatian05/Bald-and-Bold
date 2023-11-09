using BaldAndBold.Consumables;
public class Consumable_Boots : Consumables
{
    protected override void ConsumableAction(bool activate)
    {
        EventManager.TriggerEvent(Contains.CONSUMABLE_BOOTS, activate);
    }
}
