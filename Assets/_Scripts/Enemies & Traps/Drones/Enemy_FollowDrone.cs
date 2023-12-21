using UnityEngine;
using UnityEngine.AI;
public class Enemy_FollowDrone : Enemy
{
    protected enum DroneStates { Idle, Lost, Follow };
    [SerializeField] protected float _speed = 1f;

    protected EventFSM<DroneStates> _myFsm;
    State<DroneStates> idle;
    protected NavMeshAgent _navMeshAgent;
    public override void Start()
    {
        base.Start();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = true;
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _navMeshAgent.speed = _speed;

        idle = new State<DroneStates>("Idle");
        var lost = new State<DroneStates>("Lost");
        var follow = new State<DroneStates>("Follow");

        StateConfigurer.Create(idle).SetTransition(DroneStates.Follow, follow).Done();
        StateConfigurer.Create(follow).SetTransition(DroneStates.Idle, idle).SetTransition(DroneStates.Lost, lost).Done();
        StateConfigurer.Create(lost).SetTransition(DroneStates.Idle, idle).Done();

        #region IDLE

        idle.OnUpdate += delegate
        {
            if (!_playerPosition) return;
            if (CanSeePlayer()) _myFsm.SendInput(DroneStates.Follow);
        };

        #endregion

        #region LOST

        float lostTimer = 0;
        lost.OnEnter += x => SetSign(true, _lostSign);
        lost.OnUpdate += delegate
        {
            lostTimer += CustomTime.DeltaTime;
            if (lostTimer >= _lostTime) _myFsm.SendInput(DroneStates.Idle);
        };
        lost.OnExit += x => SetSign(false);

        #endregion

        #region FOLLOW

        follow.OnEnter += x => SetSign(true, _agroSign);
        follow.OnUpdate += delegate { _navMeshAgent.speed = _speed * CustomTime.LocalTimeScale; _navMeshAgent.SetDestination(_playerPosition.position); };
        follow.OnExit += x => SetSign(false);

        #endregion

        if (!Helpers.LevelTimerManager || Helpers.LevelTimerManager.LevelStarted) _myFsm = new EventFSM<DroneStates>(idle);
        else EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
    }
    void StartFSM(params object[] param) { _myFsm = new EventFSM<DroneStates>(idle); }
    protected override void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
        base.OnDestroy();
    }
    void Update()
    {
        _myFsm?.Update();
    }
    protected override void PlayerInvisibleConsumable(params object[] param)
    {
        _myFsm?.SendInput((bool)param[0] ? DroneStates.Lost : DroneStates.Idle);
        _navMeshAgent.ResetPath();
    }
    public override void ReturnObject()
    {
        base.ReturnObject();
        _myFsm?.SendInput(DroneStates.Idle);
        FRY_Enemy_FollowDrone.Instance.pool.ReturnObject(this);
    }
}
