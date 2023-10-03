using UnityEngine;
using UnityEngine.AI;
public class Enemy_FollowDrone : Enemy
{
    protected enum DroneStates { Idle, Follow};
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
        var playerTransform = Helpers.GameManager.Player.transform;

        idle = new State<DroneStates>("Idle");
        var follow = new State<DroneStates>("Follow");

        StateConfigurer.Create(idle).SetTransition(DroneStates.Follow, follow).Done();
        StateConfigurer.Create(follow).SetTransition(DroneStates.Idle, idle).Done();

        idle.OnUpdate += delegate { if (CanSeePlayer()) _myFsm.SendInput(DroneStates.Follow); };
        follow.OnUpdate += delegate { _navMeshAgent.SetDestination(playerTransform.position); };

        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
    }
    void StartFSM(params object[] param) { _myFsm = new EventFSM<DroneStates>(idle); }
    protected override void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
        base.OnDestroy();
    }
    public override void Update()
    {
        _myFsm?.Update();
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        _myFsm.SendInput(DroneStates.Idle);
        FRY_Enemy_FollowDrone.Instance.pool.ReturnObject(this);
    }
}
