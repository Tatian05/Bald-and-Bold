using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class PresidentWallsTrap : MonoBehaviour
{
    [SerializeField] float _time, _timeGoing, _timeComingBack, _timeWaiting, _timer;
    [SerializeField] GameObject[] _walls;
    Vector3[] _wallsInitialPositions, _wallsFinalPositions;

    int _counter;
    Action _wallAction = delegate { };
    void Start()
    {
        _time = Helpers.LevelTimerManager.LevelMaxTime / (_walls.Length - 1);
        _timeGoing = _time * .01f;
        _timeWaiting = _time * .2f;
        _timeComingBack = _time - (_timeWaiting + _timeGoing);
        _wallsInitialPositions = _walls.Select(x => x.transform.position).ToArray();
        _wallsFinalPositions = _wallsInitialPositions.Select(x => x - Vector3.up * 2.25f).ToArray();

        Helpers.LevelTimerManager.OnLevelStart += () => _wallAction = WallGoing;
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(Wait());
    }
    void Update()
    {
        _wallAction();
    }

    void WallGoing()
    {
        _timer += Time.deltaTime;
        _walls[_counter].transform.position = Vector3.Lerp(_wallsInitialPositions[_counter], _wallsFinalPositions[_counter], _timer / _timeGoing);

        if (_timer / _timeGoing >= 1)
        {
            _timer = 0;
            _wallAction = WallWaiting;
        }
    }
    void WallWaiting()
    {
        _timer += Time.deltaTime;
        if (_timer / _timeWaiting >= 1)
        {
            _timer = 0;
            _wallAction = WallComingBack;
        }
    }
    void WallComingBack()
    {
        _timer += Time.deltaTime;
        _walls[_counter].transform.position = Vector3.Lerp(_wallsFinalPositions[_counter], _wallsInitialPositions[_counter], _timer / _timeComingBack);

        if (_timer / _timeComingBack >= 1)
        {
            _timer = 0;
            ++_counter;
            _wallAction = _counter >= _walls.Length - 1 ? (Action)delegate { } : WallGoing;
        }
    }

    IEnumerator Wait()
    {
        var currentAction = _wallAction;
        _wallAction = delegate { };
        yield return new WaitForSeconds(1f);
        _wallAction = currentAction;
    }
}
