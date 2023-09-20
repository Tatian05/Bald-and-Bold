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

    IController _defaultController, _controller;
    PlayerModel _playerModel;
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

    public Action<float> OnMove;
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
        PlayerView playerView = new PlayerView(_anim, _dashParticle);

        StartCoroutine(CanMoveDelay());

        OnMove += _playerModel.Move;
        OnMove += playerView.Run;

        OnJump += FreezeVelocity;
        OnJump += () => _myFsm.SendInput(PlayerStates.Empty);
        OnJump += _playerModel.Jump;
        OnJump += playerView.Jump;

        OnDash += FreezeVelocity;
        OnDash += playerView.Dash;

        OnClimb += _playerModel.ClimbMove;

        #region FSM

        var Empty = new State<PlayerStates>("Empty");
        var Dash = new State<PlayerStates>("Dash");

        StateConfigurer.Create(Empty).SetTransition(PlayerStates.Dash, Dash).Done();
        StateConfigurer.Create(Dash).SetTransition(PlayerStates.Empty, Empty).Done();

        Action<float> onMove = delegate { };
        float dashTimer = 0;
        Dash.OnEnter += x =>
        {
            OnDash();
            onMove = OnMove;
            OnMove = delegate { };
            dashTimer = 0;
        };
        Dash.OnUpdate += () =>
        {
            dashTimer += Time.deltaTime;
            _playerModel.CeroGravity();
            if (dashTimer >= .1f) _myFsm.SendInput(PlayerStates.Empty);
        };
        Dash.OnFixedUpdate += _playerModel.Dash;
        Dash.OnExit += x => { OnMove = onMove; _playerModel.NormalGravity(); };

        #endregion

        _defaultController = new PlayerController(this, _playerModel);

        _controller = _defaultController;

        _myFsm = new EventFSM<PlayerStates>(Empty);

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDeath);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDeath);
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
    public void FreezeVelocity()
    {
        _rb.velocity = Vector2.zero;
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
        _controller = _defaultController;
    }
    public override void PausePlayer()
    {
        _canMove = false;
        FreezeVelocity();
        _anim.SetInteger("xAxis", 0);
        _controller = null;
    }
    public override void UnPausePlayer()
    {
        StartCoroutine(CanMoveDelay());
    }

    public void SendInput(PlayerStates PlayerState) { _myFsm.SendInput(PlayerState); }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope") && !_dead)
            EnterRope(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
            ExitClimb();
    }

    void EnterRope(GameObject rope)
    {
        FreezeVelocity();
        _controller = new ClimbController(this, _playerModel);
        _anim.SetInteger("xAxis", 0);
        transform.position = new Vector2(rope.transform.position.x, transform.position.y);
    }
    public void ExitClimb()
    {
        if (_controller == _defaultController) return;
        _controller = _defaultController;
        _playerModel.NormalGravity();
    }

    IEnumerator Death()
    {
        _dead = true;
        yield return null;
        _dead = false;
    }
    void OnPlayerDeath(params object[] param)
    {
        FreezeVelocity();
        StartCoroutine(Death());
        _playerModel.ResetStats();
    }
}