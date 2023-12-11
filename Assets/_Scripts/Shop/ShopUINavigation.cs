using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading.Tasks;

public class ShopUINavigation : MonoBehaviour
{
    [SerializeField] GameObject _playerShopsContainer, _presidentShopsContainer, _weaponsShopContainer, _bulletsShopContainer, _grenadesShopContainer, _consumablesShopContainer;
    [SerializeField] Button _buyButton, _closeButton;

    [Header("DeviceChange")]
    [SerializeField] string[] _buyTMP, _backTMP, _nextTMP, _beforeTMP;
    [SerializeField] TextMeshProUGUI _gamepadCloseTMP, _gamepadBuyTMP, _gamepadNextTMP, _gamepadBeforeTMP;
    [SerializeField] GameObject _gamepadContainer;
    [SerializeField] GameObject _exitButton;

    EventSystemScript _eventSystemScript;
    ShopWindow _shopWindow;
    List<ShopItem> _playerList, _presidentList, _weaponsList, _bulletsList, _grenadesList, _consumablesList;
    Button[] _windowsButtons;
    InputAction _nextAction, _beforeAction, _buyAction, _closeAction;
    private void Awake()
    {
        _shopWindow = GetComponent<ShopWindow>();
        _eventSystemScript = EventSystemScript.Instance;
        _windowsButtons = _shopWindow.windowsButtons;
        _nextAction = NewInputManager.PlayerInputs.UI.Next;
        _beforeAction = NewInputManager.PlayerInputs.UI.Before;
        _buyAction = NewInputManager.PlayerInputs.UI.SpecialAction;
        _closeAction = _eventSystemScript.UIInputs.UI.Cancel;
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += GamepadNavigation;
        if (NewInputManager.activeDevice == DeviceType.Keyboard) return;

        _shopWindow.OnBuy += Navigation;

        _nextAction.performed += GamepadNextWindow;
        _nextAction.Enable();
        _beforeAction.performed += GamepadBeforeWindow;
        _beforeAction.Enable();
        _buyAction.performed += GamepadBuy;
        _buyAction.Enable();
        _closeAction.performed += GamepadClose;
        _closeAction.Enable();
    }

    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= GamepadNavigation;
        if (NewInputManager.activeDevice == DeviceType.Keyboard) return;

        _shopWindow.OnBuy -= Navigation;

