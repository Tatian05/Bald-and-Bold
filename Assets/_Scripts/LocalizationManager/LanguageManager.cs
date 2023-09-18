using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Threading;
public enum Languages
{
    eng,
    spa
}
public class LanguageManager : MonoBehaviour
{
    [HideInInspector] public Languages selectedLanguage;

    [SerializeField] string _externalURL;
    Dictionary<Languages, Dictionary<string, string>> _languageManager;

    public Action OnUpdate = delegate { };

    public static LanguageManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        selectedLanguage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? Languages.eng : Languages.spa;
    }
    private void Start()
    {
        StartCoroutine(DownloadCSV(_externalURL));
    }
    public string GetTranslate(string id)
    {
        if (_languageManager == null) return null;

        return !_languageManager[selectedLanguage].ContainsKey(id) ? "Not Found" : _languageManager[selectedLanguage][id];
    }
    IEnumerator DownloadCSV(string url)
    {
        var www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        _languageManager = LanguageU.LoadCodexFromString("www", www.downloadHandler.text);

        OnUpdate?.Invoke();
    }
}
