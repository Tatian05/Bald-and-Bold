using Droppables;
public class WeaponBoxDrop : Droppable
{
    protected override void DropObject()
    {
        Instantiate(Helpers.GameManager.DropManager.GetWeaponDrop()).SetPosition(transform.position + _offset);
    }
}
