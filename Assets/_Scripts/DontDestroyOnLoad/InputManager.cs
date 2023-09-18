using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public KeyData[] _keysData;
    Dictionary<string, KeyCode> _buttonKeys;
    Dictionary<string, Tuple<Sprite, Sprite>> _buttonKeysData;
    List<KeyCode> _keysAllowed;
    public List<KeyCode> KeysAllowed { get { return _keysAllowed; } }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void OnEnable()
    {
        _buttonKeys = new Dictionary<string, KeyCode>();
        _buttonKeysData = new Dictionary<string, Tuple<Sprite, Sprite>>();

        _keysData = Resources.LoadAll<KeyData>("KeysSO");

        _buttonKeys["Move Right"] = KeyCode.D;
        _buttonKeys["Move Left"] = KeyCode.A;
        _buttonKeys["Move Up"] = KeyCode.W;
        _buttonKeys["Move Down"] = KeyCode.S;
        _buttonKeys["Jump"] = KeyCode.Space;
        _buttonKeys["Dash"] = KeyCode.LeftShift;
        _buttonKeys["Interact"] = KeyCode.E;
        //_buttonKeys["Throw Weapon"] = KeyCode.G;
        _buttonKeys["Shoot"] = KeyCode.Mouse0;
        _buttonKeys["Knife"] = KeyCode.Mouse1;

        foreach (var item in _buttonKeys)
        {
            foreach (var key in _keysData)
            {
                if (key.input == item.Value)
                {
                    _buttonKeysData.Add(item.Key, Tuple.Create(key.keySprite, key.pressedKey));
                    continue;
                }
            }
        }
    }
    private void Start()
    {
        _keysAllowed = _keysData.Select(x => x.input).ToList();
    }
    public bool GetButtonDown(string buttonName)
    {
        if (!_buttonKeys.ContainsKey(buttonName)) return false;

        return Input.GetKeyDown(_buttonKeys[buttonName]);
    }
    public bool GetButtonUp(string buttonName)
    {
        if (!_buttonKeys.ContainsKey(buttonName)) return false;

        return Input.GetKeyUp(_buttonKeys[buttonName]);
    }
    public bool GetButton(string buttonName)
    {
        if (!_buttonKeys.ContainsKey(buttonName)) return false;

        return Input.GetKey(_buttonKeys[buttonName]);
    }
    public string[] GetButtonNames()
    {
        return _buttonKeys.Keys.ToArray();
    }
    public string KeyNameForButton(string buttonName)
    {
        if (!_buttonKeys.ContainsKey(buttonName)) return "N/A";

        return _buttonKeys[buttonName].ToString();
    }
    public void SetButtonForKey(string buttonName, KeyCode keyCode, Action<Sprite> SetNewButton, Action<string, Sprite> SetButtonAlreadyExist)
    {
        if (_buttonKeys.ContainsValue(keyCode))
        {
            var keyToReplace = GetKeyNameByValue(keyCode);
            var emptyKey = _keysData.FirstOrDefault(x => x.name == "NONE");
            _buttonKeys[keyToReplace] = emptyKey.input;
            _buttonKeysData[keyToReplace] = Tuple.Create(emptyKey.keySprite, emptyKey.pressedKey);

            SetButtonAlreadyExist(keyToReplace, emptyKey.keySprite);
        }


        Sprite keySprite = _keysData.FirstOrDefault(x => x.input == keyCode).keySprite;
        Sprite pressedKey = _keysData.FirstOrDefault(x => x.input == keyCode).pressedKey;
        _buttonKeys[buttonName] = keyCode;
        _buttonKeysData[buttonName] = Tuple.Create(keySprite, pressedKey);

        SetNewButton(keySprite);
    }
    public float GetAxisRaw(string axis)
    {
        if (axis == "Horizontal")
            return GetButton("Move Right") ? 1f : GetButton("Move Left") ? -1f : 0;
        else if (axis == "Vertical")
            return GetButton("Move Up") ? 1f : GetButton("Move Down") ? -1f : 0;
        else return default;
    }

    #region UTILS

    string GetKeyNameByValue(KeyCode keyCode) => _buttonKeys.FirstOrDefault(x => x.Value == keyCode).Key;
    public Sprite GetKeySpriteByName(string keyName) => _buttonKeysData[keyName].Item1;
    public Sprite GetPressedKeySpriteByName(string keyName) => _buttonKeysData[keyName].Item2;

    #endregion
}