using UnityEngine.InputSystem;
using UnityEngine;
public class PlayerController
{
    Player _player;
    PlayerModel _playerModel;
    PlayerInputs _playerInputs;
    public float _xAxis { get; private set; }
    public float _yAxis { get; private set; }

    bool _fistInput = true;
    InputAction _movement, _jump, _dash;
    public PlayerController(Player player, PlayerModel playerModel)
    {
        _player = player;
        _playerModel = playerModel;
        _playerInputs = NewInputManager.PlayerInputs;

        _movement = _playerInputs.Player.Movement;
        _jump = _playerInputs.Player.Jump;
        _dash = _playerInputs.Player.Dash;
    }
    public void OnEnable()
    {
        _jump.performed += OnJump;
        _dash.performed += OnDash;

        _playerInputs.Enable();
    }
    public void OnDisable()
    {
        _jump.performed -= OnJump;
        _dash.performed -= OnDash;

        _playerInputs.Disable();
    }
    public void OnUpdate()
    {
        if (FirstInput())
        {
            Helpers.LevelTimerManager.StartLevelTimer();
            _fistInput = false;
        }

        _playerModel.OnUpdate();

        //_xAxis = _inputManager.GetAxisRaw("Horizontal");
        //_yAxis = _inputManager.GetAxisRaw("Vertical");
        _xAxis = _movement.ReadValue<Vector2>().x;
        _yAxis = _movement.ReadValue<Vector2>().y;

        _playerModel.LookAt(_xAxis);

        //if (_inputManager.GetButtonDown("Jump") && (_playerModel.CanJump || _playerModel.InRope)) { _player.ExitClimb(); _player.OnJump(); };

        //if (_inputManager.GetButtonDown("Dash") && _playerModel.CanDash) { _player.ExitClimb(); _player.SendInput(PlayerStates.Dash); };
    }
    public void OnFixedUpdate()
    {
        _player.OnMove(_xAxis, _yAxis);
    }
    bool FirstInput() => (_xAxis != 0 || _jump.IsPressed() || _dash.IsPressed() || _playerInputs.Player.Knife.IsPressed()) && _fistInput;

    public float YAxis() => _yAxis;

    public float XAxis() => _xAxis;

    void OnJump(InputAction.CallbackContext obj) { if (_playerModel.CanJump || _playerModel.InRope) { _player.ExitClimb(); _player.OnJump(); } }
    void OnDash(InputAction.CallbackContext obj) { if (_playerModel.CanDash) { _player.ExitClimb(); _player.SendInput(PlayerStates.Dash); } }
}
