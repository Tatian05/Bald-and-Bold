public class Spawner_Ground_Ak : Spawner_Enemy
{
    public override void SpawnEnemy(params object[] param)
    {
        if (_enemy != null) return;

        _enemy = FRY_Enemy_Ground_Ak.Instance.pool.GetObject().SetPosition(transform.position).SetSpawner(this);
    }
}
