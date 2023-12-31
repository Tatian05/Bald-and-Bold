using BaldAndBold.Consumables;
using UnityEngine;
public class Consumable_Big_Bullet : Consumables
{
    [SerializeField] float _newBulletScale = 2;
    public override void ConsumableAction(bool activate)
    {
        base.ConsumableAction(activate);
        EventManager.TriggerEvent(Contains.CONSUMABLE_BIG_BULLET, activate, _newBulletScale);
    }
}
