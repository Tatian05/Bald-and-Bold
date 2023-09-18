using TMPro;
using UnityEngine;
public class TutorialControls : MonoBehaviour
{
    [SerializeField, Header("English"), TextArea(minLines: 1, maxLines: 4)] string _ENGText;
    [SerializeField, Header("Spanish"), TextArea(minLines: 1, maxLines: 4)] string _SPAText;
    [SerializeField] string[] _inputsNames;
    [SerializeField] TextMeshProUGUI _inputActionTxt;

    KeysUI[] _keys;
    GameObject _info;
    bool _activated;
    private void Start()
    {
        _info = GetComponentInChildren<Canvas>().gameObject;
        _keys = GetComponentsInChildren<KeysUI>();
        _inputActionTxt.text = LanguageManager.Instance.selectedLanguage == Languages.eng ? _ENGText : _SPAText;
        _info.SetActive(false);
    }
    void Wait()
    {
        _activated = true;
        _info.SetActive(true);
        for (int i = 0; i < _inputsNames.Length; i++)
        {
            _keys[i].active = true;
            _keys[i].SetDelay(i % 2 == 0).SetButtonSprite(_inputsNames[i]);
        }
    }
    private void OnTriggerEnter2D()
    {
        if (!_activated)
            Wait();
    }
}
