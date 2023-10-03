using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelLightsManager : MonoBehaviour
{
    public static LevelLightsManager Instance;

    Action OnUpdate;

    StateMachine _fsm;

    [SerializeField]List<Light2D> _lights;
    public List<Light2D> Lights { get { return _lights; } private set { } }

    [SerializeField] Color[] _colors;
    public Color[] Colors { get { return _colors; } private set { } }

    [SerializeField] [Range(0.0f, 1.0f)] float _startBlinkingLights;

    [SerializeField] SpriteRenderer _onOffLight;
    public SpriteRenderer OnOffLight { get { return _onOffLight; }  private set { } }

    [SerializeField] Color[] _onOffLightColors;
    public Color[] OnOffLightColors { get { return _onOffLightColors; } private set { } }

    bool _lightsAreBlinking = false;
    public bool LightsAreBlinking { get { return _lightsAreBlinking; } private set { } }

    List<BrokenLight> _brokenLights;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _lights = GetComponentsInChildren<Light2D>().ToList();
        _brokenLights = GetComponentsInChildren<BrokenLight>().ToList();
        _onOffLight = GameObject.Find("IMG_OnOffLight_Color").GetComponent<SpriteRenderer>();

        _fsm = new StateMachine();

        _fsm.AddState(StateName.LIGHT_GoingRed, new GoingRed(_fsm, this));
        _fsm.AddState(StateName.LIGHT_Normal, new GoingNormal(_fsm, this));
        _fsm.ChangeState(StateName.LIGHT_GoingRed);

        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => _fsm.ChangeState(StateName.LIGHT_Normal);
        _onOffLight.color = _onOffLightColors[1];

        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, StartLights);

        Helpers.LevelTimerManager.RedButton += StopLights;
        Helpers.LevelTimerManager.OnLevelDefeat += StopLights;
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, StartLights);
    }
    private void Update()
    {
        OnUpdate?.Invoke();
    }

    public void StartLights(params object[] param)
    {
        OnUpdate += _fsm.Update;
        OnUpdate += CheckForBlinkingLights;
    }

    public void StopLights()
    {
        _onOffLight.color = _onOffLightColors[0];
        foreach (var item in _lights)
        {
            item.color = _colors[0];
        }
        OnUpdate -= _fsm.Update;
        StopBLinkingLights();
    }

    void CheckForBlinkingLights()
    {
        if (Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime >= _startBlinkingLights)
        {
            StartBlinkLights();
            OnUpdate -= CheckForBlinkingLights;
        }
    }

    public void StartBlinkLights()
    {
        _lightsAreBlinking = true;
        foreach (var item in _brokenLights)
        {
            item.CanBlink();
        }
    }

    public void StopBLinkingLights()
    {
        foreach (var item in _brokenLights)
        {
            item.CantBlink();
        }
    }

    public void RemoveBrokenLight(BrokenLight brokenLight, Light2D light2D)
    {
        _brokenLights.Remove(brokenLight);
        _lights.Remove(light2D);
    }
}

public class GoingRed : IState
{
    StateMachine _fsm;
    LevelLightsManager _manager;

    float _currentTimer;

    public GoingRed(StateMachine fsm, LevelLightsManager manager)
    {
        _fsm = fsm;
        _manager = manager;
    }

    public void OnEnter()
    {
        //_currentTimer = 0;
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        _currentTimer += Time.deltaTime / Helpers.LevelTimerManager.LevelMaxTime;
        foreach (var item in _manager.Lights)
        {
            item.color = Color.Lerp(_manager.Colors[0], _manager.Colors[1], _currentTimer);
        }
    }
}
public class GoingNormal : IState
{
    StateMachine _fsm;
    LevelLightsManager _manager;

    float _currentTimer;

    public GoingNormal(StateMachine fsm, LevelLightsManager manager)
    {
        _fsm = fsm;
        _manager = manager;
    }

    public void OnEnter()
    {
        _currentTimer = 0;

        if (_manager.LightsAreBlinking) _manager.StopBLinkingLights();
        _manager.OnOffLight.color = _manager.OnOffLightColors[0];

        foreach (var item in _manager.Lights)
        {
            item.color = _manager.Colors[0];
        }
    }

    public void OnExit()
    {
        if(_manager.LightsAreBlinking) _manager.StartBlinkLights();

        _manager.OnOffLight.color = _manager.OnOffLightColors[1];
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer >= Helpers.LevelTimerManager.TimeToDiscount) _fsm.ChangeState(StateName.LIGHT_GoingRed);
    }
}