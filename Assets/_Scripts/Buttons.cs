using UnityEngine;
public class Buttons : MonoBehaviour
{
    public void GoToMenu()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevelAsync("Menu", true);
    }
    public void Retry()
    {
        PlayerPrefs.DeleteAll();
        Helpers.GameManager.LoadSceneManager.ReloadLevel();
    }
    public void LoadScene(string sceneName)
    {
        Helpers.GameManager.LoadSceneManager.LoadLevelAsync(sceneName, true);
    }
    public void LoadScene(int sceneIndex)
    {
        Helpers.GameManager.LoadSceneManager.LoadLevelAsync(sceneIndex, true);
    }
}
