using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] string[] _tmpTexts;
    [SerializeField] TextMeshProUGUI _gamepadBackTxt;
    [SerializeField] Slider _generalSlider, _musicSlider, _sfxSlider;
    [SerializeField] GameObject _backButtonGO, _gamepadBackGO;

    PersistantData _persistantData;
    private void Awake()
    {
        _persistantData = Helpers.PersistantData;
        GamepadBackText();
    }
    void OnEnable()
    {
        _generalSlider.value = _persistantData.settingsData.generalVolume;
        _musicSlider.value = _persistantData.settingsData.musicVolume;
        _sfxSlider.value = _persistantData.settingsData.sfxVolume;

        NewInputManager.ActiveDeviceChangeEvent += GamepadBackText;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= GamepadBackText;
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
    void GamepadBackText()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _gamepadBackGO.SetActive(true);
            _gamepadBackTxt.text = _tmpTexts[(int)NewInputManager.activeDevice - 1];
            _backButtonGO.SetActive(false);
        }
        else
        {
            _gamepadBackGO.SetActive(false);
            _backButtonGO.SetActive(true);
        }
    }
}
