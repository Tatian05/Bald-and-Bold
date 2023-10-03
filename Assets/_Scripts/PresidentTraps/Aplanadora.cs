using UnityEngine;
public class Aplanadora : MonoBehaviour
{
    [SerializeField] Transform _weel;
    [SerializeField] Transform _aplanadoraWeel;
    [SerializeField] float _speed;

    bool _stop;
    LevelTimerManager _levelTimerManager;
    Vector3 _dir = Vector3.forward;
    private void Start()
    {
        _levelTimerManager = Helpers.LevelTimerManager;
        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, Stop);
    }
    void Stop(params object[] param) => _stop = true;
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, Stop);    
    }
    private void Update()
    {
        bool stopWeel = _levelTimerManager.TrapStopped || _stop;

        RotateWeel(_weel, _speed, stopWeel);
        RotateWeel(_aplanadoraWeel, _speed * .5f, stopWeel);
    }
    public void IncreaseSpeed(float amount) { _speed *= amount; }
    public void ReverseSpeed() { _speed *= -1; }
    public void StopAplanadora() { _speed = 0; }

    void RotateWeel(Transform weel, float speed, bool stopWeel)
    {
        weel.Rotate(stopWeel ? _dir * speed : Vector3.zero);
    }
}
