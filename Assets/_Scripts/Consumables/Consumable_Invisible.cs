using BaldAndBold.Consumables;
public class Consumable_Invisible : Consumables
{
    public override void ConsumableAction(bool activate)
    {
        base.ConsumableAction(activate);
        EventManager.TriggerEvent(Contains.CONSUMABLE_INVISIBLE, activate);
    }
}
