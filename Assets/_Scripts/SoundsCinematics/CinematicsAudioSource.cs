using UnityEngine;
public class CinematicsAudioSource : MonoBehaviour
{
    AudioSource[] _audioSources;
    void Start()
    {
        _audioSources = GetComponentsInChildren<AudioSource>();
        Helpers.AudioManager.setCinematicSound += SetSounds;
        SetSounds();
    }
    void SetSounds()
    {
        foreach (var item in _audioSources) item.volume = Helpers.AudioManager.sfxSource.volume;
    }
}
