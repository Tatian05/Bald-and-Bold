using BaldAndBold.Consumables;
public class Consumable_NoRecoil : Consumables
{  
    public override void ConsumableAction(bool activate = true)
    {
        EventManager.TriggerEvent(Contains.CONSUMABLE_NO_RECOIL, activate);
    }
}
