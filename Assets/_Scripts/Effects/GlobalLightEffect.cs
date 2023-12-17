using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class GlobalLightEffect : MonoBehaviour
{
    enum ColorPhases { Empty, GoingRed, GoingWhite }
    Volume _volume;
    ColorAdjustments _colorAdjustments;
    LevelTimerManager _levelTimerManager;
    int _index;
    EventFSM<ColorPhases> _myFsm;

    [SerializeField, ColorUsage(true, true)] Color _signalColor;
    float[] _frecuenciaParpadeo = new float[7];
    void Start()
    {
        _levelTimerManager = Helpers.LevelTimerManager;
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);
        _levelTimerManager.RedButton += OnWinLevel;
        _levelTimerManager.OnLevelDefeat += OnLevelDefeat;

        _frecuenciaParpadeo = new float[7] { _levelTimerManager.LevelMaxTime * .5f, (_levelTimerManager.LevelMaxTime * .5f) +_levelTimerManager.LevelMaxTime * .5f * .5f, _levelTimerManager.LevelMaxTime - 5,
        _levelTimerManager.LevelMaxTime - 4,_levelTimerManager.LevelMaxTime - 3,_levelTimerManager.LevelMaxTime - 2,_levelTimerManager.LevelMaxTime - 1};

        var goingRed = new State<ColorPhases>("GoingRed");
        var goingWhite = new State<ColorPhases>("GoingWhite");
        var empty = new State<ColorPhases>("Empty");

        StateConfigurer.Create(empty).SetTransition(ColorPhases.GoingRed, goingRed).Done();
        StateConfigurer.Create(goingRed).SetTransition(ColorPhases.GoingWhite, goingWhite).Done();
        StateConfigurer.Create(goingWhite).SetTransition(ColorPhases.Empty, empty).Done();

        float timer = 0;

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

            if (timer / 0.5f >= 1) _myFsm.SendInput(ColorPhases.Empty);
        };
        goingWhite.OnExit += delegate { timer = 0; };

        empty.OnUpdate += delegate
        {
            if (_levelTimerManager.Timer >= _frecuenciaParpadeo[_index])
            {
                _myFsm.SendInput(ColorPhases.GoingRed);
                _index++;
            }
        };

        _myFsm = new EventFSM<ColorPhases>(empty);
    }
    private void Update()
    {
        _myFsm?.Update();
    }
    void OnWinLevel()
    {
        _myFsm = null;
        _colorAdjustments.colorFilter.value = Color.white;
    }
    void OnLevelDefeat()
    {
        _myFsm = null;
        _index = 0;
    }
    private void OnDestroy()
    {
        _levelTimerManager.RedButton -= OnWinLevel;
        _levelTimerManager.OnLevelDefeat -= OnLevelDefeat;
    }
}
