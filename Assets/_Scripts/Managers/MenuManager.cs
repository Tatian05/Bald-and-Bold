using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class MenuManager : MonoBehaviour
{
    [SerializeField] PlayableDirector _trainCinematic;
    [SerializeField] Button _continueButton;
    [SerializeField] GameObject _warningWindow;
    private void Start()
    {
        _continueButton.gameObject.SetActive(!Helpers.PersistantData.gameData.firstTime);
    }
    public void BTN_NewGame()
    {
        if (Helpers.PersistantData.gameData.firstTime) Play();
        else _warningWindow.SetActive(true);
    }
    public void Play()
    {
        Helpers.PersistantData.DeletePersistantData();
        PlayerPrefs.DeleteAll();
        _trainCinematic.Play();
        Helpers.PersistantData.gameData.firstTime = false;
    }
    public void BTN_Credits()
    {

    }
    public void BTN_Quit()
    {
        Application.Quit();
    }

    public void BTN_GoToMenu()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel("Menu");
    }
    public void BTN_Tutorial()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel("Tutorial1");
        Helpers.GameManager.SaveDataManager.Reset();
    }
}
