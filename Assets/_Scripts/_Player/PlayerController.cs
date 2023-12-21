using UnityEngine.InputSystem;
using UnityEngine;
public class PlayerController
{
    Player _player;
    PlayerModel _playerModel;
    PlayerInputs _playerInputs;

    Vector2 _movementAxis;
    bool _fistInput = true;
    InputAction _movement, _jump, _dash;
    public float YAxis => _movementAxis.y;
    public float XAxis => _movementAxis.x;
    public PlayerController(Player player, PlayerModel playerModel)
    {
        _player = player;
        _playerModel = playerModel;
        _playerInputs = NewInputManager.PlayerInputs;

        _movement = _playerInputs.Player.Movement;
        _jump = _playerInputs.Player.Jump;
        _dash = _playerInputs.Player.Dash;
    }
    public void OnUpdate()
    {
        if (FirstInput() && Helpers.LevelTimerManager)
        {
            Helpers.LevelTimerManager.StartLevelTimer();
            _fistInput = false;
        }
        _playerModel.OnUpdate();

        _movementAxis = new Vector2 { x = Mathf.RoundToInt(_movement.ReadValue<Vector2>().x), y = Mathf.RoundToInt(_movement.ReadValue<Vector2>().y) };
    }
    public void OnFixedUpdate()
    {
        _player.OnMove(_movementAxis.x, _movementAxis.y);
    }
    bool FirstInput() => (_movementAxis.x != 0 || _jump.IsPressed() || _dash.IsPressed() || _playerInputs.Player.Knife.IsPressed()) && _fistInput;
    public void OnEnable()
    {
        _jump.performed += OnJump;
        _dash.performed += OnDash;

        _playerInputs.Enable();
    }
    public void OnDestroy()
    {
        _jump.performed -= OnJump;
        _dash.performed -= OnDash;

        _playerInputs.Disable();
    }
    void OnJump(InputAction.CallbackContext obj) { if (_playerModel.CanJump || _playerModel.InRope) { _player.ExitClimb(); _player.OnJump(); } }
    void OnDash(InputAction.CallbackContext obj) { if (_playerModel.CanDash) { _player.ExitClimb(); _player.SendInput(PlayerStates.Dash); } }
}
