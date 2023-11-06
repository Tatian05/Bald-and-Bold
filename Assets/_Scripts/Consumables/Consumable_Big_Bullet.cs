using BaldAndBold.Consumables;
using UnityEngine;
public class Consumable_Big_Bullet : Consumables
{
    [SerializeField] float _newBulletScale = 2;
    protected override void ConsumableAction(bool activate)
    {
        EventManager.TriggerEvent(Contains.CONSUMABLE_BIG_BULLET, activate, _newBulletScale);
    }
}
