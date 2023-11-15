using BaldAndBold.Consumables;
public class Consumable_NoRecoil : Consumables
{  
    public override void ConsumableAction(bool activate)
    {
        base.ConsumableAction(activate);
        EventManager.TriggerEvent(Contains.CONSUMABLE_NO_RECOIL, activate);
    }
}
