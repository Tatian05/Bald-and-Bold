using UnityEngine;
public abstract class Spawner_Enemy : MonoBehaviour
{
    protected Enemy _enemy;
    public System.Action onEnemyDeath;
    protected void Start()
    {
        EventManager.SubscribeToEvent(Contains.WAIT_PLAYER_DEAD, SpawnEnemy);
        SpawnEnemy();
        onEnemyDeath += OnEnemyDeath;
    }
    protected void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.WAIT_PLAYER_DEAD, SpawnEnemy);
        onEnemyDeath -= OnEnemyDeath;
    }
    public abstract void SpawnEnemy(params object[] param);

    void OnEnemyDeath()
    {
        _enemy = null;
    }
}
