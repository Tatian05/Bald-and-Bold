using UnityEngine;
using DG.Tweening;
public class Enemy_ChargeDrone : Enemy
{
    [SerializeField] float _idleWaitTime;
    [SerializeField] float _chargeSpeed;
    [SerializeField] float _chargeDistance;
    [SerializeField] GameObject _particles;
    enum ChargeDroneStates { Idle, Wait_To_Charge, LoadCharge, Charge, Lost }
    EventFSM<ChargeDroneStates> _myFSM;
    State<ChargeDroneStates> IDLE;
    Transform _spriteParentTransform;
    public override void Start()
    {
        base.Start();
        _spriteParentTransform = anim.transform;
        IDLE = new State<ChargeDroneStates>("IDLE");
        var WAIT_TO_CHARGE = new State<ChargeDroneStates>("WAIT_TO_CHARGE");
        var LOADCHARGE = new State<ChargeDroneStates>("LOAD_CHARGE");
        var CHARGE = new State<ChargeDroneStates>("CHARGE");
        var LOST = new State<ChargeDroneStates>("LOST");

        StateConfigurer.Create(IDLE).SetTransition(ChargeDroneStates.Wait_To_Charge, WAIT_TO_CHARGE).Done();
        StateConfigurer.Create(WAIT_TO_CHARGE).SetTransition(ChargeDroneStates.Idle, IDLE).
                                               SetTransition(ChargeDroneStates.LoadCharge, LOADCHARGE).
                                               SetTransition(ChargeDroneStates.Lost, LOST).Done();
        StateConfigurer.Create(LOADCHARGE).SetTransition(ChargeDroneStates.Idle, IDLE).
                                           SetTransition(ChargeDroneStates.Charge, CHARGE).
                                           SetTransition(ChargeDroneStates.Lost, LOST).Done();
        StateConfigurer.Create(CHARGE).SetTransition(ChargeDroneStates.Idle, IDLE).SetTransition(ChargeDroneStates.Lost, LOST).Done();
        StateConfigurer.Create(LOST).SetTransition(ChargeDroneStates.Idle, IDLE).Done();

        #region IDLE

        float currentIdleTime = 0;
        IDLE.OnEnter += x =>
        {
            _particles.SetActive(false);
            SetSign(false);
            anim.Play("ChargeDrone_Idle");
        };

        IDLE.OnUpdate += delegate {
            if (!_enemyDataSO.playerPivot) return;
            if (CanSeePlayer()) _myFSM.SendInput(ChargeDroneStates.Wait_To_Charge);
        };


        #endregion

        #region WAIT_TO_CHARGE 

        WAIT_TO_CHARGE.OnEnter += x => {
            currentIdleTime = 0;
            SetSign(true, _agroSign);
            transform.DOScale(new Vector2(1.25f, 1.25f), .1f).SetLoops(2, LoopType.Yoyo);
        };
        WAIT_TO_CHARGE.OnUpdate += delegate
        {
            currentIdleTime += Time.deltaTime;
            LookAtPlayer();

            if (currentIdleTime > _idleWaitTime) _myFSM.SendInput(ChargeDroneStates.LoadCharge);
        };
        WAIT_TO_CHARGE.OnExit += x => currentIdleTime = 0;
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
            if (currentChargeDistance > _chargeDistance || Physics2D.Raycast(_spriteParentTransform.position, _spriteParentTransform.right, .6f, borderLayer))
                _myFSM.SendInput(ChargeDroneStates.Idle);

            transform.position += _spriteParentTransform.right * _chargeSpeed * Time.deltaTime;
        };
        CHARGE.OnExit += x => currentChargeDistance = 0;

        #endregion

        #region LOST
        float lostTimer = 0;
        LOST.OnEnter += x => SetSign(true, _lostSign);
        LOST.OnUpdate += delegate
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= _enemyDataSO.lostTime) _myFSM.SendInput(ChargeDroneStates.Idle);
        };
        LOST.OnExit += x => SetSign(false);

        #endregion

        if (Helpers.LevelTimerManager.LevelStarted) _myFSM = new EventFSM<ChargeDroneStates>(IDLE);
        else EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
    }
    void StartFSM(params object[] param) { _myFSM = new EventFSM<ChargeDroneStates>(IDLE); }
    protected override void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, StartFSM);
        base.OnDestroy();
    }
    void Update()
    {
        _myFSM?.Update();
    }
    protected override void PlayerInvisibleConsumable(params object[] param)
    {
        _myFSM.SendInput((bool)param[0] ? ChargeDroneStates.Lost : ChargeDroneStates.Idle);
    }
    void LookAtPlayer() { _spriteParentTransform.right = DistanceToPlayer().normalized; }
    public override void ReturnObject()
    {
        base.ReturnObject();
        _myFSM.SendInput(ChargeDroneStates.Idle);
        FRY_Enemy_ChargeDrone.Instance.pool.ReturnObject(this);
    }
}