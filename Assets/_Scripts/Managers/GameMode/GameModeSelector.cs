using UnityEngine;
public enum GameMode
{
    EasyGameMode = 0,
    HardGameMode = 1
}
public class GameModeSelector : MonoBehaviour
{
    [SerializeField] GameModeManager easyGameMode, hardGameMode;

    int _myGameMode;
    private void Start()
    {
        _myGameMode = Helpers.PersistantData.gameData.gameMode;

         switch(_myGameMode)
         {
             case (int)GameMode.EasyGameMode:
                 Instantiate(easyGameMode, transform);
                 break;
             case (int)GameMode.HardGameMode:
                 Instantiate(hardGameMode, transform);
                 break;
             default:
                 Debug.LogWarning("GameMode NOT FOUND");
                 break;
         }
    }
}
