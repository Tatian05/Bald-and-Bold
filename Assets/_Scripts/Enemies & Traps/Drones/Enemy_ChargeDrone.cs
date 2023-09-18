using UnityEngine;
public class Enemy_ChargeDrone : Enemy
{
    StateMachine _fsm;

    [Header ("Idle")]
    [SerializeField] float _idleWaitTime;
    public float IdleWaitTime { get { return _idleWaitTime; } }

    [Header("Charge")]
    [SerializeField] float _chargeSpeed;
    public float ChargeSpeed { get { return _chargeSpeed; } }

    [SerializeField] float _chargeDistance;
    public float ChargeDistance { get { return _chargeDistance; } }

    [SerializeField] GameObject _particles;
    public GameObject Particles { get { return _particles; } }

    public Animator Anim { get { return anim; } }

    public override void Start()
    {
        base.Start();
  
        //Components
        _fsm = new StateMachine();

        //StateMachine
        _fsm.AddState(StateName.CD_Idle, new CD_Idle(_fsm, this));
        _fsm.AddState(StateName.CD_Charge, new CD_Charge(_fsm, this));
        _fsm.AddState(StateName.CD_LoadCharge, new CD_LoadCharge(_fsm, this));
        _fsm.ChangeState(StateName.CD_Idle);

        OnUpdate += FalseUpdate;
    }
    void FalseUpdate()
    {
        /*if(CanSeePlayer())*/ _fsm.Update();
    }
    public Vector3 GetDistanceToPlayer()
    {
        return DistanceToPlayer();
    }
    public void LookAtPlayer()
    {
        transform.right = GetDistanceToPlayer().normalized;
    }
    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_ChargeDrone.Instance.pool.ReturnObject(this);
        _fsm.ChangeState(StateName.CD_Idle);
    }

    public bool SeePlayer()
    {
        return CanSeePlayer();
    }
}

public class CD_Idle : IState
{
    StateMachine _fsm;
    Enemy_ChargeDrone _enemy;

    float _currentIdleTime;

    public CD_Idle(StateMachine fsm, Enemy_ChargeDrone enemy)
    {
        _fsm = fsm;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        _enemy.Particles.SetActive(false);
        _currentIdleTime = 0;

        _enemy.Anim.Play("ChargeDrone_Idle");
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate() 
    { 

    }

    public void OnUpdate()
    {
        if(!_enemy.SeePlayer()) return;
        _currentIdleTime += Time.deltaTime;
        _enemy.LookAtPlayer();

        if(_currentIdleTime > _enemy.IdleWaitTime) _fsm.ChangeState(StateName.CD_LoadCharge);
    }
}
public class CD_LoadCharge : IState
{
    StateMachine _fsm;
    Enemy_ChargeDrone _enemy;

    public CD_LoadCharge(StateMachine fsm, Enemy_ChargeDrone enemy)
    {
        _fsm = fsm;
        _enemy = enemy;

    }

    public void OnEnter()
    {
        _enemy.Anim.Play("ChargeDrone_LoadCharge");
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (_enemy.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < .9f) return;
       
        _fsm.ChangeState(StateName.CD_Charge);
    }
}
public class CD_Charge : IState
{
    StateMachine _fsm;
    Enemy_ChargeDrone _enemy;
    GameManager _gameManager;

    float _currentChargeDistance;

    public CD_Charge(StateMachine fsm, Enemy_ChargeDrone enemy)
    {
        _fsm = fsm;
        _enemy = enemy;

        _gameManager = GameManager.instance;
    }

    public void OnEnter()
    {
        _enemy.Particles.SetActive(true);
        _enemy.Anim.Play("ChargeDrone_Charge"); 
    }

    public void OnExit()
    {
        _currentChargeDistance = 0;
    }

    public void OnFixedUpdate() 
    { 
    
    }

    public void OnUpdate()
    {
        _currentChargeDistance += Time.deltaTime;
        if (_currentChargeDistance > _enemy.ChargeDistance || Physics2D.Raycast(_enemy.transform.position, _enemy.transform.right, .6f, _gameManager.BorderLayer))
            _fsm.ChangeState(StateName.CD_Idle);

        Charge();
    }
    void Charge()
    {
        _enemy.transform.position += _enemy.transform.right * _enemy.ChargeSpeed * Time.deltaTime;
    }

}