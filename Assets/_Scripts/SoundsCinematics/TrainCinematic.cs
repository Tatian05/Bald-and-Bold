using System.Collections;
using UnityEngine;
public class TrainCinematic : MonoBehaviour
{
    [SerializeField] AudioSource _bocinaTrenAS;
    [SerializeField] AudioClip[] _bocinaClips;

    System.Action _defaultState;
    [SerializeField] float _time, _timer;
    int _index;
    void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(Wait());
        Helpers.LevelTimerManager.OnLevelDefeat += () => _defaultState -= CurrentState;
        _time = Helpers.LevelTimerManager.LevelMaxTime / (_bocinaClips.Length);
        _defaultState += CurrentState;
        _bocinaTrenAS.Play();
    }
    void Update()
    {
        _defaultState?.Invoke();
    }
    void CurrentState()
    {
        _timer += Time.deltaTime;
        if (_timer >= _time)
        {
            //_index = (_index + 1) % _bocinaClips.Length;
            //Debug.Log(_index);
            _bocinaTrenAS.clip = _bocinaClips[_index++ % _bocinaClips.Length];
            _bocinaTrenAS.Play();
            _timer = 0;
        }
    }
    IEnumerator Wait()
    {
        var currentState = _defaultState;
        _defaultState = delegate { };
        yield return new WaitForSeconds(1f);
        _defaultState = currentState;
    }
}
