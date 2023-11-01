using UnityEngine;
public class HardGameMode : GameModeManager
{
    [SerializeField] int _currentLives;
    string _currentLivesName = "CurrentLives";

    public override void Start()
    {
        base.Start();
        _currentLives = Helpers.GameManager.SaveDataManager.GetInt(_currentLivesName, Helpers.GameManager.DefaultHardLives);
    }

    public override void PlayerDead(params object[] param)
    {
        _currentLives--;

        if (_currentLives <= 0)
        {
            Helpers.GameManager.SaveDataManager.SaveInt(_currentLivesName, Helpers.GameManager.DefaultHardLives);
            Helpers.GameManager.LoadSceneManager.LoadLevelAsync("Level 0.3", false);
        }
        else
        {
            Helpers.GameManager.SaveDataManager.SaveInt(_currentLivesName, _currentLives);
            Debug.Log("currentLives " + _currentLives);
        }

        playerHealth.EffectsOnDeath();
        playerHealth.RestartPosition();

    }
}