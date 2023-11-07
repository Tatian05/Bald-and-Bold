using UnityEngine;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider _generalSlider, _musicSlider, _sfxSlider;
    PersistantData _persistantData;
    private void Awake()
    {
        _persistantData = Helpers.PersistantData;
    }
    void OnEnable()
    {
        _generalSlider.value = _persistantData.settingsData.generalVolume;
        _musicSlider.value = _persistantData.settingsData.musicVolume;
        _sfxSlider.value = _persistantData.settingsData.sfxVolume;
    }
    public void SetGeneralVolume()
    {
        AudioListener.volume = _generalSlider.value;
        _persistantData.settingsData.SetGeneralVolume(_generalSlider.value);
    }
    public void SetMusicVolume()
    {
        Helpers.AudioManager.musicSource.volume = _musicSlider.value;
        _persistantData.settingsData.SetMusicVolume(_musicSlider.value);
    }
    public void SetSFXVolume()
    {
        Helpers.AudioManager.sfxSource.volume = _sfxSlider.value;
        Helpers.AudioManager.setCinematicSound?.Invoke();
        _persistantData.settingsData.SetSFXVolume(_sfxSlider.value);
    }
}
