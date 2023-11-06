using UnityEngine;
public class Spawner_ShootingRobot : Spawner_Enemy
{
    [SerializeField] bool _flip;

    public override void SpawnEnemy(params object[] param)
    {
        var shootingRobot = (Enemy_ShootingRobot)FRY_Enemy_ShootingRobot.Instance.pool.GetObject().SetPosition(transform.position);
        if (_flip) shootingRobot.Flip(_flip);
        else FRY_Enemy_ShootingRobot.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
