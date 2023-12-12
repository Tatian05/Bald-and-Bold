using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneManager : MonoBehaviour
{
    Animator _anim;
    WaitForSeconds _wait = new WaitForSeconds(1f);
    PersistantData _persistantData;
    AudioManager _audioManager;
    private void Start()
    {
        _audioManager = Helpers.AudioManager;
        _persistantData = Helpers.PersistantData;
        _anim = GetComponent<Animator>();
        if (PlayerPrefs.GetInt("FadeOut") != 0) _audioManager.FadeInOutVolume(0, _persistantData.settingsData.generalVolume);
    }
    public void ReloadLevel() => StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex, false));
    public void LoadLevelAsync(int levelIndex, bool fadeOut) => StartCoroutine(LoadAsync(levelIndex, fadeOut));
    public void LoadLevelAsync(string levelName, bool fadeOut) => StartCoroutine(LoadAsync(levelName, fadeOut));
    public void SaveCurrentLevel()
    {
        var levelName = string.Empty;

        foreach (char c in SceneManager.GetActiveScene().name)
            if (char.IsNumber(c)) levelName += c;
        var currentLevel = Convert.ToInt32(levelName);

        bool lastLevel = currentLevel >= Helpers.TotalLevels;
        _persistantData.gameData.currentLevel = lastLevel || _persistantData.gameData.currentLevel > currentLevel ? _persistantData.gameData.currentLevel : currentLevel + 1;

        string nextScene = lastLevel && _persistantData.gameData.currentDeaths > ZonesManager.Instance.zones.Last().deathsNeeded ||
            ZonesManager.Instance.lastLevelsZone.Any(x => x == SceneManager.GetActiveScene().name) ? "LevelSelect" :
            lastLevel && Helpers.PersistantData.gameData.currentDeaths <= ZonesManager.Instance.zones.Last().deathsNeeded ? "WinScreen"
            : $"Level {currentLevel + 1}";

        LoadLevelAsync(nextScene, nextScene.Equals("LevelSelect"));
    }

    IEnumerator LoadAsync(string sceneName, bool fadeOut)
    {
        if (fadeOut) _audioManager?.FadeInOutVolume(_persistantData.settingsData.generalVolume, 0);
        PlayerPrefs.SetInt("FadeOut", fadeOut ? 1 : 0);
        _anim.Play("Close");
        yield return _wait;
        SceneManager.LoadSceneAsync(sceneName);
    }
    IEnumerator LoadAsync(int sceneIndex, bool fadeOut)
    {
        if (fadeOut) _audioManager?.FadeInOutVolume(_persistantData.settingsData.generalVolume, 0);
        PlayerPrefs.SetInt("FadeOut", fadeOut ? 1 : 0);
        _anim.Play("Close");
        yield return _wait;
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    //LAS LLAMO POR EVENTO EN BOTONES
    public void LoadAsyncFadeOut(string sceneName) { LoadLevelAsync(sceneName, true); }
}