        _nextAction.performed -= GamepadNextWindow;
        _nextAction.Disable();
        _beforeAction.performed -= GamepadBeforeWindow;
        _beforeAction.Disable();
        _buyAction.performed -= GamepadBuy;
        _buyAction.Disable();
        _closeAction.performed -= GamepadClose;
        _closeAction.Disable();
    }
    void Start()
    {
        GamepadNavigation();
    }
    async void GamepadNavigation()
    {
        await Task.Yield();
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _windowsButtons[0].onClick.AddListener(PlayerWindowButton);

            _windowsButtons[1].onClick.AddListener(PresidentWindowButton);

            _windowsButtons[2].onClick.AddListener(WeaponWindowButton);

            _windowsButtons[3].onClick.AddListener(BulletWindowButton);

            _windowsButtons[4].onClick.AddListener(GrenadeWindowButton);

            _windowsButtons[5].onClick.AddListener(ConsumableWindowButton);

            Navigation();

            _shopWindow.OnBuy += Navigation;

            _nextAction.performed += GamepadNextWindow;
            _nextAction.Enable();
            _beforeAction.performed += GamepadBeforeWindow;
            _beforeAction.Enable();
            _buyAction.performed += GamepadBuy;
            _buyAction.Enable();
            _closeAction.performed += GamepadClose;
            _closeAction.Enable();

            _gamepadCloseTMP.text = _backTMP[(int)NewInputManager.activeDevice - 1];
            _gamepadBuyTMP.text = _buyTMP[(int)NewInputManager.activeDevice - 1];
            _gamepadNextTMP.text = _nextTMP[(int)NewInputManager.activeDevice - 1];
            _gamepadBeforeTMP.text = _beforeTMP[(int)NewInputManager.activeDevice - 1];
            _gamepadContainer.SetActive(true);
            _exitButton.SetActive(false);
        }
        else
        {
            _windowsButtons[0].onClick.RemoveListener(PlayerWindowButton);
            _windowsButtons[1].onClick.RemoveListener(PresidentWindowButton);
            _windowsButtons[2].onClick.RemoveListener(WeaponWindowButton);
            _windowsButtons[3].onClick.RemoveListener(BulletWindowButton);
            _windowsButtons[4].onClick.RemoveListener(GrenadeWindowButton);
            _windowsButtons[5].onClick.RemoveListener(ConsumableWindowButton);

            _shopWindow.OnBuy -= Navigation;

            _nextAction.performed -= GamepadNextWindow;
            _nextAction.Disable();
            _beforeAction.performed -= GamepadBeforeWindow;
            _beforeAction.Disable();
            _buyAction.performed -= GamepadBuy;
            _buyAction.Disable();
            _closeAction.performed -= GamepadClose;
            _closeAction.Disable();

            _gamepadContainer.SetActive(false);
            _exitButton.SetActive(true);
        }
    }

    #region WindowsButtons

    void PlayerWindowButton()
    {
        var go = _playerList.FirstOrDefault();
        if (!go) return;
        _eventSystemScript.SetCurrentGameObjectSelected(go.gameObject);
        _shopWindow.shoppableSelected = go.ShoppableSO;
        _shopWindow.itemSelected = go;
    }
    void PresidentWindowButton()
    {
        var go = _presidentList.FirstOrDefault();
        if (!go) return;
        _eventSystemScript.SetCurrentGameObjectSelected(go.gameObject);
        _shopWindow.shoppableSelected = go.ShoppableSO;
        _shopWindow.itemSelected = go;
    }

    void WeaponWindowButton()
    {
        var go = _weaponsList.FirstOrDefault();
        if (!go) return;
        _eventSystemScript.SetCurrentGameObjectSelected(go.gameObject);
        _shopWindow.shoppableSelected = go.ShoppableSO;
        _shopWindow.itemSelected = go;
    }

    void BulletWindowButton()
    {
        var go = _bulletsList.FirstOrDefault();
        if (!go) return;
        _eventSystemScript.SetCurrentGameObjectSelected(go.gameObject);
        _shopWindow.shoppableSelected = go.ShoppableSO;
        _shopWindow.itemSelected = go;
    }
    void GrenadeWindowButton()
    {
        var go = _grenadesList.FirstOrDefault();
        if (!go) return;
        _eventSystemScript.SetCurrentGameObjectSelected(go.gameObject);
        _shopWindow.shoppableSelected = go.ShoppableSO;
        _shopWindow.itemSelected = go;
    }
    void ConsumableWindowButton()
    {
        var go = _consumablesList.FirstOrDefault();
        if (!go) return;
        _eventSystemScript.SetCurrentGameObjectSelected(go.gameObject);
        _shopWindow.shoppableSelected = go.ShoppableSO;
        _shopWindow.itemSelected = go;
    }

    #endregion
    void Navigation()
    {
        UpdateLists();

        _windowsButtons[_windowIndex].onClick.Invoke();
        Button beforePlayerButton = null;
        Button beforePresiButton = null;
        Button beforeWeaponButton = null;
        Button beforeBulletButton = null;
        Button beforeGrenadeButton = null;
        Button beforeConsumableButton = null;

        foreach (var item in _playerList)
        {
            var navigation = item.Button.navigation;
            navigation.selectOnRight = _playerList.Next(item).Button;
            navigation.selectOnLeft = beforePlayerButton ? beforePlayerButton : _playerList.Last().Button;

            item.Button.navigation = navigation;
            beforePlayerButton = item.Button;
        }

        foreach (var item in _presidentList)
        {
            var navigation = item.Button.navigation;
            navigation.selectOnRight = _presidentList.Next(item).Button;
            navigation.selectOnLeft = beforePresiButton ? beforePresiButton : _presidentList.Last().Button;

            item.Button.navigation = navigation;
            beforePresiButton = item.Button;
        }

        foreach (var item in _weaponsList)
        {
            var navigation = item.Button.navigation;
            navigation.selectOnRight = _weaponsList.Next(item).Button;
            navigation.selectOnLeft = beforeWeaponButton ? beforeWeaponButton : _weaponsList.Last().Button;

            item.Button.navigation = navigation;
            beforeWeaponButton = item.Button;
        }

        foreach (var item in _bulletsList)
        {
            var navigation = item.Button.navigation;
            navigation.selectOnRight = _bulletsList.Next(item).Button;
            navigation.selectOnLeft = beforeBulletButton ? beforeBulletButton : _bulletsList.Last().Button;

            item.Button.navigation = navigation;
            beforeBulletButton = item.Button;
        }

        foreach (var item in _grenadesList)
        {
            var navigation = item.Button.navigation;
            navigation.selectOnRight = _grenadesList.Next(item).Button;
            navigation.selectOnLeft = beforeGrenadeButton ? beforeGrenadeButton : _grenadesList.Last().Button;

            item.Button.navigation = navigation;
            beforeGrenadeButton = item.Button;
        }

        foreach (var item in _consumablesList)
        {
            var navigation = item.Button.navigation;
            navigation.selectOnRight = _consumablesList.Next(item).Button;
            navigation.selectOnLeft = beforeConsumableButton ? beforeConsumableButton : _consumablesList.Last().Button;

            item.Button.navigation = navigation;
            beforeConsumableButton = item.Button;
        }
    }
    void UpdateLists()
    {
        _shopWindow = GetComponent<ShopWindow>();
        _playerList = _playerShopsContainer.GetComponentsInChildren<ShopItem>().Where(x => !x.InCollection).ToList();
        _presidentList = _presidentShopsContainer.GetComponentsInChildren<ShopItem>().Where(x => !x.InCollection).ToList();
        _weaponsList = _weaponsShopContainer.GetComponentsInChildren<ShopItem>().Where(x => !x.InCollection).ToList();
        _bulletsList = _bulletsShopContainer.GetComponentsInChildren<ShopItem>().Where(x => !x.InCollection).ToList();
        _grenadesList = _grenadesShopContainer.GetComponentsInChildren<ShopItem>().Where(x => !x.InCollection).ToList();
        _consumablesList = _consumablesShopContainer.GetComponentsInChildren<ShopItem>().ToList();
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
    void GamepadBuy(InputAction.CallbackContext obj)
    {
        if (_buyButton.interactable) _buyButton.onClick.Invoke();
    }
    void GamepadClose(InputAction.CallbackContext obj)
    {
        _closeButton.onClick.Invoke();
    }
}