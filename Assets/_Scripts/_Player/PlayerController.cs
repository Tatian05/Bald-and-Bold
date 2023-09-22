public class PlayerController : IController
{
    Player _player;
    PlayerModel _playerModel;
    InputManager _inputManager;
    public float _xAxis { get; private set; }
    public float _yAxis { get; private set; }

    bool _fistInput = true;
    public PlayerController(Player player, PlayerModel playerModel)
    {
        _player = player;
        _playerModel = playerModel;

        _inputManager = InputManager.Instance;
    }

    public void OnUpdate()
    {
        if (FirstInput())
        {
            Helpers.LevelTimerManager.StartLevelTimer();
            _fistInput = false;
        }

        _playerModel.OnUpdate();

        _xAxis = _inputManager.GetAxisRaw("Horizontal");
        _yAxis = _inputManager.GetAxisRaw("Vertical");

        if (_inputManager.GetButtonDown("Jump") && _playerModel.CanJump) _player.OnJump();

        if (_inputManager.GetButtonDown("Dash") && _playerModel.CanDash) _player.SendInput(PlayerStates.Dash);
    }
    public void OnFixedUpdate()
    {
        _player.OnMove(_xAxis);
    }
    bool FirstInput() => (_xAxis != 0 || _inputManager.GetButtonDown("Jump") || _inputManager.GetButtonDown("Dash") || _inputManager.GetButtonDown("Knife")) && _fistInput;

    public float YAxis() => _yAxis;

    public float XAxis() => _xAxis;
}
public class ClimbController : IController
{
    Player _player;
    PlayerModel _playerModel;
    InputManager _inputManager;
    public float _xAxis { get; private set; }
    public float _yAxis { get; private set; }
    public ClimbController(Player player, PlayerModel playerModel)
    {
        _player = player;
        _playerModel = playerModel;
        _inputManager = InputManager.Instance;

        _playerModel.ResetStats();
        _playerModel.CeroGravity();
    }

    public void OnUpdate()
    {
        _yAxis = _inputManager.GetAxisRaw("Vertical");
        _xAxis = _inputManager.GetAxisRaw("Horizontal");

        _playerModel.OnUpdate();

        if (_inputManager.GetButtonDown("Jump")) { _player.ExitClimb(); _player.OnJump(); };

        if (_inputManager.GetButtonDown("Dash") && _playerModel.CanDash) _player.SendInput(PlayerStates.Dash);
    }
    public void OnFixedUpdate()
    {
        _player.OnClimb(_yAxis);
    }

    public float YAxis() => _yAxis;

    public float XAxis() => _xAxis;
}
