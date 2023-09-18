using UnityEngine;
public abstract class Spawner_Enemy : MonoBehaviour
{
    protected void Start()
    {
        EventManager.SubscribeToEvent(Contains.WAIT_PLAYER_DEAD, SpawnEnemy);
        SpawnEnemy();
    }
    protected void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.WAIT_PLAYER_DEAD, SpawnEnemy);   
    }
    public abstract void SpawnEnemy(params object[] param);
}
