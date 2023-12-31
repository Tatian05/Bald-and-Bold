using UnityEngine;
public class PlayerModel
{
    float _currentJumps, _maxJumps;
    LayerMask _groundLayer;
    float _dashTimer, _dashCooldown = .2f;
    float _coyotaTimer;
    float _coyotaTime;
    float _currentSpeed;
    bool _secondJump;
    bool _inRope;
    public bool InRope { get { return _inRope; } set { _inRope = value; } }
    public float Speed { get { return _currentSpeed; } }

    #region Constructor 

    Rigidbody2D _rb;
    Transform _myTransform, _groundCheckTransform1, _groundCheckTransform2;
    float _speed, _jumpForce, _dashSpeed, _defaultGravity;
    WeaponManager _weaponManager;

    #endregion
    public int GetLookingForDir => _weaponManager.GetAngle() < -90 || _weaponManager.GetAngle() > 90 ? -1 : 1;

    public bool InGrounded => Physics2D.OverlapCircle(_groundCheckTransform1.position, .05f, _groundLayer) || Physics2D.OverlapCircle(_groundCheckTransform2.position, .05f, _groundLayer);
    public bool CanJump => (InGrounded && _currentJumps <= 0) || _coyotaTimer > 0 || (!InGrounded && _secondJump && _currentJumps <= _maxJumps);
    public bool CanDash => _dashTimer >= _dashCooldown;
    public PlayerModel(Rigidbody2D rb, Transform myTransform, Transform groundCheckTransform1, Transform groundCheckTransform2,
        float speed, float jumpForce, float maxJumps, float dashSpeed, float defaultGravity, float coyotaTime, WeaponManager weaponManager)
    {
        _rb = rb;
        _myTransform = myTransform;
        _groundCheckTransform1 = groundCheckTransform1;
        _groundCheckTransform2 = groundCheckTransform2;
        _speed = speed;
        _jumpForce = jumpForce;
        _maxJumps = maxJumps;
        _dashSpeed = dashSpeed;
        _defaultGravity = defaultGravity;
        _coyotaTime = coyotaTime;
        _weaponManager = weaponManager;

        _rb.gravityScale = defaultGravity;
        _dashTimer = _dashCooldown;
        _groundLayer = LayerMask.GetMask("Border") + LayerMask.GetMask("Ground");

        EventManager.SubscribeToEvent(Contains.CONSUMABLE_BOOTS, BootsConsumable);
    }
    public void OnUpdate()
    {
        if (InGrounded)
        {
            if (_rb.velocity.y <= 0 && _currentJumps != 0)
            {
                _currentJumps = 0;
                _secondJump = true;
            }
            _coyotaTimer = _coyotaTime;
        }
        else _coyotaTimer -= Time.deltaTime;

        _dashTimer += Time.deltaTime;

        LookAt();
    }
    public void Move(float xAxis, float yAxis = 0)
    {
        _currentSpeed = Mathf.Sign(_myTransform.right.x) != Mathf.Sign(_rb.velocity.x) && InGrounded ? _speed * .5f : _speed;

        _rb.velocity = new Vector2(xAxis * _currentSpeed * Time.fixedDeltaTime, _rb.velocity.y);
    }

    public void ClimbMove(float xAxis, float yAxis)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, yAxis * _speed * Time.fixedDeltaTime);
    }
    public void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode2D.Impulse);
        _currentJumps++;
        _coyotaTimer = 0;
        _secondJump = false;
    }

    public void Dash(float xAxis)
    {
        float dir = xAxis != 0 ? xAxis : _myTransform.right.x;

        _rb.velocity = Vector2.right * Mathf.Sign(dir) * _dashSpeed * Time.fixedDeltaTime;
        _dashTimer = 0;
        _secondJump = true;
    }
    public void LookAt()
    {
        _myTransform.localEulerAngles = new Vector3(0, _weaponManager.AimDirection.x >= 0 ? 0 : 180, 0);
    }
    public void FreezeVelocity() { _rb.velocity = Vector2.zero; }
    public void CeroGravity() { _rb.gravityScale = 0; }
    public void NormalGravity() { _rb.gravityScale = _defaultGravity; }
    public void ResetStats()
    {
        _dashTimer = _dashCooldown;
        _currentJumps = 0;
        _coyotaTimer = _coyotaTime;
        _secondJump = true;
        NormalGravity();
    }
    void BootsConsumable(params object[] param) { _groundLayer += (bool)param[0] ? LayerMask.GetMask("OnlyPlayerInteractuable") : -LayerMask.GetMask("OnlyPlayerInteractuable"); }
    public void OnPlayerDeath()
    {
        FreezeVelocity();
        ResetStats();
    }

    public void OnDestroy()
    {
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_BOOTS, BootsConsumable);
    }
}