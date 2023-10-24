using UnityEngine;
using TMPro;
public class KeysUI : MonoBehaviour
{
    [SerializeField] ListOfTmpSpriteAssets _listOfTmpSpriteAssets;
    [SerializeField] TextMeshProUGUI _textBox;

    SetTextToBoxText _setText;
    string _actionName;
    private void Awake()
    {
        _setText = new SetTextToBoxText(_listOfTmpSpriteAssets, _textBox);
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += TriggerSetText;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= TriggerSetText;
    }
    void TriggerSetText() { _setText.SetText(_actionName); }

    #region BUILDER
    public KeysUI SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public KeysUI SetAction(string actionName)
    {
        _actionName = actionName;
        return this;
    }
    public KeysUI SetText()
    {
        _setText.SetText(_actionName);
        return this;
    }
    #endregion
    public static void TurnOn(KeysUI k)
    {
        k.gameObject.SetActive(true);
    }
    public static void TurnOff(KeysUI k)
    {
        if (k) k.gameObject.SetActive(false);
    }

    public void ReturnObject()
    {
        FRY_KeysUI.Instance.ReturnObject(this);
    }
}
