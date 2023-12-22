using System.Linq;
using UnityEngine;
public class Enemy_KamikazeRobot : Enemy
{
    [SerializeField] float _minDropSpeed, _maxDropSpeed, _dropSpeedMultiplier = .25f;
    [SerializeField] float _overlapCircleRadius = 1.5f;
    [SerializeField] float _dmg;

    float _dropSpeed;
    bool _isDropping;
    EnemyHealth _enemyHealth;
    enum KamikazeStates { Idle, Drop }
    EventFSM<KamikazeStates> _myFSM;
    State<KamikazeStates> IDLE;
    public override void Start()
    {
        base.Start();
        _dropSpeed = _minDropSpeed;
        _enemyHealth = GetComponentInChildren<EnemyHealth>();
        IDLE = new State<KamikazeStates>("IDLE");
        var DROP = new State<KamikazeStates>("DROP");

        StateConfigurer.Create(IDLE).SetTransition(KamikazeStates.Drop, DROP).Done();
        StateConfigurer.Create(DROP).SetTransition(KamikazeStates.Idle, IDLE).Done();

        IDLE.OnUpdate += delegate
        {
            if (Physics2D.CircleCast(transform.position, 1, -Vector3.up, 10f, gameManager.PlayerLayer))
            {
                if (!Physics2D.Raycast(transform.position, -Vector3.up, (transform.position - Helpers.GameManager.Player.transform.position).magnitude, gameManager.GroundLayer))
                {
                    _isDropping = true;
                    _myFSM.SendInput(KamikazeStates.Drop);
                    Helpers.AudioManager.PlaySFX("KamikazeDrop");
                }
            }
        };

        DROP.OnUpdate += () =>
        {
            _dropSpeed += CustomTime.TimeScale * _dropSpeedMultiplier;
            transform.position += -transform.up * _dropSpeed * CustomTime.DeltaTime;
            Mathf.Clamp(_dropSpeed, _minDropSpeed, _maxDropSpeed);
        };

        if (!Helpers.LevelTimerManager || Helpers.LevelTimerManager.LevelStarted) _myFSM = new EventFSM<KamikazeStates>(IDLE);
        else EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
    }

    private void Update()
    {
        _myFSM?.Update();
    }
    void StartFSM(params object[] param) { _myFSM = new EventFSM<KamikazeStates>(IDLE); }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_eyes.position, _overlapCircleRadius);
    }
    protected override void PlayerInvisibleConsumable(params object[] param)
    {
        _myFSM = (bool)param[0] ? null : new EventFSM<KamikazeStates>(IDLE);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isDropping || collision.CompareTag("Bullet") || collision.gameObject.layer == 25) return;


        if (collision.TryGetComponent(out IDamageable player))
        {
            player.TakeDamage(_dmg);
            return;
        }

        var overlap = Physics2D.OverlapCircleAll(_eyes.position, _overlapCircleRadius, gameManager.PlayerLayer).
                      Where(x => Physics2D.Raycast(_eyes.position, DistanceToPlayer(), _overlapCircleRadius)).FirstOrDefault();

        if (overlap && overlap.TryGetComponent(out player))
        {
            player.TakeDamage(_dmg);
            return;
        }

        _enemyHealth.Die();
    }
    protected override void OnReset()
    {
        base.OnReset();

        _dropSpeed = _minDropSpeed;
        _myFSM?.SendInput(KamikazeStates.Idle);

        if (_isDropping)
            _isDropping = false;
    }

    protected override void Reset()
    {
        base.Reset();

        _dropSpeed = _minDropSpeed;
        _myFSM?.SendInput(KamikazeStates.Idle);

        if (_isDropping)
            _isDropping = false;
    }
    public override void ReturnObject()
    {
        FRY_Enemy_KamikazeRobot.Instance.pool.ReturnObject(this);
        base.ReturnObject();
    }
}