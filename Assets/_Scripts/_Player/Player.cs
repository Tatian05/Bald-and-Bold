using System.Collections;
using UnityEngine;
using System;
public enum PlayerStates { Empty, Dash }

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Player : GeneralPlayer
{
    bool _dead;

    #region Components
    [SerializeField] Animator _anim;
    [SerializeField] Transform _playerSprite, _groundCheckTransform;

    PlayerController _controller;
    PlayerModel _playerModel;
    PlayerView _playerView;
    Rigidbody2D _rb;
    WeaponManager _weaponManager;
    public WeaponManager WeaponManager { get { return _weaponManager; } private set { } }

    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] float _speed;
    [SerializeField] float _maxDelayCanMove = .2f;
    #endregion

    #region Jump
    [Header("Jump")]
    [SerializeField] float _jumpForce = 5;
    [SerializeField] float _maxJumps = 1;
    [SerializeField] float _coyotaTime;
    #endregion

    #region Dash
    [Header("Dash")]
    [SerializeField] float _dashSpeed;
    [SerializeField] ParticleSystem _dashParticle;

    #endregion

    PlayerInputs _playerInputs;
    public PlayerInputs PlayerInputs { get { return _playerInputs; } private set { } }

    public Action<float, float> OnMove;
    public Action OnDash = delegate { };
    public Action OnJump;
    public Action<float> OnClimb;

    EventFSM<PlayerStates> _myFsm;
    private void Start()
    {
        _weaponManager = GetComponent<WeaponManager>();
        _rb = GetComponent<Rigidbody2D>();
        float defaultGravity = _rb.gravityScale;

        _playerModel = new PlayerModel(_rb, transform, _playerSprite, _groundCheckTransform, _speed, _jumpForce, _maxJumps, _dashSpeed, defaultGravity, _coyotaTime, _weaponManager);
        _playerView = new PlayerView(_anim, _dashParticle);

        StartCoroutine(CanMoveDelay());

        OnMove = (x, y) => { _playerModel.Move(x, y); _playerView.Run(x, y); };

        OnJump += _playerModel.FreezeVelocity;
        OnJump += () => _myFsm.SendInput(PlayerStates.Empty);
        OnJump += _playerModel.Jump;
        OnJump += _playerView.Jump;

        OnDash += _playerModel.FreezeVelocity;
        OnDash += () => _playerView.Dash(transform.right.x);

        #region FSM

        var Empty = new State<PlayerStates>("Empty");
        var Dash = new State<PlayerStates>("Dash");

        StateConfigurer.Create(Empty).SetTransition(PlayerStates.Dash, Dash).Done();
        StateConfigurer.Create(Dash).SetTransition(PlayerStates.Empty, Empty).Done();

        Action<float, float> onMove = delegate { };
        float dashTimer = 0;
        Dash.OnEnter += x =>
        {
            OnDash();
            onMove = OnMove;
            OnMove = delegate { };
            dashTimer = 0;
            _playerModel.CeroGravity();
        };
        Dash.OnUpdate += () =>
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= .1f) _myFsm.SendInput(PlayerStates.Empty);
        };
        Dash.OnFixedUpdate += _playerModel.Dash;
        Dash.OnExit += x => { OnMove = onMove; _playerModel.NormalGravity(); };

        #endregion



        _controller = new PlayerController(this, _playerModel);

        _myFsm = new EventFSM<PlayerStates>(Empty);

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDeath);
        _controller.OnEnable();
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDeath);
        _controller.OnDisable();
    }
    private void Update()
    {
        _controller?.OnUpdate();
        _myFsm.Update();
    }

    private void FixedUpdate()
    {
        _controller?.OnFixedUpdate();
        _myFsm.FixedUpdate();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheckTransform.position, .2f);
    }
    public IEnumerator CanMoveDelay()
    {
        yield return new WaitForSeconds(_maxDelayCanMove);

        _canMove = true;
    }
    public override void PausePlayer()
    {
        _canMove = false;
        _controller = null;
        _myFsm.SendInput(PlayerStates.Empty);
        _playerModel.FreezeVelocity();
        _anim.SetInteger("xAxis", 0);
    }
    public override void UnPausePlayer() { StartCoroutine(CanMoveDelay()); }

    public void SendInput(PlayerStates PlayerState) { _myFsm.SendInput(PlayerState); }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope") && !_dead)
        {
            _playerModel.InRope = true;
            if (_playerModel.InGrounded) return;

            _myFsm.SendInput(PlayerStates.Empty);
            EnterRope(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerModel.InRope && !_dead)
        {
            if (!_playerModel.InGrounded) return;

            if (_controller.YAxis() >= 1 || _rb.velocity.y >= 1)
                EnterRope(collision.gameObject);
            else if (_controller.XAxis() != 0)
                ExitClimb();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
            ExitClimb();
    }

    void EnterRope(GameObject rope)
    {
        _playerModel.FreezeVelocity();
        _playerModel.ResetStats();
        _playerModel.CeroGravity();

        OnMove = _playerModel.ClimbMove;

        _anim.SetInteger("xAxis", 0);
        transform.position = new Vector2(rope.transform.position.x, transform.position.y);
    }
    public void ExitClimb()
    {
        if (!_playerModel.InRope) return;

        _playerModel.InRope = false;
        _playerModel.NormalGravity();
        OnMove = (x, y) => { _playerModel.Move(x, y); _playerView.Run(x, y); };
    }

    IEnumerator Death()
    {
        _dead = true;
        yield return null;
        _dead = false;
    }
    void OnPlayerDeath(params object[] param)
    {
        _playerModel.FreezeVelocity();
        StartCoroutine(Death());
        _playerModel.ResetStats();
        _myFsm.SendInput(PlayerStates.Empty);
        OnMove = (x, y) => { _playerModel.Move(x, y); _playerView.Run(x, y); };
    }
}