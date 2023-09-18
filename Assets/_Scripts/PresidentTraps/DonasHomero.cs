using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
public class DonasHomero : MonoBehaviour
{
    [SerializeField] GameObject _palanca, _presidentBody;
    [SerializeField] List<GameObject> _donasInCinta, _donasToSpawn;
    [SerializeField] InfiniteRotation[] _cintas;
    [SerializeField] Transform _mouth, _donasSpawner;
    [SerializeField, Range(0, 3f)] float _go, _back, _wait;

    Action _eatingDona = delegate { };
    GameObject _donaAte;
    EventFSM<DonasStates> _myFsm;
    enum DonasStates { GO, BACK, WAIT }
    void Start()
    {
        Helpers.LevelTimerManager.OnLevelDefeat += () => enabled = false;

        for (int i = 0; i < _cintas.Length; i++) _cintas[i].enabled = false;

        var go = new State<DonasStates>("GO");
        var back = new State<DonasStates>("BACK");
        var wait = new State<DonasStates>("WAIT");

        StateConfigurer.Create(go).SetTransition(DonasStates.BACK, back).Done();
        StateConfigurer.Create(back).SetTransition(DonasStates.WAIT, wait).Done();
        StateConfigurer.Create(wait).SetTransition(DonasStates.GO, go).Done();

        float counter = 0;
        Vector3 startRotation = _palanca.transform.eulerAngles;
        Vector3 palancaRotation = new Vector3(0, 0, -25f);

        #region GO

        go.OnEnter += x =>
        {
            counter = 0;
            for (int i = 0; i < _cintas.Length; i++) _cintas[i].enabled = true;
        };

        go.OnUpdate += () =>
        {
            counter += Time.deltaTime;
            Palanca(startRotation, palancaRotation, _go, counter);

            for (int i = 0; i < _donasInCinta.Count; i++)
                _donasInCinta[i].transform.position += -Vector3.right * 2 * Time.deltaTime;

            if ((counter / _go) >= 1f) _myFsm.SendInput(DonasStates.BACK);
        };

        #endregion

        #region BACK

        back.OnEnter += (x) =>
        {
            counter = 0;
            for (int i = 0; i < _cintas.Length; i++) _cintas[i].enabled = false;

            var newDona = _donasToSpawn[0];
            newDona.GetComponent<Rigidbody2D>().simulated = true;
            newDona.transform.localScale = Vector3.one;
            newDona.transform.eulerAngles = Vector3.zero;
            newDona.SetActive(true);
            _donasInCinta.Add(newDona);
            _donasToSpawn.Remove(newDona);
        };

        back.OnUpdate += () =>
        {
            counter += Time.deltaTime;
            Palanca(palancaRotation, startRotation, _back, counter);

            if ((counter / _back) >= 1f) _myFsm.SendInput(DonasStates.WAIT);
        };

        #endregion

        #region WAIT 

        wait.OnEnter += x => counter = 0;

        wait.OnUpdate += () =>
        {
            counter += Time.deltaTime;
            if ((counter / _wait) >= 1f) _myFsm.SendInput(DonasStates.GO);
        };

        #endregion

        Helpers.LevelTimerManager.OnLevelStart += () => _myFsm = new EventFSM<DonasStates>(go);
    }
    void Update()
    {
        _myFsm?.Update();
        _eatingDona?.Invoke();
    }
    void Palanca(Vector3 currentRot, Vector3 goalRot, float time, float timer)
    {
        _palanca.transform.eulerAngles = Vector3.Lerp(currentRot, goalRot, timer / time);
    }

    float _donaTimer;
    Vector3 _presiBodyScale = Vector3.one;
    void Dona()
    {
        if (!_donaAte) return;

        _donaTimer += Time.deltaTime;
        _donaAte.transform.localScale = Vector3.Lerp(_donaAte.transform.localScale, new Vector3(.5f, .5f, 1), _donaTimer / 1f);
        _donaAte.transform.position = Vector3.Lerp(_donaAte.transform.position, _mouth.position, _donaTimer / 1f);
        _presidentBody.transform.localScale = Vector3.Lerp(_presidentBody.transform.localScale, _presiBodyScale + Vector3.one * .05f, _donaTimer / 1f);
        Vector3 lerpAmount =  _presidentBody.transform.localScale - _presiBodyScale;

        if (_donaTimer >= 1f)
        {
            _donaTimer = 0;
            _presiBodyScale = _presidentBody.transform.localScale;
            _donasInCinta.Remove(_donaAte);
            _donasToSpawn.Add(_donaAte);
            _donaAte.transform.position = _donasSpawner.position;
            _donaAte.SetActive(false);
            _donaAte = null;
            _eatingDona = delegate { };
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        _donaAte = collision.gameObject;
        _donaAte.GetComponent<Rigidbody2D>().simulated = false;
        _eatingDona = Dona;
    }
}
