using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class GlobalLightEffect : MonoBehaviour
{
    enum ColorPhases { Empty, GoingRed, GoingWhite }
    Volume _volume;
    ColorAdjustments _colorAdjustments;
    LevelTimerManager _levelTimerManager;
    EventFSM<ColorPhases> _myFsm;

    [SerializeField, ColorUsage(true, true)] Color _signalColor;
    [SerializeField] bool _cancelEffect;
    [SerializeField] bool _startBlinking;
    void Start()
    {
        _levelTimerManager = Helpers.LevelTimerManager;
        AudioManager audioManager = Helpers.AudioManager;
        if (_cancelEffect)
        {
            enabled = false;
            return;
        }

        _volume = GetComponent<Volume>();
        _volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);

        if (_levelTimerManager)
        {
            _levelTimerManager.RedButton += OnLevelFinish;
            _levelTimerManager.OnLevelDefeat += OnLevelFinish;
        }

        var goingRed = new State<ColorPhases>("GoingRed");
        var goingWhite = new State<ColorPhases>("GoingWhite");
        var empty = new State<ColorPhases>("Empty");

        StateConfigurer.Create(empty).SetTransition(ColorPhases.GoingRed, goingRed).Done();
        StateConfigurer.Create(goingRed).SetTransition(ColorPhases.GoingWhite, goingWhite).Done();
        StateConfigurer.Create(goingWhite).SetTransition(ColorPhases.GoingRed, goingRed).Done();

        float timer = 0;

        goingRed.OnEnter += x => audioManager.PlaySFX("Sirena");
        goingRed.OnUpdate += delegate
        {
            timer += CustomTime.DeltaTime;
            _colorAdjustments.colorFilter.value = Color.Lerp(Color.white, _signalColor, timer / .5f);

            if (timer / 0.5f >= 1) _myFsm.SendInput(ColorPhases.GoingWhite);
        };
        goingRed.OnExit += delegate { timer = 0; };

        goingWhite.OnUpdate += delegate
        {
            timer += CustomTime.DeltaTime;
            _colorAdjustments.colorFilter.value = Color.Lerp(_signalColor, Color.white, timer / .5f);

            if (timer / 0.5f >= 1) _myFsm.SendInput(ColorPhases.GoingRed);
        };
        goingWhite.OnExit += delegate { timer = 0; };

        empty.OnUpdate += delegate
        {
            if (_levelTimerManager.Timer >= _levelTimerManager.LevelMaxTime - 10)
                _myFsm.SendInput(ColorPhases.GoingRed);
        };

        _myFsm = new EventFSM<ColorPhases>(_startBlinking ? goingRed : empty);
    }
    private void Update()
    {
        _myFsm?.Update();
    }
    void OnLevelFinish()
    {
        _myFsm = null;
        _colorAdjustments.colorFilter.value = Color.white;
    }
    private void OnDisable()
    {
        if (_levelTimerManager)
        {
            _levelTimerManager.RedButton -= OnLevelFinish;
            _levelTimerManager.OnLevelDefeat -= OnLevelFinish;
        }
    }
}
