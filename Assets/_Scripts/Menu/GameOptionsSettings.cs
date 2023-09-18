using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class GameOptionsSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _languageDD, _gameModeDD;
    [SerializeField] TextToTranslate _gameModeTextToTranslate;

    List<string> _languages = new List<string>() { "English", "Spanish" };
    List<string> _gameModeOptions = new List<string>() { "Easy", "Hard" };
    void Start()
    {
        #region GameMode

        _gameModeDD.ClearOptions();
        _gameModeDD.AddOptions(_gameModeOptions);

        _gameModeDD.value = Helpers.PersistantData.gameData.gameMode;

        #endregion

        #region Language 
        _languageDD.ClearOptions();
        _languageDD.AddOptions(_languages);

        _languageDD.value = (int)LanguageManager.Instance.selectedLanguage;

        if (_languageDD.value == 0)
        {
            _languageDD.options[0].text = "English";
            _languageDD.options[1].text = "Spanish";

            _gameModeDD.options[0].text = "Easy";
            _gameModeDD.options[1].text = "Hard";
        }
        else
        {
            _languageDD.options[0].text = "Ingles";
            _languageDD.options[1].text = "Espanol";

            _gameModeDD.options[0].text = "Facil";
            _gameModeDD.options[1].text = "Dificil";
        }

        #endregion

        _languageDD.onValueChanged.AddListener(SetLanguage);
        _gameModeDD.onValueChanged.AddListener(SetGameMode);

        _languageDD.RefreshShownValue();
        _gameModeDD.RefreshShownValue();

        _gameModeTextToTranslate.UpdateText(_gameModeDD.value == 0 ? "ID_Easy" : "ID_Hard");
    }

    public void SetLanguage(int language)
    {
        LanguageManager.Instance.selectedLanguage = (Languages)language;
        LanguageManager.Instance.OnUpdate();

        if (_languageDD.value == 0)
        {
            _languageDD.options[0].text = _languages[0] = "English";
            _languageDD.options[1].text = _languages[1] = "Spanish";

            _gameModeDD.options[0].text = "Easy";
            _gameModeDD.options[1].text = "Hard";
        }
        else
        {
            _languageDD.options[0].text = _languages[0] = "Ingles";
            _languageDD.options[1].text = _languages[1] = "Espanol";

            _gameModeDD.options[0].text = "Facil";
            _gameModeDD.options[1].text = "Dificil";
        }

        _languageDD.RefreshShownValue();
        _gameModeDD.RefreshShownValue();
        Helpers.PersistantData.persistantDataSaved.currentLanguageIndex = language;
    }

    public void SetGameMode(int gameMode)
    {
        Helpers.PersistantData.gameData.gameMode = (int)(GameMode)gameMode;
        _gameModeTextToTranslate.UpdateText(_gameModeDD.value == 0 ? "ID_Easy" : "ID_Hard");
    }
}
