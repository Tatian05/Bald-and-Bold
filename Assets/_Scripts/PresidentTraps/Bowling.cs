using UnityEngine;
using System;
using System.Linq;
public class Bowling : MonoBehaviour
{
    [SerializeField] float _time, _timeGoing, _timeComingBack, _timeWaiting, _timer;
    [SerializeField] GameObject[] _bolosContainer;
    [SerializeField] GameObject[] _bolos;
    Vector3[] _wallsInitialPositions, _wallsFinalPositions;

    int _counter;
    Action _wallAction = delegate { };
    void Start()
    {
        _bolos = _bolosContainer.Select(x => x.transform.GetChild(0).GetChild(0).gameObject).ToArray();
        _time = Helpers.LevelTimerManager.LevelMaxTime / _bolosContainer.Length;
        _timeGoing = _time * .45f;
        _timeWaiting = _time * .1f;
        _timeComingBack = _time - (_timeWaiting + _timeGoing);
        _wallsInitialPositions = _bolosContainer.Select(x => x.transform.position).ToArray();
        _wallsFinalPositions = _wallsInitialPositions.Select(x => x - Vector3.up * 2.75f).ToArray();

        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, WallAction);

        Helpers.LevelTimerManager.RedButton += () =>
        {
            _bolos.Last().transform.SetParent(GameObject.Find("Cinematic").transform);
            _bolos.Last().AddComponent<Rigidbody2D>().freezeRotation = true;
        };
    }
    void WallAction(params object[] param) => _wallAction = BoloGoing;
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, WallAction);      
    }
    void Update()
    {
        _wallAction();
    }

    void BoloGoing()
    {
        _timer += Time.deltaTime;
        _bolosContainer[_counter].transform.position = Vector3.Lerp(_wallsInitialPositions[_counter], _wallsFinalPositions[_counter], _timer / _timeGoing);

        if (_timer / _timeGoing >= 1)
        {
            _timer = 0;
            _wallAction = BoloWaiting;
            _bolos[_counter].transform.SetParent(GameObject.Find("Cinematic").transform);
            var rb = _bolos[_counter].GetComponent<Rigidbody2D>();
            if (rb) rb.isKinematic = false;
        }
    }
    void BoloWaiting()
    {
        _timer += Time.deltaTime;
        if (_timer / _timeWaiting >= 1)
        {
            _timer = 0;
            _wallAction = BoloComingBack;
        }
    }
    void BoloComingBack()
    {
        _timer += Time.deltaTime;
        _bolosContainer[_counter].transform.position = Vector3.Lerp(_wallsFinalPositions[_counter], _wallsInitialPositions[_counter], _timer / _timeComingBack);

        if (_timer / _timeComingBack >= 1)
        {
            _timer = 0;
            ++_counter;
            _wallAction = _counter >= _bolosContainer.Length ? (Action)delegate { } : BoloGoing;
        }
    }
}
