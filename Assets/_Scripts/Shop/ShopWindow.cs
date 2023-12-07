using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System;
public class ShopWindow : MonoBehaviour
{
    [Header("Inspector Variables")]
    [Tooltip("PlayerCosmetic, PresidentCosmetic, Consumables, Bullets, Grenades")] public Button[] windowsButtons;
    [SerializeField] GameObject[] _windowContainers;
    [SerializeField] Button _buyButton;
    [SerializeField] TextMeshProUGUI _coinsTextBuyButton, _shoppableSelectedTxt;
    [SerializeField] ShopItem _shopItemPrefab;

    [Header("Player Settings")]
    [SerializeField] Transform _playerGridParent;
    [SerializeField] Image _playerHeadSprite, _playerTorsoSprite, _playerRightLegSprite, _playerLeftLegSprite, _playerRightHandSprite, _playerLeftHandSprite, _playerTailSprite;

    [Header("President Settings")]
    [SerializeField] Transform _presidentGridParent;
    [SerializeField] Image _presidentHeadSprite, _presidentTorsoSprite, _presidentRightLegSprite, _presidentLeftLegSprite, _presidentRightHandSprite, _presidentLeftHandSprite, _presidentTailSprite;

    [Header("Consumables Settings")]
    [SerializeField] Transform _consumablesGridParent;
    [SerializeField] TextMeshProUGUI _consumableDescription, _consumableDuration, _consumableAmountDurationTxt;
    [SerializeField] Image _consumableImage;

    [Header("Bullets Settings")]
    [SerializeField] Transform _bulletsGridParent;
    [SerializeField] Transform _grenadesGridParent;
    [SerializeField] Image _bulletPreviewImg, _grenadePreviewImg;

    [Space(20)]
    [SerializeField] Color _selectedColor;
    [SerializeField] Color _canBuyColor, _cantBuyColor;
    [SerializeField] TextMeshProUGUI _coins;
    [SerializeField] ShoppableSO[] _allShopables;
    [SerializeField] CosmeticData[] _playerCosmetics;
    [SerializeField] CosmeticData[] _presidentCosmetics;
    [SerializeField] ConsumableData[] _consumables;
    [SerializeField] BulletData[] _bullets;
    [SerializeField] BulletData[] _grenades;

    public ShoppableSO shoppableSelected;
    public ShopItem itemSelected;
    Color _buttonsColor;
    PersistantData _persistantData;
    PersistantDataSaved _persistantDataSaved;
    List<ShopItem> _allShopItems = new List<ShopItem>(), _inCollectionShopItems = new List<ShopItem>();
    bool _start = true;
    EventSystemScript _eventSystemScript;
    Button _lastPlayerCosmeticSelected, _lastPresidentCosmeticSelected, _lastConsumableSelected, _lastBulletSelected, _lastGrenadeSelected;

    public event Action OnBuy;
    public static event Action UpdateCollList;
    void Awake()
    {
        _persistantData = Helpers.PersistantData;
        _persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        _playerCosmetics = _allShopables.OfType<CosmeticData>().Where(x => x.cosmeticType == CosmeticType.Player).OrderBy(x => x.shoppableQuality).ToArray();
        _presidentCosmetics = _allShopables.OfType<CosmeticData>().Where(x => x.cosmeticType == CosmeticType.President).OrderBy(x => x.shoppableQuality).ToArray();
        _consumables = _allShopables.OfType<ConsumableData>().OrderBy(x => x.shoppableQuality).ToArray();
        _bullets = _allShopables.OfType<BulletData>().Where(x => x.bulletType == BulletType.Bullet).OrderBy(x => x.shoppableQuality).ToArray();
        _grenades = _allShopables.OfType<BulletData>().Where(x => x.bulletType == BulletType.Grenade).OrderBy(x => x.shoppableQuality).ToArray();
    }
    private void OnEnable()
    {
        if (_start) return;

        foreach (var item in _allShopItems.Except(_inCollectionShopItems))
            if (_persistantData.shoppablesInCollection.Contains(item.ShoppableSO)) item.SetInCollection(true);
    }
    void Start()
    {
        _start = false;
        _buttonsColor = windowsButtons[0].image.color;
        _coins.text = _persistantDataSaved.presiCoins.ToString();
        _consumableImage.enabled = false;
        _bulletPreviewImg.enabled = false;
        _grenadePreviewImg.enabled = false;
        Image buyButtonImg = _buyButton.GetComponent<Image>();

        List<Button> allButtons = new List<Button>();

        #region Instanceo de Cosmetics

        for (int i = 0; i < _playerCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_playerGridParent).
                               SetCosmeticData(_playerCosmetics[i]);

            var button = cosmeticItem.GetComponent<Button>();

            _allShopItems.Add(cosmeticItem);

            allButtons.Add(button);
            if (_persistantData.shoppablesInCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection(true);
                _inCollectionShopItems.Add(cosmeticItem);
                continue;
            }

            button.onClick.AddListener(() =>
            {
                shoppableSelected = cosmeticItem.ShoppableSO;
                itemSelected = cosmeticItem;
                ShowSelectedPlayerCosmetic();
                _lastPlayerCosmeticSelected = button;
            });
        }

