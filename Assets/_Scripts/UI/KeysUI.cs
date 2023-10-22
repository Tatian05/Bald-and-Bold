using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class KeysUI : MonoBehaviour
{
    [SerializeField] Image _keyImg;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Sprite[] _gamepadButtonsSprites;

    string[] _gamepadButtonsNames = new string[3] { "button", "Stick", "Shoulder" };
    Dictionary<string, Sprite> _gamepadButtonImage;

    private void Awake()
    {
        _gamepadButtonImage = _gamepadButtonsNames.DictioraryFromTwoLists(_gamepadButtonsSprites);
    }
    #region BUILDER
    public KeysUI SetText(string text)
    {
        _text.text = text;
        return this;
    }
    public KeysUI SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public KeysUI SetImage(string bindingPath)
    {
        if (OnControlsChange.Instance.CurrentControl != "Gamepad") return this;

        _keyImg.sprite = _gamepadButtonImage.FirstOrDefault(x => bindingPath.Contains(x.Key)).Value;
        return this;
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
