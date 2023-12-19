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
    }
    private async void OnEnable()
    {
        Helpers.LevelTimerManager.RedButton += StopSound;
        if (!_first) return;

        await Task.Yield();
        _ambienceAudioSource.Play();
        _ambienceAudioSource.time = _ambienceTime;
    }
    private void OnDisable()
    {
        Helpers.LevelTimerManager.RedButton -= StopSound;       
        _ambienceTime = _ambienceAudioSource.time;
        _ambienceAudioSource.Stop();
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, PlaySound);
    }
    void PlaySound(params object[] param) { _ambienceAudioSource.Play(); _first = true; }
    public void StopSound() { _ambienceAudioSource.Stop(); }
}
