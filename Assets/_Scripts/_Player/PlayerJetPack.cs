using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerJetPack : GeneralPlayer
{
    Action OnUpdate;
    Rigidbody2D _rb;
    [HideInInspector] public StateMachine fsm;
    PlayerJetPackController _controller;
    public Animator Animator { get { return _animator; } }

    WeaponManager _weaponManager;
    public WeaponManager WeaponManager { get { return _weaponManager; } private set { } }

    [SerializeField] Transform _playerSprite, _groundCheckTransform;
    [SerializeField] Animator _animator;
    [SerializeField] float _speed = 400f;
    [SerializeField] float _flyingSpeed = 400f;
    [SerializeField] float _maxDelayCanMove = .2f;
    [SerializeField] float _maxFuel = 2f;
    float _currentFuel = 0;
    float _defaultGravity;

    Vector3 _playerDefaultSpriteSize;
    LayerMask _groundLayer;

    //Visual    
    [SerializeField] Image _fuelBar;
    [SerializeField] GameObject _fireParticle;
    public GameObject FireParticle { get { return _fireParticle; } private set { } }

    public bool InGrounded => Physics2D.OverlapCircle(_groundCheckTransform.position, .1f, _groundLayer);
    private void Start()
    {
        StartCoroutine(CanMoveDelay());
        _groundLayer = LayerMask.GetMask("Border") + LayerMask.GetMask("Ground");
        _rb = GetComponent<Rigidbody2D>();
        _weaponManager = GetComponent<WeaponManager>();
        fsm = new StateMachine();
        _controller = new PlayerJetPackController(this);

        OnUpdate += fsm.Update;
        OnUpdate += LookAtMouse;

        fsm.AddState(StateName.FlyingUp, new UpState(this, _controller));
        fsm.AddState(StateName.Droping, new DownState(this, _controller));
        fsm.AddState(StateName.OnFloor, new OnFloorState(this, _controller));
        fsm.ChangeState(StateName.Droping);

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);

        _playerDefaultSpriteSize = _playerSprite.localScale;
        _defaultGravity = _rb.gravityScale;
    }
    private void OnDestroy()
    {
        OnUpdate -= fsm.Update;
        OnUpdate -= LookAtMouse;
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDead);
    }

    private void Update()
    {
        if (_canMove) OnUpdate?.Invoke();

        _controller?.OnUpdate();
    }
    private void FixedUpdate()
    {
        if (_canMove) fsm.FixedUpdate();
    }

    public void Move(float axis)
    {
        _rb.velocity = new Vector2(axis * _speed * Time.fixedDeltaTime, _rb.velocity.y);
    }

    public void FlyUp()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _flyingSpeed * Time.fixedDeltaTime);
    }

    public void ReturnFuel()
    {
        if (_currentFuel >= _maxFuel) return;
        _currentFuel += Time.deltaTime * 2f;
        UpdateFuelBar();
    }

    public void TakeFuel()
    {
        _currentFuel -= Time.deltaTime;
        UpdateFuelBar();
        if (_currentFuel <= 0) fsm.ChangeState(StateName.Droping);
    }

    public override void PausePlayer()
    {
        _canMove = false;
        CeroGravity();
        FreezeVelocity();
        _animator.SetInteger("xAxis", 0);
    }
    public override void UnPausePlayer()
    {
        StartCoroutine(CanMoveDelay());
        NormalGravity();
    }

    public IEnumerator CanMoveDelay()
    {
        yield return new WaitForSeconds(_maxDelayCanMove);

        _canMove = true;
    }

    public void CeroGravity()
    {
        _rb.gravityScale = 0;
    }

    public void NormalGravity()
    {
        _rb.gravityScale = _defaultGravity;
    }

    public void FreezeVelocity(bool xAxis = false)
    {
        if (xAxis) _rb.velocity = new Vector2(0, _rb.velocity.y);
        else _rb.velocity = Vector2.zero;
        //OnIdle();
    }

    public void LookAtMouse()
    {
        Vector3 playerLocalScale = _playerDefaultSpriteSize;
        float angle = _weaponManager.GetAngle();

        if (angle > 90 || angle < -90)
            playerLocalScale.x = -_playerDefaultSpriteSize.x;
        else
            playerLocalScale.x = _playerDefaultSpriteSize.x;

        _playerSprite.localScale = playerLocalScale;
    }
    void UpdateFuelBar()
    {
        _fuelBar.fillAmount = _currentFuel / _maxFuel;
    }

    void OnPlayerDead(params object[] param)
    {
        _currentFuel = _maxFuel;
        UpdateFuelBar();
        fsm.ChangeState(StateName.Droping);
    }
}

public class UpState : IState
{
    PlayerJetPack _player;
    PlayerJetPackController _controller;
    public UpState(PlayerJetPack player, PlayerJetPackController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        _player.FireParticle.SetActive(true);
        _player.Animator.SetInteger("xAxis", 0);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        _player.FlyUp();
        _player.Move(_controller.xAxis);
    }

    public void OnUpdate()
    {
        _controller.FlyingInputs();
        _player.TakeFuel();
    }
}
public class DownState : IState
{
    PlayerJetPack _player;
    PlayerJetPackController _controller;
    public DownState(PlayerJetPack player, PlayerJetPackController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        _player.FireParticle.SetActive(false);
        _player.Animator.SetInteger("xAxis", 0);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        _player.Move(_controller.xAxis);
    }

    public void OnUpdate()
    {
        _player.ReturnFuel();
        _controller.DropingInputs();
    }
}
public class OnFloorState : IState
{
    PlayerJetPack _player;
    PlayerJetPackController _controller;
    public OnFloorState(PlayerJetPack player, PlayerJetPackController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        _player.FreezeVelocity();
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        _player.Move(_controller.xAxis);
        _player.Animator.SetInteger("xAxis", (int)Mathf.Abs(_controller.xAxis));
    }

    public void OnUpdate()
    {
        _controller.OnFloorInputs();
        _player.ReturnFuel();
    }
}

public class PlayerJetPackController
{
    PlayerJetPack _player;
    InputManager _inputManager;
    public float xAxis { get; private set; }
    public float yAxis { get; private set; }
    public PlayerJetPackController(PlayerJetPack player)
    {
        _inputManager = InputManager.Instance;
        _player = player;
    }

    public void OnUpdate()
    {
        xAxis = _inputManager.GetAxisRaw("Horizontal");
        yAxis = _inputManager.GetAxisRaw("Vertical");
    }

    public void DropingInputs()
    {
        if (_inputManager.GetButtonDown("Jump"))
            _player.fsm.ChangeState(StateName.FlyingUp);
        if (_player.InGrounded)
            _player.fsm.ChangeState(StateName.OnFloor);
    }
    public void FlyingInputs()
    {
        if (_inputManager.GetButtonUp("Jump"))
            _player.fsm.ChangeState(StateName.Droping);
    }
    public void OnFloorInputs()
    {
        if (_inputManager.GetButtonDown("Jump"))
            _player.fsm.ChangeState(StateName.FlyingUp);
    }
}