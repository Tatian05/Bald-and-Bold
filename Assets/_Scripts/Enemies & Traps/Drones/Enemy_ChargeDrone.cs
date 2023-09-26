using UnityEngine;
public class Enemy_ChargeDrone : Enemy
{
    [SerializeField] float _idleWaitTime;
    [SerializeField] float _chargeSpeed;
    [SerializeField] float _chargeDistance;
    [SerializeField] GameObject _particles;
    enum ChargeDroneStates { Idle, LoadCharge, Charge }
    EventFSM<ChargeDroneStates> _myFSM;
    public override void Start()
    {
        base.Start();

        var IDLE = new State<ChargeDroneStates>("IDLE");
        var LOADCHARGE = new State<ChargeDroneStates>("LOAD_CHARGE");
        var CHARGE = new State<ChargeDroneStates>("CHARGE");

        StateConfigurer.Create(IDLE).SetTransition(ChargeDroneStates.LoadCharge, LOADCHARGE).Done();
        StateConfigurer.Create(LOADCHARGE).SetTransition(ChargeDroneStates.Charge, CHARGE).Done();
        StateConfigurer.Create(CHARGE).SetTransition(ChargeDroneStates.Idle, IDLE).Done();

        #region IDLE

        float currentIdleTime = 0;
        IDLE.OnEnter += x =>
        {
            _particles.SetActive(false);
            currentIdleTime = 0;

            anim.Play("ChargeDrone_Idle");
        };

        IDLE.OnUpdate += delegate
        {
            if (!SeePlayer()) return;
            currentIdleTime += Time.deltaTime;
            LookAtPlayer();

            if (currentIdleTime > _idleWaitTime) _myFSM.SendInput(ChargeDroneStates.LoadCharge);
        };

        #endregion

        #region LOADCHARGE

        LOADCHARGE.OnEnter += x => anim.Play("ChargeDrone_LoadCharge");
        LOADCHARGE.OnUpdate += delegate
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < .9f) return;
            _myFSM.SendInput(ChargeDroneStates.Charge);
        };

        #endregion

        #region CHARGE
        LayerMask borderLayer = gameManager.BorderLayer;
        float currentChargeDistance = 0;
        CHARGE.OnEnter += x =>
        {
            _particles.SetActive(true);
            anim.Play("ChargeDrone_Charge");
            currentChargeDistance = 0;
        };

        CHARGE.OnUpdate += delegate
        {
            currentChargeDistance += Time.deltaTime;
            if (currentChargeDistance > _chargeDistance || Physics2D.Raycast(transform.position, transform.right, .6f, borderLayer))
                _myFSM.SendInput(ChargeDroneStates.Idle);

            transform.position += transform.right * _chargeSpeed * Time.deltaTime;
        };

        #endregion

        _myFSM = new EventFSM<ChargeDroneStates>(IDLE);
    }
    public override void Update()
    {
        _myFSM.Update();
    }
    public void LookAtPlayer()
    {
        transform.right = DistanceToPlayer().normalized;
    }
    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_ChargeDrone.Instance.pool.ReturnObject(this);
        _myFSM.SendInput(ChargeDroneStates.Idle);
    }

    public bool SeePlayer()
    {
        return CanSeePlayer();
    }
}