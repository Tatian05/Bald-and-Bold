using Droppables;
public class WeaponBoxDrop : Droppable
{
    protected override void DropObject()
    {
        Instantiate(GameManager.instance.DropManager.GetWeaponDrop()).SetPosition(transform.position + _offset);
    }
}
