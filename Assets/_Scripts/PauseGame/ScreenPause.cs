using UnityEngine;
using UnityEngine.UI;
public class ScreenPause : MonoBehaviour, IScreen
{
    [SerializeField] Button _resumeButton;
    private void Start()
    {
        _resumeButton.onClick.AddListener(Resume);
    }
    public void Resume()
    {
        Helpers.GameManager.PauseManager.Pop();
        Helpers.GameManager.PauseManager.TurnPause();
    }
    public void Activate(){}

    public void Deactivate(){}
    public void PauseObjectsInCinematic(){}
    public void Free(){ Destroy(gameObject);}

    public void UnpauseObjectsInCinematic(){}
}
