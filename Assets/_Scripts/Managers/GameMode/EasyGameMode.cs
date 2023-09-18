public class EasyGameMode : GameModeManager
{
    public override void PlayerDead(params object[] param)
    {
        playerHealth.EffectsOnDeath();
        playerHealth.RestartPosition();
    }
}
