using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;

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
    Tween _dashTween;
    WeaponManager _weaponManager;
    public WeaponManager WeaponManager { get { return _weaponManager; } private set { } }
    public Tween DashTween { get { return _dashTween; } set { _dashTween = value; } }
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
    public Action<float> OnDash = delegate { };
    public Action OnJump;
    public Action<float> OnClimb;

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
        OnJump += _playerModel.Jump;
        OnJump += playerView.Jump;

        OnDash += x => FreezeVelocity();
        OnDash += playerView.Dash;

        OnClimb += _playerModel.ClimbMove;

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDeath);

        _defaultController = new PlayerController(this, _playerModel);

        _controller = _defaultController;
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, OnPlayerDeath);
    }
    private void Update()
    {
        _controller?.OnUpdate();
    }

    private void FixedUpdate()
    {
        _controller?.OnFixedUpdate();
    }
    public void FreezeVelocity()
    {
        _dashTween.Kill();
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