using System.Collections;
using UnityEngine;
using System;
using System.Threading.Tasks;

public enum PlayerStates { Empty, Dash }

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : GeneralPlayer, IDamageable
{
    bool _dead, _boots, _climbing;

    #region Components
    [SerializeField] Animator _anim;
    [SerializeField] Transform _groundCheckTransform1, _groundCheckTransform2, _spritesContainer;
    [SerializeField] SpriteRenderer[] _playerSprites;
    [SerializeField] BoxCollider2D _bootsCollider;

    PlayerController _controller;
    PlayerModel _playerModel;
    PlayerView _playerView;
    Rigidbody2D _rb;
    WeaponManager _weaponManager;
    Vector3 _initialPos;
    CheckPoint _checkpoint;
    public WeaponManager WeaponManager { get { return _weaponManager; } private set { } }
    public CheckPoint Checkpoint { get { return _checkpoint; } set { _checkpoint = value; } }

    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] float _speed;
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

    public Action<float, float> OnMove;
    public Action OnDash = delegate { };
    public Action OnJump;
    public Action<float> OnClimb;

    EventFSM<PlayerStates> _myFsm;
    GoldenBald _goldenBald;
    LayerMask _ladderMask;
    private void Start()
    {
        _weaponManager = GetComponent<WeaponManager>();
        _rb = GetComponent<Rigidbody2D>();
        float defaultGravity = _rb.gravityScale;
        _ladderMask = LayerMask.GetMask("Ladder");
        _initialPos = transform.position;

        _playerModel = new PlayerModel(_rb, transform, _groundCheckTransform1, _groundCheckTransform2, _speed, _jumpForce, _maxJumps, _dashSpeed, defaultGravity, _coyotaTime, _weaponManager);
        _playerView = new PlayerView(transform, _anim, _dashParticle, _playerSprites, _bootsCollider, _spritesContainer);

        OnMove = (x, y) => { _playerModel.Move(x, y); _playerView.Run(x, _playerModel.InGrounded, _playerModel.Speed / _speed, y); };

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

        float dashTimer = 0;
        Dash.OnEnter += x =>
        {
            OnDash();
            ExitClimb();
            OnMove = delegate { };
            dashTimer = 0;
            _playerModel.CeroGravity();
        };
        Dash.OnUpdate += () =>
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= .1f) _myFsm.SendInput(PlayerStates.Empty);
        };

        Dash.OnFixedUpdate += delegate { _playerModel.Dash(_controller.XAxis); };
        Dash.OnExit += x =>
        {
            OnMove = (x, y) => { _playerModel.Move(x, y); _playerView.Run(x, _playerModel.InGrounded, _playerModel.Speed / _speed, y); };
            _playerModel.NormalGravity();
            ExitClimb();
        };

        #endregion

        _controller = new PlayerController(this, _playerModel);

        _myFsm = new EventFSM<PlayerStates>(Empty);

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDeath);
        _controller.OnEnable();

        _boots = Helpers.PersistantData.consumablesData.boots;
        if (_boots) EventManager.TriggerEvent(Contains.CONSUMABLE_BOOTS, true);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDeath);
        _playerView.OnDestroy();
        _playerModel.OnDestroy();
        _controller.OnDestroy();
    }
    private void Update()
    {
        _controller?.OnUpdate();
        _myFsm.Update();
        _goldenBald?.SetPosition(transform.position + Vector3.up * 2.25f);

        var ropeCheck = Physics2D.OverlapCircle(transform.position, .5f, _ladderMask);

        if ((bool)ropeCheck && _playerModel.InGrounded) CheckForRope(ropeCheck.transform);
    }

    private void FixedUpdate()
    {
        _controller?.OnFixedUpdate();
        _myFsm.FixedUpdate();
    }
    public async void TakeDamage(float dmg)
    {
        EventManager.TriggerEvent(Contains.PLAYER_DEAD);
        await Task.Delay(100);
        EventManager.TriggerEvent(Contains.WAIT_PLAYER_DEAD);
    }
    public void Die() { }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheckTransform1.position, .05f);
        Gizmos.DrawWireSphere(_groundCheckTransform2.position, .05f);


        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
    public override void PausePlayer()
    {
        _controller.OnDestroy();
        _myFsm.SendInput(PlayerStates.Empty);
        _playerModel.FreezeVelocity();
        _anim.SetInteger("xAxis", 0);
    }
    public void SendInput(PlayerStates PlayerState) { _myFsm.SendInput(PlayerState); }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out GoldenBald goldenBald)) _goldenBald = goldenBald.SetOwner(true);

        if (collision.CompareTag("Rope") && !_dead)
        {
            if (_playerModel.InGrounded) return;

            _myFsm.SendInput(PlayerStates.Empty);
            EnterRope(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
            ExitClimb();
    }
    void CheckForRope(Transform ropeTransform)
    {
        _playerModel.InRope = true;
        if (_playerModel.InRope && !_dead)
        {
            if (_controller.YAxis > 0 && !_climbing)
                EnterRope(ropeTransform);
            else if (_controller.XAxis != 0 && _controller.YAxis <= 0 && _climbing)
                ExitClimb();
        }
    }
    void EnterRope(Transform rope)
    {
        if (_climbing) return;

        _playerModel.InRope = true;
        _playerModel.FreezeVelocity();
        transform.position = new Vector2(rope.position.x, transform.position.y);

        _playerModel.CeroGravity();
        _playerModel.ResetStats();
        _climbing = true;

        OnMove = _playerModel.ClimbMove;
        OnMove += _playerView.Climb;

        _playerView.OnStartClimb(_weaponManager.HasWeapon);
    }
    public void ExitClimb()
    {
        Debug.Log("Exitd");
        if (!_playerModel.InRope) return;

        Debug.Log("Exit");
        _climbing = false;
        _playerModel.InRope = false;
        _playerModel.NormalGravity();
        OnMove = (x, y) => { _playerModel.Move(x, y); _playerView.Run(x, _playerModel.InGrounded, _playerModel.Speed / _speed, y); };
        OnMove -= _playerView.Climb;
        _playerView.OnExitClimb();
    }

    IEnumerator Death()
    {
        _dead = true;
        yield return null;
        _dead = false;
    }
    void OnPlayerDeath(params object[] param)
    {
        _playerView.OnDeath();
        _playerModel.OnPlayerDeath();
        _goldenBald?.ResetGoldenBald();
        _goldenBald = null;
        StartCoroutine(Death());
        _myFsm.SendInput(PlayerStates.Empty);
        OnMove = (x, y) => { _playerModel.Move(x, y); _playerView.Run(x, _playerModel.InGrounded, _playerModel.Speed / _speed, y); };
        transform.position = _checkpoint ? _checkpoint.transform.position : _initialPos;
    }
    public void PauseInDeath()
    {
        PausePlayer();
        _playerModel.CeroGravity();
        _playerView.ElectrocuteDeath();
    }
    public void ResetCheckPoint() => _checkpoint = null;
}