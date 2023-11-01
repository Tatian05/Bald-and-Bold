using UnityEngine;
public class SkipCinematic : MonoBehaviour
{
    [SerializeField] bool _principalCinematic;
    void Update()
    {
        if (Input.anyKey)
        {
            if (_principalCinematic) Helpers.GameManager.LoadSceneManager.LoadLevelAsync("Level 0 Tutorial", true);
            else Helpers.GameManager.CinematicManager.SkipDefeatCinematic();
            gameObject.SetActive(false);
        }
    }
}
