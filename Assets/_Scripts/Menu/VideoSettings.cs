using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VideoSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _resolutionDropDown, _windowModeDropDown;
    [SerializeField] TextMeshProUGUI _tmpBackTxt;
    [SerializeField] string[] _gamepadTmpSprites;
    [SerializeField] GameObject _backButton, _backTMPTxt;

    Resolution[] _resolutions;
    List<string> _windowOptions = new List<string>() { "Full Screen", "Windowed" };

    private void OnEnable()
    {
        _windowModeDropDown.ClearOptions();

        _windowModeDropDown.AddOptions(_windowOptions);

        #region Language 

        if (LanguageManager.Instance.selectedLanguage == Languages.eng)
        {
            _windowModeDropDown.options[0].text = "Full Screen";
            _windowModeDropDown.options[1].text = "Windowed";
        }
        else
        {
            _windowModeDropDown.options[0].text = "Pantalla Completa";
            _windowModeDropDown.options[1].text = "Modo Ventana";
        }

        #endregion

        _windowModeDropDown.value = Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? 0 : 1;

        NewInputManager.ActiveDeviceChangeEvent += GamepadBack;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= GamepadBack;
    }
    private void Start()
    {
        #region Resolution

        _resolutions = Screen.resolutions;

        _resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }
        _resolutionDropDown.AddOptions(options);

        _resolutionDropDown.value = currentResolutionIndex;

        #endregion

        _resolutionDropDown.onValueChanged.AddListener(SetResolution);
        _windowModeDropDown.onValueChanged.AddListener(SetWindowMode);

        GamepadBack();
    }
    public void SetWindowMode(int screenMode)
    {
        FullScreenMode newMode = screenMode == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.fullScreenMode = newMode;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    void GamepadBack()
    {
        if(NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _backTMPTxt.SetActive(true);
            _tmpBackTxt.text = _gamepadTmpSprites[(int)NewInputManager.activeDevice - 1];
            _backButton.SetActive(false);
        }
        else
        {
            _backTMPTxt.SetActive(false);
            _backButton.SetActive(true);
        }
    }
}
