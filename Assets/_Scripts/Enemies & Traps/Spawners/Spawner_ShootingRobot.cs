using UnityEngine;
public class Spawner_ShootingRobot : Spawner_Enemy
{
    [SerializeField] bool _flip;
    public override void SpawnEnemy(params object[] param)
    {
        if (_enemy != null) return;

        _enemy = FRY_Enemy_ShootingRobot.Instance.pool.GetObject().SetPosition(transform.position).SetSpawner(this);
        if (_flip) (_enemy as Enemy_ShootingRobot).Flip(_flip);
    }
}
