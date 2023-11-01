using UnityEngine;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider _generalSlider, _musicSlider, _sfxSlider;
    PersistantDataSaved _persistantData;
    private void Awake()
    {
        _persistantData = Helpers.PersistantData.persistantDataSaved;
    }
    void OnEnable()
    {
        _generalSlider.value = _persistantData.generalVolume;
        _musicSlider.value = _persistantData.musicVolume;
        _sfxSlider.value = _persistantData.sfxVolume;
    }
    public void SetGeneralVolume()
    {
        AudioListener.volume = _generalSlider.value;
        _persistantData.generalVolume = _generalSlider.value;
    }
    public void SetMusicVolume()
    {
        Helpers.AudioManager.musicSource.volume = _musicSlider.value;
        _persistantData.musicVolume = _musicSlider.value;
    }
    public void SetSFXVolume()
    {
        Helpers.AudioManager.sfxSource.volume = _sfxSlider.value;
        Helpers.AudioManager.setCinematicSound?.Invoke();
        _persistantData.sfxVolume = _sfxSlider.value;
    }
}
