public class Spawner_FollowDrone : Spawner_Enemy
{
    public override void SpawnEnemy(params object[] param)
    {
        FRY_Enemy_FollowDrone.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
