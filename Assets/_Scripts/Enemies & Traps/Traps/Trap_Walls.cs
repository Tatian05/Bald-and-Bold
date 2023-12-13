using UnityEngine;
public class Trap_Walls : MonoBehaviour
{
    enum WallsStates { GO, BACK, WAIT_TOP, WAIT_BASE }
    Vector3[] _initialPos = new Vector3[2];
    Vector3 _targetPos;
    EventFSM<WallsStates> _myFsm;
    [SerializeField] GameObject _damageCollider;

    [SerializeField] Transform[] _walls;

    [Header("VARIABLES")]
    [SerializeField] float _waitBaseTime;
    [SerializeField] float _waitTopTime;
    [SerializeField] float _goTopTime;
    [SerializeField] float _goBaseTime;
    private void Start()
    {
        for (int i = 0; i < _walls.Length; i++)
            _initialPos[i] = _walls[i].position;

        _targetPos = (_walls[1].position + _walls[0].position) * .5f;

        _damageCollider.SetActive(false);

        var go = new State<WallsStates>("GO");
        var back = new State<WallsStates>("BACK");
        var wait_top = new State<WallsStates>("WAIT_TOP");
        var wait_base = new State<WallsStates>("WAIT_BASE");

        StateConfigurer.Create(wait_base).SetTransition(WallsStates.GO, go).Done();
        StateConfigurer.Create(go).SetTransition(WallsStates.WAIT_TOP, wait_top).Done();
        StateConfigurer.Create(wait_top).SetTransition(WallsStates.BACK, back).Done();
        StateConfigurer.Create(back).SetTransition(WallsStates.WAIT_BASE, wait_base).Done();

        float timer = 0;

        #region WAIT_BASE

        wait_base.OnEnter += x => timer = 0;

        wait_base.OnUpdate += () =>
        {
            timer += Time.deltaTime;
            if (timer >= _waitBaseTime) _myFsm.SendInput(WallsStates.GO);
        };

        #endregion

        #region GO

        go.OnEnter += x =>
        {
            timer = 0;
            _damageCollider.SetActive(true);
        };

        go.OnUpdate += () =>
        {
            timer += Time.deltaTime;

            for (int i = 0; i < _walls.Length; i++)
                _walls[i].position = Vector3.Lerp(_initialPos[i], _targetPos, timer / _goTopTime);

            if (timer / _goTopTime >= 1) _myFsm.SendInput(WallsStates.WAIT_TOP);
        };

        go.OnExit += x =>
        {
            Helpers.AudioManager.PlaySFX("WallsTrap");
            _damageCollider.SetActive(false);
        };

        #endregion

        #region WAIT_TOP

        wait_top.OnEnter += x => timer = 0;

        wait_top.OnUpdate += () =>
        {
            timer += Time.deltaTime;
            if (timer >= _waitTopTime) _myFsm.SendInput(WallsStates.BACK);
        };

        #endregion

        #region BACK

        back.OnEnter += x => timer = 0;

        back.OnUpdate += () =>
        {
            timer += Time.deltaTime;

            for (int i = 0; i < _walls.Length; i++)
                _walls[i].position = Vector3.Lerp(_targetPos, _initialPos[i], timer / _goTopTime);

            if (timer / _goBaseTime >= 1) _myFsm.SendInput(WallsStates.WAIT_BASE);
        };

        #endregion

        _myFsm = new EventFSM<WallsStates>(wait_base);
    }

    private void Update()
    {
        _myFsm.Update();
    }
}
