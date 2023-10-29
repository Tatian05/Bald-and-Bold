using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RedButton : MonoBehaviour
{
    Collider2D _collider;
    Animator _anim;
    bool _isPlayerOnTrigger;
    ShowKeyUI _showKeyUI;
    PlayerInputs _playerInputs;
    InputAction _interact;

    //[SerializeField] GameObject lightParent;
    [SerializeField] SpriteRenderer _buttonSprite;
    private void Start()
    {
        _playerInputs = NewInputManager.PlayerInputs;
        _interact = _playerInputs.Player.Interact;
        EventManager.SubscribeToEvent(Contains.ON_ENEMIES_KILLED, ShowExit);
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
        Helpers.LevelTimerManager.RedButton += () => _anim.SetTrigger("Off");

        _interact.performed += OnPlayRedButton;

        _showKeyUI = GetComponentInChildren<ShowKeyUI>();
        _showKeyUI.gameObject.SetActive(false);
        _collider = GetComponent<Collider2D>();
        _anim = GetComponentInChildren<Animator>();
        StartCoroutine(HideExit());
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_ENEMIES_KILLED, ShowExit);
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
    }
    void OnPlayRedButton(InputAction.CallbackContext obj) { if (_isPlayerOnTrigger) EventManager.TriggerEvent(Contains.ON_ROOM_WON); }
    void OnPlayerDead(params object[] param) { StartCoroutine(HideExit()); }
    void ShowExit(params object[] param)
    {
        _anim.SetBool("IsOpen", true);
        _collider.enabled = true;
        _showKeyUI.gameObject.SetActive(true);
        //lightParent.SetActive(true);
        _buttonSprite.color = Color.red;
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        _collider.enabled = false;
        _showKeyUI.gameObject.SetActive(false);
        _anim.SetBool("IsOpen", false);
        //lightParent.SetActive(false);
        _buttonSprite.color = Color.white;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>()) _isPlayerOnTrigger = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>()) _isPlayerOnTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>()) _isPlayerOnTrigger = false;
    }
}
