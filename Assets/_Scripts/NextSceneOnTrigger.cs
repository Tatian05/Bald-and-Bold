using UnityEngine;
using System.Collections;
public class NextSceneOnTrigger : MonoBehaviour
{
    [SerializeField] string _nextScene;
    [SerializeField] GameObject lightParent;
    [SerializeField] Animator _anim;

    Collider2D _collider;
    private void Start()
    {
        EventManager.SubscribeToEvent(Contains.ON_ROOM_WON, ShowExit);
        EventManager.SubscribeToEvent(Contains.WAIT_PLAYER_DEAD, StartHideExit);

        _collider = GetComponent<Collider2D>();
        StartCoroutine(HideExit());
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_ROOM_WON, ShowExit);
        EventManager.UnSubscribeToEvent(Contains.WAIT_PLAYER_DEAD, StartHideExit);
    }
    void StartHideExit(params object[] param) { StartCoroutine(HideExit()); }
    void ShowExit(params object[] param)
    {
        _anim.SetBool("IsOpen", true);
        _collider.enabled = true;
        lightParent.SetActive(true);
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        lightParent.SetActive(false);
        _collider.enabled = false;
        _anim.SetBool("IsOpen", false);

    }
    private void OnTriggerEnter2D()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel(_nextScene);
        if (Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