        for (int i = 0; i < _presidentCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_presidentGridParent).
                               SetCosmeticData(_presidentCosmetics[i]);
            _allShopItems.Add(cosmeticItem);

            var button = cosmeticItem.GetComponent<Button>();

            allButtons.Add(button);
            if (_persistantData.shoppablesInCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection(true);
                _inCollectionShopItems.Add(cosmeticItem);
                continue;
            }

            button.onClick.AddListener(() =>
            {
                shoppableSelected = cosmeticItem.ShoppableSO;
                ShowSelectedPresidentCosmetic();
                itemSelected = cosmeticItem;
                _lastPresidentCosmeticSelected = button;
            });
        }

        for (int i = 0; i < _consumables.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_consumablesGridParent).
                               SetCosmeticData(_consumables[i]);

            var button = cosmeticItem.GetComponent<Button>();

            allButtons.Add(button);

            button.onClick.AddListener(() =>
            {
                shoppableSelected = cosmeticItem.ShoppableSO;
                itemSelected = cosmeticItem;
                _consumableDuration.gameObject.SetActive(true);
                ShowConsumableSelected();
                _lastConsumableSelected = button;
            });
        }

        for (int i = 0; i < _bullets.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_bulletsGridParent).
                               SetCosmeticData(_bullets[i]);
            _allShopItems.Add(cosmeticItem);

            var button = cosmeticItem.GetComponent<Button>();

            allButtons.Add(button);
            if (_persistantData.shoppablesInCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection(true);
                _inCollectionShopItems.Add(cosmeticItem);
                continue;
            }

            button.onClick.AddListener(() =>
            {
                shoppableSelected = cosmeticItem.ShoppableSO;
                itemSelected = cosmeticItem;
                ShowBulletSelected();
                _lastBulletSelected = button;
            });
        }

        for (int i = 0; i < _grenades.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_grenadesGridParent).
                               SetCosmeticData(_grenades[i]);
            _allShopItems.Add(cosmeticItem);

            var button = cosmeticItem.GetComponent<Button>();

            allButtons.Add(button);
            if (_persistantData.shoppablesInCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection(true);
                _inCollectionShopItems.Add(cosmeticItem);
                continue;
            }

            button.onClick.AddListener(() =>
            {
                shoppableSelected = cosmeticItem.ShoppableSO;
                itemSelected = cosmeticItem;
                ShowGrenadeSelected();
                _lastGrenadeSelected = button;
            });
        }

        #endregion

        Action updateBuyButtonState = delegate
        {
            _buyButton.interactable = _persistantDataSaved.presiCoins >= shoppableSelected.cost ? true : false;
            buyButtonImg.color = _buyButton.interactable ? _canBuyColor : _cantBuyColor;
        };

        foreach (var item in allButtons)
        {
            item.onClick.AddListener(() =>
            {
                allButtons.ForEach(x => x.image.color = Color.clear);
                item.image.color = _selectedColor;
                _shoppableSelectedTxt.text = shoppableSelected.shoppableName;
                _coinsTextBuyButton.text = shoppableSelected.cost.ToString();
                updateBuyButtonState();
            });
        }

        #region botones

        var buttonsAndWindow = windowsButtons.Zip(_windowContainers, (x, y) => Tuple.Create(x, y));
        foreach (var item in buttonsAndWindow)
        {
            item.Item1.onClick.AddListener(() =>
            {
                for (int a = 0; a < windowsButtons.Length; a++)
                {
                    windowsButtons[a].image.color = _buttonsColor;
                    _windowContainers[a].gameObject.SetActive(false);
                }
                item.Item1.image.color = Color.black * .5f;
                item.Item2.gameObject.SetActive(true);
            });
        }

        #endregion

        _buyButton.onClick.AddListener(() =>
        {
            itemSelected.SetInCollection(false);
            if (itemSelected.ShoppableSO is CosmeticData) _inCollectionShopItems.Add(itemSelected);
            _coins.text = _persistantDataSaved.presiCoins.ToString();
            if (shoppableSelected is ConsumableData)
            {
                updateBuyButtonState();
                return;
            };
            shoppableSelected = null;
            itemSelected = null;
            _buyButton.interactable = false;
            _coinsTextBuyButton.text = string.Empty;
            UpdateCollList();
            OnBuy?.Invoke();
        });

        _eventSystemScript = EventSystemScript.Instance;

        windowsButtons[0].onClick.Invoke();
    }
    void ShowSelectedPlayerCosmetic()
    {
        var cosmeticData = (CosmeticData)shoppableSelected;
        cosmeticData.SetCosmetic(_playerHeadSprite, _playerTorsoSprite, _playerRightLegSprite,
                _playerLeftLegSprite, _playerRightHandSprite, _playerLeftHandSprite, _playerTailSprite);
    }
    void ShowSelectedPresidentCosmetic()
    {
        var cosmeticData = (CosmeticData)shoppableSelected;
        cosmeticData.SetCosmetic(_presidentHeadSprite, _presidentTorsoSprite, _presidentRightLegSprite,
                  _presidentLeftLegSprite, _presidentRightHandSprite, _presidentLeftHandSprite, _presidentTailSprite);
    }

    void ShowConsumableSelected()
    {
        var consumable = (ConsumableData)shoppableSelected;
        _consumableImage.enabled = true;
        consumable.SetShopConsumable(_consumableDescription, _consumableAmountDurationTxt, _consumableImage);
    }

    void ShowBulletSelected()
    {
        _bulletPreviewImg.enabled = true;
        (shoppableSelected as BulletData).SetShopBullet(_bulletPreviewImg);
    }
    void ShowGrenadeSelected()
    {
        _grenadePreviewImg.enabled = true;
        (shoppableSelected as BulletData).SetShopBullet(_grenadePreviewImg);
    }
    void OnWindowChange()
    {
        _consumableDuration.gameObject.SetActive(false);
        _shoppableSelectedTxt.text = string.Empty;
    }
    public void GamepadOnOpenWindow(bool gamepad)
    {
        if (gamepad)
        {
            windowsButtons[0].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastPlayerCosmeticSelected.gameObject));
            windowsButtons[1].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastPresidentCosmeticSelected.gameObject));
            windowsButtons[2].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastBulletSelected.gameObject));
            windowsButtons[3].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastGrenadeSelected.gameObject));
            windowsButtons[4].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastConsumableSelected.gameObject));
        }
        else
        {
            windowsButtons[0].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastPlayerCosmeticSelected.gameObject));
            windowsButtons[1].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastPresidentCosmeticSelected.gameObject));
            windowsButtons[2].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastBulletSelected.gameObject));
            windowsButtons[3].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastGrenadeSelected.gameObject));
            windowsButtons[4].onClick.RemoveListener(() => _eventSystemScript.SetCurrentGameObjectSelected(_lastConsumableSelected.gameObject));
        }
    }

    //LO LLAMO POR EVENTO EN INSPECTOR
    public void OnPlayerWindow()
    {
        OnWindowChange();
        _lastPlayerCosmeticSelected?.onClick.Invoke();
    }
    public void OnPresidentWindow()
    {
        OnWindowChange();
        _lastPresidentCosmeticSelected?.onClick.Invoke();
    }
    public void OnConsumablesWindow()
    {
        OnWindowChange();
        _lastConsumableSelected?.onClick.Invoke();
    }
    public void OnBulletsWindow()
    {
        OnWindowChange();
        _lastBulletSelected?.onClick.Invoke();
    }
    public void OnGrenadesWindow()
    {
        OnWindowChange();
        _lastGrenadeSelected?.onClick.Invoke();
    }
}