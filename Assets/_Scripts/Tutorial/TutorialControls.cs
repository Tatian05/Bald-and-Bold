using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
public class TutorialControls : MonoBehaviour
{
    [SerializeField] TutorialControl[] _tutorialControls;
    [SerializeField] TextMeshProUGUI _inputActionTxt;
    [SerializeField] ListOfTmpSpriteAssets _listOfTmpSpriteAssets;
    [SerializeField] int _spritesSize;
    TutorialControl _current;
    SetTextToBoxText _setText;
    private void Awake()
    {
        _setText = new SetTextToBoxText(_listOfTmpSpriteAssets);     
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += OnControlChange;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= OnControlChange;      
    }
    void OnControlChange()
    {
        _current = _tutorialControls.FirstOrDefault(x => x.deviceType == NewInputManager.activeDevice);
        _current.SetTexts(_setText, _spritesSize);
        SetMessage();
    }
    public void SetMessage()
    {
        _inputActionTxt.text = LanguageManager.Instance.selectedLanguage == Languages.eng ? _current.tutorialTextENG : _current.tutorialTextESP;
    }
    private void OnValidate()
    {
        for (int i = 0; i < _tutorialControls.Length; i++)
            _tutorialControls[i].OnValidate();
    }
}
[System.Serializable]
public class TutorialControl
{
    public DeviceType deviceType;
    public TutorialInputs[] tutorialInputs;
    public string tutorialTextESP, tutorialTextENG;
    public void OnValidate()
    {
        for (int i = 0; i < tutorialInputs.Length; i++)
            tutorialInputs[i].OnValidate();
    }
    public void SetTexts(SetTextToBoxText setText, int spritesSize)
    {
        for (int i = 0; i < tutorialInputs.Length; i++)
            tutorialInputs[i].spriteText = $"<size={spritesSize}em>{setText.SetText(tutorialInputs[i].inputBinding.action, deviceType, tutorialInputs[i].extraFrames, tutorialInputs[i].selectedBind)}</size>";

        if (tutorialTextENG.Contains("-"))
        {
            tutorialTextENG = tutorialTextENG.Replace("-", tutorialInputs[0].spriteText);
            tutorialTextESP = tutorialTextESP.Replace("-", tutorialInputs[0].spriteText);
        }
        if (tutorialTextENG.Contains("*"))
        {
            tutorialTextENG = tutorialTextENG.Replace("*", tutorialInputs[1].spriteText);
            tutorialTextESP = tutorialTextESP.Replace("*", tutorialInputs[1].spriteText);
        }
    }
}
[System.Serializable]
public struct TutorialInputs
{
    public InputActionReference action;
    public int selectedBind, extraFrames;
    public InputBinding inputBinding;
    [TextArea(1, 3)]
    public string spriteText;

    public void OnValidate()
    {
        if (action.action.bindings.Count > selectedBind)
            inputBinding = action.action.bindings[selectedBind];
    }
}
