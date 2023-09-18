using UnityEngine;
public class GameModeManager : MonoBehaviour
{
    protected GameManager gameManager;
    protected PlayerHealth playerHealth;
    public virtual void Start()
    {
        gameManager = Helpers.GameManager;
        playerHealth = gameManager.Player.GetComponent<PlayerHealth>();

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, PlayerDead);
    }
    protected virtual void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, PlayerDead);
    }
    public virtual void PlayerDead(params object[] param)
    {

    }
}
