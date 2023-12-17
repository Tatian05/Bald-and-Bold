using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class LevelLightsManager : MonoBehaviour
{
    Action OnUpdate;

    [SerializeField]List<Light2D> _lights;
    [SerializeField] Color[] _colors;
    [SerializeField] [Range(0.0f, 1.0f)] float _startBlinkingLights;
    [SerializeField] SpriteRenderer _onOffLight;
    [SerializeField] Color[] _onOffLightColors;

    List<BrokenLight> _brokenLights;
    private void Start()
    {
        _lights = GetComponentsInChildren<Light2D>().ToList();
        _brokenLights = GetComponentsInChildren<BrokenLight>().ToList();
        _onOffLight = GameObject.Find("IMG_OnOffLight_Color").GetComponent<SpriteRenderer>();

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
        OnUpdate += CheckForBlinkingLights;
    }

    public void StopLights()
    {
        _onOffLight.color = _onOffLightColors[0];
        foreach (var item in _lights)
        {
            item.color = _colors[0];
        }
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
        foreach (var item in _brokenLights)
            item.CanBlink();
    }

    public void StopBLinkingLights()
    {
        foreach (var item in _brokenLights)
            item.CantBlink();
    }

    public void RemoveBrokenLight(BrokenLight brokenLight, Light2D light2D)
    {
        _brokenLights.Remove(brokenLight);
        _lights.Remove(light2D);
    }
}