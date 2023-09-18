public class Spawner_Sniper : Spawner_Enemy
{
    public override void SpawnEnemy(params object[] param)
    {
        FRY_Enemy_Sniper.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
