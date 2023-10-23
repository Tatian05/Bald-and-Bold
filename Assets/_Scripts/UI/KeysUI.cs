using UnityEngine;
using TMPro;
using System.Linq;
public class KeysUI : MonoBehaviour
{
    [Tooltip("EL TEXTO DEBE SER SIEMPRE \"{PLAYER/+?}\" LA ACCION QUE QUERRAMOS")]
    [SerializeField] string _actionAddress;
    [SerializeField] ListOfTmpSpriteAssets _listOfTmpSpriteAssets;
    [SerializeField] TextMeshProUGUI _textBox;
    string _actionName;
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += UpdateText;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= UpdateText;
    }

    #region BUILDER
    public KeysUI SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public KeysUI SetAction(string actionName)
    {
        _actionName = actionName;
        _actionAddress = "{Player/" + actionName + "}";
        return this;
    }
    public KeysUI SetText()
    {
        int currentDevice = (int)NewInputManager.activeDevice;

        if (currentDevice > _listOfTmpSpriteAssets.spriteAssets.Count - 1)
            Debug.Log($"Missing Sprite Asset for {NewInputManager.activeDevice}");

        _textBox.text = ReadAndReplaceBinding.ReplaceActiveBindings(_actionAddress, _listOfTmpSpriteAssets);
        return this;
    }
    void UpdateText()
    {
        if (string.IsNullOrEmpty(_actionName)) return;
        int currentDevice = (int)NewInputManager.activeDevice;

        if (currentDevice > _listOfTmpSpriteAssets.spriteAssets.Count - 1)
            Debug.Log($"Missing Sprite Asset for {NewInputManager.activeDevice}");

        _textBox.text = ReadAndReplaceBinding.ReplaceActiveBindings(_actionAddress, _listOfTmpSpriteAssets);
    }
    #endregion
    public static void TurnOn(KeysUI k)
    {
        k.gameObject.SetActive(true);
    }
    public static void TurnOff(KeysUI k)
    {
        if (k)
            k.gameObject.SetActive(false);
    }

    public void ReturnObject()
    {
        FRY_KeysUI.Instance.ReturnObject(this);
    }
}
