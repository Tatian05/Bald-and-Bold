public class Spawner_KamikazeRobot : Spawner_Enemy
{
    public override void SpawnEnemy(params object[] param)
    {
        if (_enemy != null) return;

        _enemy = FRY_Enemy_KamikazeRobot.Instance.pool.GetObject().SetPosition(transform.position).SetSpawner(this);
    }
}

