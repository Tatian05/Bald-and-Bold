using UnityEngine;
public class Enemy_GroundHumanoid : Enemy
{
    [SerializeField] protected float _viewRadius;
    [SerializeField] protected float _viewAngle;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _sightRange = 10f;
    [SerializeField] float _stopAttackingTime = 1f;
    enum AK_States { Patrol, Attack }
    EventFSM<AK_States> _myFSM;
    Vector3 _dir = Vector3.right;
    public override void Start()
    {
        base.Start();
        LayerMask invisibleWall = gameManager.InvisibleWallLayer;

        var PATROL = new State<AK_States>("PATROL");
        var ATTACK = new State<AK_States>("ATTACK");

        StateConfigurer.Create(PATROL).SetTransition(AK_States.Attack, ATTACK).Done();
        StateConfigurer.Create(ATTACK).SetTransition(AK_States.Patrol, PATROL).Done();

        #region PATROL

        PATROL.OnEnter += x => OnPatrolStart();
        PATROL.OnUpdate += () =>
        {
            transform.position += _dir * _speed * Time.deltaTime;
            if (Physics2D.Raycast(transform.position, transform.right, 1f, invisibleWall)) FlipEnemy();
            if (GetCanSeePlayer())
                _myFSM.SendInput(AK_States.Attack);
        };

        #endregion

        #region ATTACK

        ATTACK.OnEnter += x =>
        {
            OnAttackStart();
            //Si no lo seteas a uno se rompe el brazo y apunta para atras por como esta seteado el flip del patrol
            transform.localScale = Vector3.one;
        };

        ATTACK.OnUpdate += delegate
        {
            OnAttack();

            if (!GetCanSeePlayer())
                _myFSM.SendInput(AK_States.Patrol);
        };

        #endregion

        _myFSM = new EventFSM<AK_States>(PATROL);
    }

    public override void Update()
    {
        _myFSM.Update();
    }
    public bool GetCanSeePlayer()
    {
        if (Vector3.Angle(transform.right, DistanceToPlayer().normalized) <= _viewAngle / 2)
            return CanSeePlayer();

        return default;
    }
    void FlipEnemy()
    {
        _dir *= -1;
        float angle = transform.eulerAngles.y == 0 ? 180 : 0;

        transform.eulerAngles = new Vector3(0, angle, 0);
    }
    public virtual void OnAttack() { }
    public virtual void OnPatrolStart() { }
    public virtual void OnAttackStart() { }

    public override void ReturnObject()
    {
        base.ReturnObject();
        _myFSM.SendInput(AK_States.Patrol);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 1);
    }
}