public class Spawner_KamikazeRobot : Spawner_Enemy
{
    public override void SpawnEnemy(params object[] param)
    {
        FRY_Enemy_KamikazeRobot.Instance.pool.GetObject().SetPosition(transform.position);
    }
}

