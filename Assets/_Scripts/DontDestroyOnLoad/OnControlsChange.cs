using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class OnControlsChange : MonoBehaviour
{
    public static OnControlsChange Instance { get; private set; }

    PlayerInput _playerInput;
    public string GAMEPAD_SCHEME = "Gamepad";
    public string KEYBOARD_MOUSE = "Keyboard&Mouse";
    public string CurrentControl = string.Empty;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        _playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(Contains.ON_CONTROLS_CHANGED, OnControlsChanged);

        _playerInput.onControlsChanged += TriggerOnControlsChange;
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_CONTROLS_CHANGED, OnControlsChanged);

        _playerInput.onControlsChanged -= TriggerOnControlsChange;
    }
    public void TriggerOnControlsChange(PlayerInput obj)
    {
        EventManager.TriggerEvent(Contains.ON_CONTROLS_CHANGED, _playerInput.currentControlScheme);
    }
    void OnControlsChanged(params object[] param)
    {
        CurrentControl = (string)param[0];
        Cursor.visible = CurrentControl == KEYBOARD_MOUSE ? true : false;
    }
}
