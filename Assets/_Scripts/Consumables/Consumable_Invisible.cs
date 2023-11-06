using BaldAndBold.Consumables;
public class Consumable_Invisible : Consumables
{
    protected override void ConsumableAction(bool activate)
    {
        EventManager.TriggerEvent(Contains.CONSUMABLE_INVISIBLE, activate);
    }
}
