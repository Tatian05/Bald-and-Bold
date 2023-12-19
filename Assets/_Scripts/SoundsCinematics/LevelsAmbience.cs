using System.Threading.Tasks;
using UnityEngine;
public class LevelsAmbience : MonoBehaviour
{
    [SerializeField] AudioSource _ambienceAudioSource;
    bool _first;
    float _ambienceTime;
    void Start()
    {
        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, PlaySound);
        Helpers.LevelTimerManager.RedButton += StopSound;
    }
    private async void OnEnable()
    {
        if (!_first) return;

        await Task.Yield();
        _ambienceAudioSource.Play();
        _ambienceAudioSource.time = _ambienceTime;

        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, PlaySound);
        Helpers.LevelTimerManager.RedButton -= StopSound;
    }
    private void OnDisable()
    {
        _ambienceTime = _ambienceAudioSource.time;
        _ambienceAudioSource.Stop();
    }
    void PlaySound(params object[] param) { _ambienceAudioSource.Play(); _first = true; }
    public void StopSound() { _ambienceAudioSource.Stop(); }
}
