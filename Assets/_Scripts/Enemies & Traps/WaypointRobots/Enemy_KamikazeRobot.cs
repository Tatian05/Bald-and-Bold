using System.Linq;
using UnityEngine;
public class Enemy_KamikazeRobot : Enemy
{
    [SerializeField] float _dropSpeed;
    [SerializeField] float _overlapCircleRadius = 1.5f;
    [SerializeField] float _dmg;

    bool _isDropping;
    enum KamikazeStates { Idle, Drop }
    EventFSM<KamikazeStates> _myFSM;
    State<KamikazeStates> IDLE;
    public override void Start()
    {
        base.Start();
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
                }
            }
        };

        DROP.OnUpdate += () => transform.position += -transform.up * _dropSpeed * Time.deltaTime;

        if (Helpers.LevelTimerManager.LevelStarted) _myFSM = new EventFSM<KamikazeStates>(IDLE);
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
        Gizmos.DrawWireSphere(sprite.position, _overlapCircleRadius);
    }
    protected override void PlayerInvisibleConsumable(params object[] param)
    {
        base.PlayerInvisibleConsumable(param);
        _myFSM = (bool)param[0] ? null : new EventFSM<KamikazeStates>(IDLE);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isDropping || collision.CompareTag("Bullet") || collision.gameObject.layer == 25) return;

        Die();

        if (collision.TryGetComponent(out IDamageable player))
        {
            player.TakeDamage(_dmg);
            return;
        }

        var overlap = Physics2D.OverlapCircleAll(sprite.position, _overlapCircleRadius, gameManager.PlayerLayer).
                      Where(x => Physics2D.Raycast(_eyes.position, DistanceToPlayer(), _overlapCircleRadius)).FirstOrDefault();

        if (overlap && overlap.TryGetComponent(out player))
            player.TakeDamage(_dmg);
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_KamikazeRobot.Instance.pool.ReturnObject(this);
    }

    public override void Reset()
    {
        _myFSM?.SendInput(KamikazeStates.Idle);

        if (_isDropping)
        {
            _isDropping = false;
        }
        base.Reset();
    }
}
