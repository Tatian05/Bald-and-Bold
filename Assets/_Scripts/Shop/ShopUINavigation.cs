using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ShopUINavigation : MonoBehaviour
{
    [SerializeField] GameObject _playerShopsContainer, _presidentShopsContainer, _bulletsShopContainer, _grenadesShopContainer, _consumablesShopContainer;

    EventSystemScript _eventSystemScript;
    ShopWindow _shopWindow;
    List<Button> _playerList, _presidentList, _bulletsList, _grenadesList, _consumablesList;
    Button[] _windowsButtons;
    InputAction _nextAction, _beforeAction;
    private void Awake()
    {
        _shopWindow = GetComponent<ShopWindow>();
        _eventSystemScript = EventSystemScript.Instance;
        _windowsButtons = _shopWindow.windowsButtons;
        _nextAction = NewInputManager.PlayerInputs.UI.Next;
        _beforeAction = NewInputManager.PlayerInputs.UI.Before;
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += GamepadNavigation;
        _shopWindow.OnBuy += Navigation;
    }

    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= GamepadNavigation;
        _shopWindow.OnBuy -= Navigation;
    }
    void Start()
    {
        GamepadNavigation();
    }
    void GamepadNavigation()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _windowsButtons[0].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastPlayerCosmeticSelected?.gameObject));
            _windowsButtons[1].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastPresidentCosmeticSelected?.gameObject));
            _windowsButtons[2].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastBulletSelected?.gameObject));
            _windowsButtons[3].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastGrenadeSelected?.gameObject));
            _windowsButtons[4].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastConsumableSelected?.gameObject));

            _nextAction.performed += GamepadNextWindow;
            _nextAction.Enable();
            _beforeAction.performed += GamepadBeforeWindow;
            _beforeAction.Enable();

            Navigation();
        }
        else
        {
            _windowsButtons[0].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastPlayerCosmeticSelected.gameObject));
            _windowsButtons[1].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastPresidentCosmeticSelected.gameObject));
            _windowsButtons[2].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastBulletSelected.gameObject));
            _windowsButtons[3].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastGrenadeSelected.gameObject));
            _windowsButtons[4].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_shopWindow.lastConsumableSelected.gameObject));

            _nextAction.performed -= GamepadNextWindow;
            _nextAction.Disable();
            _beforeAction.performed -= GamepadBeforeWindow;
            _beforeAction.Disable();
        }
    }
    void Navigation()
    {
        UpdateLists();

        Button beforeButton = null;

        foreach (var item in _playerList)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _playerList.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _playerList.Last();

            item.navigation = navigation;
            beforeButton = item;
        }

        foreach (var item in _presidentList)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _presidentList.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _presidentList.Last();

            item.navigation = navigation;
            beforeButton = item;
        }

        foreach (var item in _bulletsList)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _bulletsList.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _bulletsList.Last();

            item.navigation = navigation;
            beforeButton = item;
        }

        foreach (var item in _grenadesList)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _grenadesList.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _grenadesList.Last();

            item.navigation = navigation;
            beforeButton = item;
        }

        foreach (var item in _consumablesList)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _consumablesList.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _consumablesList.Last();

            item.navigation = navigation;
            beforeButton = item;
        }
    }
    void UpdateLists()
    {
        _shopWindow = GetComponent<ShopWindow>();
        _playerList = _playerShopsContainer.GetComponentsInChildren<Button>().Where(x => x.interactable).ToList();
        _presidentList = _presidentShopsContainer.GetComponentsInChildren<Button>().Where(x => x.interactable).ToList();
        _bulletsList = _bulletsShopContainer.GetComponentsInChildren<Button>().Where(x => x.interactable).ToList();
        _grenadesList = _grenadesShopContainer.GetComponentsInChildren<Button>().Where(x => x.interactable).ToList();
        _consumablesList = _consumablesShopContainer.GetComponentsInChildren<Button>().ToList();
    }

    int _windowIndex = 0;
    void GamepadNextWindow(InputAction.CallbackContext obj)
    {
        _windowIndex = (_windowIndex + 1) % _windowsButtons.Length;
        _windowsButtons[_windowIndex].onClick.Invoke();
    }
    void GamepadBeforeWindow(InputAction.CallbackContext obj)
    {
        _windowIndex--;
        if (_windowIndex < 0) _windowIndex += _windowsButtons.Length;
        _windowsButtons[_windowIndex].onClick.Invoke();
    }
}