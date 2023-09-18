using Droppables;
public class PickUpDrop : Droppable
{
    protected override void DropObject()
    {
        FRY_PickUps.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
