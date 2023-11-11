using BaldAndBold.Consumables;
public class Consumable_Boots : Consumables
{
    public override void ConsumableAction(bool activate)
    {
        EventManager.TriggerEvent(Contains.CONSUMABLE_BOOTS, activate);
    }
}
