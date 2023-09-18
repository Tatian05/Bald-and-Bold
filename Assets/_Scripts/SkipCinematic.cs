using UnityEngine;
public class SkipCinematic : MonoBehaviour
{
    [SerializeField] bool _principalCinematic;
    void Update()
    {
        if (Input.anyKey)
        {
            if (_principalCinematic) Helpers.GameManager.LoadSceneManager.LoadLevel("Level 0 Tutorial");
            else Helpers.GameManager.CinematicManager.SkipDefeatCinematic();
            gameObject.SetActive(false);
        }
    }
}
