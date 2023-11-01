using UnityEngine;
public class OutroScene : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Outro), 3f);
    }
    void Outro() { Helpers.GameManager.LoadSceneManager.LoadLevelAsync("Menu", true); }
}
