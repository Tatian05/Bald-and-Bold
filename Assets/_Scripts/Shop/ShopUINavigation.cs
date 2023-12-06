using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ShopUINavigation : MonoBehaviour
{
    ShopWindow _shopWindow;
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += Navigation;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= Navigation;       
    }
    void Start()
    {
        _shopWindow = GetComponent<ShopWindow>();

        Navigation();
    }
    void Navigation()
    {
        if (NewInputManager.activeDevice == DeviceType.Keyboard) return;

        Button beforeButton = null;

        foreach (var item in _shopWindow.playerShopButtons)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _shopWindow.playerShopButtons.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _shopWindow.playerShopButtons.Last();

            item.navigation = navigation;
            beforeButton = item;
        }

        foreach (var item in _shopWindow.presidentShopButtons)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _shopWindow.playerShopButtons.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _shopWindow.playerShopButtons.Last();

            item.navigation = navigation;
            beforeButton = item;
        }
        foreach (var item in _shopWindow.bulletsShopButtons)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _shopWindow.playerShopButtons.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _shopWindow.playerShopButtons.Last();

            item.navigation = navigation;
            beforeButton = item;
        }
        foreach (var item in _shopWindow.grenadesShopButtons)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _shopWindow.playerShopButtons.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _shopWindow.playerShopButtons.Last();

            item.navigation = navigation;
            beforeButton = item;
        }
        foreach (var item in _shopWindow.consumablesShopButtons)
        {
            var navigation = item.navigation;
            navigation.selectOnRight = _shopWindow.playerShopButtons.Next(item);
            navigation.selectOnLeft = beforeButton ? beforeButton : _shopWindow.playerShopButtons.Last();

            item.navigation = navigation;
            beforeButton = item;
        }
    }
}