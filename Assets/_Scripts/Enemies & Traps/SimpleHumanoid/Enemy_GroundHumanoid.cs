using System.Collections;
using UnityEngine;
public class Enemy_GroundHumanoid : Enemy
{
    StateMachine _fsm;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected float _viewRadius;
    [SerializeField] protected float _viewAngle;

    public Transform Sprite { get { return sprite; } private set { } }

    [SerializeField] float _speed = 1f;
    public float Speed { get { return _speed; } private set { } }

    [SerializeField] float _sightRange = 10f;
    public float SightRange { get { return _sightRange; } private set { } }

    [SerializeField] float _stopAttackingTime = 1f;
    public float StopAttackingTime { get { return _stopAttackingTime; } private set { } }

    public bool isFacingRight = true;

    public override void Start()
    {
        base.Start();

        //Components
        _fsm = new StateMachine();
        _anim = GetComponentInChildren<Animator>();

        //StateMachine
        _fsm.AddState(StateName.SH_Patrol, new SH_PatrolState(_fsm, this));
        _fsm.AddState(StateName.SH_Attack, new SH_AttackState(_fsm, this));
        _fsm.ChangeState(StateName.SH_Patrol);

        OnUpdate += _fsm.Update;
    }

    public bool GetCanSeePlayer()
    {
        Vector3 dir = DistanceToPlayer();

        if (Vector3.Angle(transform.right, dir.normalized) <= _viewAngle / 2)
            return CanSeePlayer();

        return default;
    }

    public virtual void OnAttack() { }
    public virtual void OnPatrolStart() { }
    public virtual void OnAttackStart() { }

    public override void ReturnObject()
    {
        base.ReturnObject();
        _fsm.ChangeState(StateName.SH_Patrol);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 1);
    }
}

public class SH_PatrolState : IState
{
    StateMachine _fsm;
    Enemy_GroundHumanoid _enemy;

    Vector3 _dir = Vector3.right;

    public SH_PatrolState(StateMachine fsm, Enemy_GroundHumanoid enemy)
    {
        _fsm = fsm;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        _enemy.OnPatrolStart();
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate() { }

    public void OnUpdate()
    {
        Move();

        if (_enemy.GetCanSeePlayer())
            _fsm.ChangeState(StateName.SH_Attack);
    }

    void Move()
    {
        _enemy.transform.position += _dir * _enemy.Speed * Time.deltaTime;

        if (Physics2D.Raycast(_enemy.transform.position, _enemy.transform.right, 1f, Helpers.GameManager.InvisibleWallLayer)) Flip();
    }

    void Flip()
    {
        _dir *= -1;
        float angle = _enemy.transform.eulerAngles.y == 0 ? 180 : 0;

        _enemy.transform.eulerAngles = new Vector3(0, angle, 0);
    }
}
public class SH_AttackState : IState
{
    StateMachine _fsm;
    Enemy_GroundHumanoid _enemy;

    bool _isInCoroutine = false;

    public SH_AttackState(StateMachine fsm, Enemy_GroundHumanoid enemy)
    {
        _fsm = fsm;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        _enemy.OnAttackStart();
        _isInCoroutine = false;

        //Si no lo seteas a uno se rompe el brazo y apunta para atras por como esta seteado el flip del patrol
        _enemy.transform.localScale = Vector3.one;

    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate() { }

    public void OnUpdate()
    {
        _enemy.OnAttack();

        if (!_enemy.GetCanSeePlayer())
        {
            if (!_isInCoroutine)
            {
                _enemy.StartCoroutine(StopAttackingCoroutine());
                _isInCoroutine = true;
            }
        }
    }

    IEnumerator StopAttackingCoroutine()
    {
        yield return new WaitForSeconds(_enemy.StopAttackingTime);
        _fsm.ChangeState(StateName.SH_Patrol);
    }
}
