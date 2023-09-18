using UnityEngine;
public class Malo : MonoBehaviour
{
    Animator _animator;
    [SerializeField] string _winAnimationName, _onEnemyKilledAnimation, _levelStartAnimName;
    [SerializeField] bool _setAnimOnLevelStart = false;
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_setAnimOnLevelStart) Helpers.LevelTimerManager.OnLevelStart += delegate { _animator.Play(_levelStartAnimName); };
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => _animator.Play(_onEnemyKilledAnimation);
        Helpers.LevelTimerManager.OnLevelDefeat += () => _animator.Play(_winAnimationName);
    }
}
