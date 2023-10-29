using System.Collections;
using UnityEngine;
public class Door : MonoBehaviour
{
    [SerializeField] GameObject lightParent;
    [SerializeField] Animator _anim;
    private void Start()
    {
        EventManager.SubscribeToEvent(Contains.ON_ENEMIES_KILLED, ShowExit);
        EventManager.SubscribeToEvent(Contains.WAIT_PLAYER_DEAD, StartHideExit);

        StartCoroutine(HideExit());
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_ENEMIES_KILLED, ShowExit);
        EventManager.UnSubscribeToEvent(Contains.WAIT_PLAYER_DEAD, StartHideExit);
    }
    void StartHideExit(params object[] param) { StartCoroutine(HideExit()); }
    void ShowExit(params object[] param)
    {
        _anim.SetBool("IsOpen", true);
        lightParent.SetActive(true);
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        lightParent.SetActive(false);
        _anim.SetBool("IsOpen", false);
    }
}
