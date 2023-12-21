using System.Collections;
using UnityEngine;
using System;
public class LevelTimerManager : Singleton<LevelTimerManager>
{
    [SerializeField] float _timer;
    [SerializeField] float _levelMaxTime;
    [SerializeField] float _timeToDiscount;
    public float Timer { get { return _timer; } set { _timer = value; } }
    public float LevelMaxTime { get { return _levelMaxTime; } }
    public float TimeToDiscount { get { return _timeToDiscount; } }
    public bool TrapStopped { get { return _stopTrap; } }
    public bool LevelStarted { get { return _timer > 0; } }

    bool _stopTrap, _firstTime, _wonLevel;

    public event Action RedButton;
    public event Action OnLevelDefeat;
    void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += StopTrap;
        RedButton += WinLevel;
        EventManager.SubscribeToEvent(Contains.ON_ROOM_WON, PlayRedButton);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_ROOM_WON, PlayRedButton);
    }
    void PlayRedButton(params object[] param) { RedButton(); }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            StartLevelTimer();
            _timer = _levelMaxTime;
        }
        else if (Input.GetKeyDown(KeyCode.F1))
            EventManager.TriggerEvent(Contains.ON_ROOM_WON);
    }
    private void OnDisable()
    {
        RedButton -= WinLevel;
    }
    public void StartLevelTimer() { if (enabled) StartCoroutine(LevelTimer()); }
    IEnumerator LevelTimer()
    {
        if (_firstTime) yield break;
        _firstTime = true;
        EventManager.TriggerEvent(Contains.ON_LEVEL_START);
        CustomTime.SetTimeScale(1f);
        WaitForSeconds wait = new WaitForSeconds(_timeToDiscount);
        while (_timer <= _levelMaxTime)
        {
            if (_wonLevel) yield break;                                                                     //Cuando termina el nivel
            if (_stopTrap || CustomTime.TimeScale <= 0) yield return wait;                                  //Cuando muere un enemigo
            _stopTrap = false;
            _timer += CustomTime.DeltaTime;
            yield return null;
        }
        CustomTime.SetTimeScale(0f);
        OnLevelDefeat();
    }
    public void WinLevel()
    {
        _wonLevel = true;
        CustomTime.SetTimeScale(0f);
        Helpers.GameManager.EnemyManager.OnEnemyKilled -= StopTrap;
    }
    void StopTrap()
    {
        _stopTrap = true;
    }
}
