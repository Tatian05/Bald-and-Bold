using System.Collections;
using UnityEngine;
public class Cocodrilo : MonoBehaviour
{
    System.Action _state;
    Animator _animator;
    float _time, _timer;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _time = Helpers.LevelTimerManager.LevelMaxTime / 16;
        _state = CurrentState;
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(Wait());
    }
    private void Update()
    {
        _state?.Invoke();
    }
    void CurrentState()
    {
        _timer += Time.deltaTime;
        if (_timer >= _time)
        {
            _animator.Play("Cocodrilo V");
            _timer = 0;
        }
    }
    IEnumerator Wait()
    {
        var action = _state;
        _state = delegate { };
        yield return new WaitForSeconds(1f);
        _state = action;
    }
}
