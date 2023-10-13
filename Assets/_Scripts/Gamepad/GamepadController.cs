using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadController : MonoBehaviour
{
    PlayerInput _playerInput;
    Mouse _virtualMouse, _currentMouse;
    RectTransform _virtualCursorRectTransform, _canvasRectTransform;
    Canvas _canvas;
    Camera _mainCamera;
    GameManager _gameManager;
    const string GAMEPAD_SCHEME = "Gamepad";
    const string KEYBOARD_MOUSE = "Keyboard&Mouse";
    string _previousControlScheme = string.Empty;
    Vector2 _gamepadCursor;

    [SerializeField] float _cursorSpeed = 1000f;

    public Vector2 GamepadCursor { get { return _gamepadCursor; } private set { } }
    private void OnEnable()
    {
        _gameManager = Helpers.GameManager;
        _playerInput = GetComponent<PlayerInput>();
        _virtualCursorRectTransform = _gameManager.gamepadRectTransform;
        _canvasRectTransform = _gameManager.canvasRectTransform;
        _canvas = _gameManager.gamepadCanvas;
        _mainCamera = Helpers.MainCamera;

        if (_virtualMouse == null) _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        else if (!_virtualMouse.added) InputSystem.AddDevice(_virtualMouse);

        _currentMouse = Mouse.current;
        InputUser.PerformPairingWithDevice(_virtualMouse, _playerInput.user);

        if (_virtualCursorRectTransform)
        {
            Vector2 position = _virtualCursorRectTransform.anchoredPosition;
            InputState.Change(_virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        _playerInput.onControlsChanged += OnControlChanged;
    }
    private void OnDisable()
    {
        if (_virtualMouse != null && _virtualMouse.added) InputSystem.RemoveDevice(_virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        _playerInput.onControlsChanged -= OnControlChanged;
    }
    void UpdateMotion()
    {
        if (_virtualMouse == null || Gamepad.current == null) return;
        Vector2 deltaValue = Gamepad.current.rightStick.ReadValue();
        deltaValue.Normalize();
        deltaValue *= _cursorSpeed * Time.unscaledDeltaTime;

        Vector2 currentPosition = _virtualMouse.position.ReadValue();
        _gamepadCursor = currentPosition + deltaValue;

        _gamepadCursor.x = Mathf.Clamp(_gamepadCursor.x, 0, Screen.width);
        _gamepadCursor.y = Mathf.Clamp(_gamepadCursor.y, 0, Screen.height);
        
        InputState.Change(_virtualMouse.position, _gamepadCursor);
        InputState.Change(_virtualMouse.delta, deltaValue);

        AnchorCursor(_gamepadCursor);
    }

    void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, position, _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera, out anchoredPosition);
        _virtualCursorRectTransform.anchoredPosition = anchoredPosition;
    }
    void OnControlChanged(PlayerInput obj)
    {
        if (_playerInput.currentControlScheme == KEYBOARD_MOUSE && _previousControlScheme != KEYBOARD_MOUSE)
        {
            InputSystem.onAfterUpdate -= UpdateMotion;
            Debug.Log(KEYBOARD_MOUSE);
            if(_virtualCursorRectTransform.gameObject.activeSelf) _virtualCursorRectTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
            _previousControlScheme = KEYBOARD_MOUSE;
        }
        else if(_playerInput.currentControlScheme == GAMEPAD_SCHEME && _previousControlScheme != GAMEPAD_SCHEME)
        {
            InputSystem.onAfterUpdate += UpdateMotion;
            Debug.Log(GAMEPAD_SCHEME);
            _virtualCursorRectTransform.gameObject.SetActive(true);
            Cursor.visible = false;
            InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
            AnchorCursor(_currentMouse.position.ReadValue());
            _previousControlScheme = GAMEPAD_SCHEME;
        }

        EventManager.TriggerEvent(Contains.ON_CONTROLS_CHANGED, _playerInput.currentControlScheme);
        Debug.Log(_playerInput.currentControlScheme);
    }
}
