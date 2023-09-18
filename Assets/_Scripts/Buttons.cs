using UnityEngine;
public class Buttons : MonoBehaviour
{
    public void GoToMenu()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel("Menu");
    }
    public void Retry()
    {
        PlayerPrefs.DeleteAll();
        Helpers.GameManager.LoadSceneManager.ReloadLevel();
    }
    public void MinigamesRetry()
    {
        PlayerPrefs.DeleteAll();
        Helpers.GameManager.LoadSceneManager.LoadLevel("MiniGame 1 0");
    }
    public void LoadScene(string sceneName)
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel(sceneName);
    }
    public void LoadScene(int sceneIndex)
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel(sceneIndex);
    }
}
