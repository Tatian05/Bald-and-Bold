public class Spawner_FollowDrone : Spawner_Enemy
{
    public override void SpawnEnemy(params object[] param)
    {
        if (_enemy != null) return;
        _enemy = FRY_Enemy_FollowDrone.Instance.pool.GetObject().SetPosition(transform.position).SetSpawner(this);
    }
}
