using BaldAndBold.Consumables;
public class Consumable_Boots : Consumables
{
    public override void ConsumableAction(bool activate)
    {
        base.ConsumableAction(activate);
        EventManager.TriggerEvent(Contains.CONSUMABLE_BOOTS, activate);
        Helpers.PersistantData.consumablesData.boots = activate;
    }
}
