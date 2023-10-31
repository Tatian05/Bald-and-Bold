using UnityEngine;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider _generalSlider, _musicSlider, _sfxSlider;

    const string GeneralVolume = "General Volume";
    const string MusicVolume = "Music Volume";
    const string SFXVolume = "SFX Volume";
    void OnEnable()
    {
        _generalSlider.value = AudioListener.volume;
        _musicSlider.value = Helpers.AudioManager.musicSource.volume;
        _sfxSlider.value = Helpers.AudioManager.sfxSource.volume;
    }
    public void SetGeneralVolume()
    {
        AudioListener.volume = _generalSlider.value;
    }
    public void SetMusicVolume()
    {
        Helpers.AudioManager.musicSource.volume = _musicSlider.value;
    }
    public void SetSFXVolume()
    {
        Helpers.AudioManager.sfxSource.volume = _sfxSlider.value;
        Helpers.AudioManager.setCinematicSound?.Invoke();
    }

    public void SaveVolumes()
    {
        PlayerPrefs.SetFloat(GeneralVolume, _generalSlider.value);
        PlayerPrefs.SetFloat(MusicVolume, _musicSlider.value);
        PlayerPrefs.SetFloat(SFXVolume, _sfxSlider.value);
    }
}
