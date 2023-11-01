using UnityEngine;
public class NextSceneOnTrigger : MonoBehaviour
{
    [SerializeField] string _nextScene;
    private void OnTriggerEnter2D()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevelAsync(_nextScene, false);
        if (Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
