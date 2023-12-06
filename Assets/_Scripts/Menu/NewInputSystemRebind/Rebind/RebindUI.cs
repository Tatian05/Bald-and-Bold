using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class RebindUI : MonoBehaviour
{
    [SerializeField] InputActionReference _inputActionReference;
    [SerializeField] bool _excludeMouse = false;
    [SerializeField, Range(0, 10)] int _selectedBinding;
    [SerializeField] InputBinding.DisplayStringOptions _displayStringOptions;

    [Header("Binding INFO - DO NOT EDIT")]
    [SerializeField] InputBinding _inputBinding;

    int _bindingIndex;
    string _actionName;
    SetTextToBoxText _setText;

    [Header("UI Fields")]
    [SerializeField] Button _rebindButton;
    [SerializeField] TextMeshProUGUI _rebindText;
    [SerializeField] ListOfTmpSpriteAssets _listOfTmpSpriteAssets;
    [SerializeField] DeviceType _deviceType;
    [SerializeField] int _frames;
    [SerializeField, Tooltip("OPTIONAL, SI DESEA USARLO DEBE CONTENER EL NOMBRE DEL TMP_SPRITE CORRESPONDIENTE EN EL MENSAJE")] string _message;
    private void OnEnable()
    {
        _rebindButton.onClick.AddListener(() => DoRebind());

        if (_inputActionReference != null)
        {
            GetBindingInfo();
            _setText = new SetTextToBoxText(_listOfTmpSpriteAssets);
            NewInputManager.LoadUserBindings(_actionName);
            UpdateUI();
        }

        NewInputManager.RebindComplete += UpdateUI;
        NewInputManager.RebindCanceled += UpdateUI;
    }
    private void OnDisable()
    {
        NewInputManager.RebindComplete -= UpdateUI;
        NewInputManager.RebindCanceled -= UpdateUI;
    }
    private void DoRebind()
    {
        NewInputManager.StartRebind(_actionName, _bindingIndex, _rebindText, _excludeMouse);
    }

    private void OnValidate()
    {
        if (_inputActionReference == null) return;

        GetBindingInfo();
        UpdateUI();
    }

    void GetBindingInfo()
    {
        _actionName = _inputActionReference.action.name;

        if (_inputActionReference.action.bindings.Count > _selectedBinding)
        {
            _inputBinding = _inputActionReference.action.bindings[_selectedBinding];
            _bindingIndex = _selectedBinding;
        }
    }
    void UpdateUI()
    {
        if (_rebindText != null && _setText != null)
        {
            _rebindText.text = _setText.SetText(_actionName, _deviceType, _frames, startFrame: 0, actionCompositeIndex: _bindingIndex);
            //if (Application.isPlaying)
            // _rebindText.text = NewInputManager.GetBindingName(_actionName, _bindingIndex);
            //else
            //    _rebindText.text = _inputActionReference.action.GetBindingDisplayString(_bindingIndex);
        }
    }
    public void ResetBinding()
    {
        NewInputManager.ResetBinding(_actionName, _bindingIndex);
        NewInputManager.SaveUserBindings(_inputActionReference);
        UpdateUI();
    }
}
