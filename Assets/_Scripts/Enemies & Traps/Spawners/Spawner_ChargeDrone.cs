public class Spawner_ChargeDrone : Spawner_Enemy
{
    public override void SpawnEnemy(params object[] param)
    {
        FRY_Enemy_ChargeDrone.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
