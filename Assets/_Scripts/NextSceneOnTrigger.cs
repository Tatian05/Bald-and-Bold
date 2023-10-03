using UnityEngine;
public class NextSceneOnTrigger : MonoBehaviour
{
    [SerializeField] string _nextScene;
    private void OnTriggerEnter2D()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel(_nextScene);
        if (Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
