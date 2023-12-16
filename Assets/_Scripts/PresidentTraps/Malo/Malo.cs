using UnityEngine;
public class Malo : MonoBehaviour
{
    Animator _animator;
    [SerializeField] string _winAnimationName, _onEnemyKilledAnimation, _levelStartAnimName;
    [SerializeField] bool _setAnimOnLevelStart = false;
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_setAnimOnLevelStart) EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, PlayAnim);
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => _animator.Play(_onEnemyKilledAnimation);
        Helpers.LevelTimerManager.OnLevelDefeat += () => _animator.Play(_winAnimationName);
    }
    void PlayAnim(params object[] param) { _animator.Play(_levelStartAnimName); }
    public void PlaySound() { Helpers.AudioManager.PlaySFX("Inflador"); }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, PlayAnim);
    }
}
