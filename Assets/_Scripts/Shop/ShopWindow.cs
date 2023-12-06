using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System;
public class ShopWindow : MonoBehaviour
{
    [Header("Inspector Variables")]
    [Tooltip("PlayerCosmetic, PresidentCosmetic, Consumables, Bullets, Grenades")] Button[] _windowsButtons;
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
    [SerializeField] TextMeshProUGUI _coins;
    [SerializeField] ShoppableSO[] _allShopables;
    [SerializeField] CosmeticData[] _playerCosmetics;
    [SerializeField] CosmeticData[] _presidentCosmetics;
    [SerializeField] ConsumableData[] _consumables;
    [SerializeField] BulletData[] _bullets;
    [SerializeField] BulletData[] _grenades;

    ShoppableSO _shoppableSelected;
    ShopItem _itemSelected;
    Color _buttonsColor;
    PersistantData _persistantData;
    PersistantDataSaved _persistantDataSaved;
    List<ShopItem> _allShopItems = new List<ShopItem>(), _inCollectionShopItems = new List<ShopItem>();
    Button _lastPlayerCosmeticSelected, _lastPresidentCosmeticSelected, _lastConsumableSelected, _lastBulletSelected, _lastGrenadeSelected;
    bool _start = true;
    EventSystemScript _eventSystemScript;

    public static event Action UpdateCollList;
    [HideInInspector] public List<Button> playerShopButtons = new List<Button>();
    [HideInInspector] public List<Button> presidentShopButtons = new List<Button>();
    [HideInInspector] public List<Button> bulletsShopButtons = new List<Button>();
    [HideInInspector] public List<Button> grenadesShopButtons = new List<Button>();
    [HideInInspector] public List<Button> consumablesShopButtons = new List<Button>();
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
        _buttonsColor = _windowsButtons[0].image.color;
        _coins.text = _persistantDataSaved.presiCoins.ToString();
        _consumableImage.enabled = false;
        _bulletPreviewImg.enabled = false;
        _grenadePreviewImg.enabled = false;

        List<Button> allButtons = new List<Button>();

        #region Instanceo de Cosmetics

        for (int i = 0; i < _playerCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_playerGridParent).
                               SetCosmeticData(_playerCosmetics[i]);
            _allShopItems.Add(cosmeticItem);

            var button = cosmeticItem.GetComponent<Button>();
            playerShopButtons.Add(button);
            allButtons.Add(button);
            if (_persistantData.shoppablesInCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection(true);
                _inCollectionShopItems.Add(cosmeticItem);
                continue;
            }

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = cosmeticItem.ShoppableSO;
                _itemSelected = cosmeticItem;
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
            presidentShopButtons.Add(button);
            allButtons.Add(button);
            if (_persistantData.shoppablesInCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection(true);
                _inCollectionShopItems.Add(cosmeticItem);
                continue;
            }

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = cosmeticItem.ShoppableSO;
                ShowSelectedPresidentCosmetic();
                _itemSelected = cosmeticItem;
                _lastPresidentCosmeticSelected = button;
            });
        }

        for (int i = 0; i < _consumables.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_consumablesGridParent).
                               SetCosmeticData(_consumables[i]);

            var button = cosmeticItem.GetComponent<Button>();
            consumablesShopButtons.Add(button);
            allButtons.Add(button);

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = cosmeticItem.ShoppableSO;
                _itemSelected = cosmeticItem;
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
            bulletsShopButtons.Add(button);
            allButtons.Add(button);
            if (_persistantData.shoppablesInCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection(true);
                _inCollectionShopItems.Add(cosmeticItem);
                continue;
            }

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = cosmeticItem.ShoppableSO;
                _itemSelected = cosmeticItem;
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
            grenadesShopButtons.Add(button);
            allButtons.Add(button);
            if (_persistantData.shoppablesInCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection(true);
                _inCollectionShopItems.Add(cosmeticItem);
                continue;
            }

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = cosmeticItem.ShoppableSO;
                _itemSelected = cosmeticItem;
                ShowGrenadeSelected();
                _lastGrenadeSelected = button;
            });
        }

        #endregion

        foreach (var item in allButtons)
        {
            item.onClick.AddListener(() =>
            {
                allButtons.ForEach(x => x.image.color = Color.clear);
                item.image.color = _selectedColor;
                _shoppableSelectedTxt.text = _shoppableSelected.shoppableName;
                _coinsTextBuyButton.text = _shoppableSelected.cost.ToString();
                _buyButton.interactable = _persistantDataSaved.presiCoins >= _shoppableSelected.cost ? true : false;
            });
        }

        #region botones

        var buttonsAndWindow = _windowsButtons.Zip(_windowContainers, (x, y) => Tuple.Create(x, y));
        foreach (var item in buttonsAndWindow)
        {
            item.Item1.onClick.AddListener(() =>
            {
                for (int a = 0; a < _windowsButtons.Length; a++)
                {
                    _windowsButtons[a].image.color = _buttonsColor;
                    _windowContainers[a].gameObject.SetActive(false);
                }
                item.Item1.image.color = Color.black * .5f;
                item.Item2.gameObject.SetActive(true);
            });
        }

        #endregion

        _buyButton.onClick.AddListener(() =>
        {
            _itemSelected.SetInCollection(false);
            if (_itemSelected.ShoppableSO is CosmeticData) _inCollectionShopItems.Add(_itemSelected);
            _coins.text = _persistantDataSaved.presiCoins.ToString();
            if (_shoppableSelected is ConsumableData) return;
            _shoppableSelected = null;
            _itemSelected = null;
            _buyButton.interactable = false;
            UpdateCollList();
        });

        _windowsButtons[0].onClick.Invoke();

        _eventSystemScript = EventSystemScript.Instance;

        _windowsButtons[0].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(playerShopButtons.First().gameObject));
        _windowsButtons[1].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(presidentShopButtons.First().gameObject));
        _windowsButtons[2].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(consumablesShopButtons.First().gameObject));
        _windowsButtons[3].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(bulletsShopButtons.First().gameObject));
        _windowsButtons[4].onClick.AddListener(() => _eventSystemScript.SetCurrentGameObjectSelected(grenadesShopButtons.First().gameObject));
    }

    void ShowSelectedPlayerCosmetic()
    {
        var cosmeticData = (CosmeticData)_shoppableSelected;
        cosmeticData.SetCosmetic(_playerHeadSprite, _playerTorsoSprite, _playerRightLegSprite,
                _playerLeftLegSprite, _playerRightHandSprite, _playerLeftHandSprite, _playerTailSprite);
    }
    void ShowSelectedPresidentCosmetic()
    {
        var cosmeticData = (CosmeticData)_shoppableSelected;
        cosmeticData.SetCosmetic(_presidentHeadSprite, _presidentTorsoSprite, _presidentRightLegSprite,
                  _presidentLeftLegSprite, _presidentRightHandSprite, _presidentLeftHandSprite, _presidentTailSprite);
    }

    void ShowConsumableSelected()
    {
        var consumable = (ConsumableData)_shoppableSelected;
        _consumableImage.enabled = true;
        consumable.SetShopConsumable(_consumableDescription, _consumableAmountDurationTxt, _consumableImage);
    }

    void ShowBulletSelected()
    {
        _bulletPreviewImg.enabled = true;
        (_shoppableSelected as BulletData).SetShopBullet(_bulletPreviewImg);
    }
    void ShowGrenadeSelected()
    {
        _grenadePreviewImg.enabled = true;
        (_shoppableSelected as BulletData).SetShopBullet(_grenadePreviewImg);
    }
    void OnWindowChange()
    {
        _consumableDuration.gameObject.SetActive(false);
        _shoppableSelectedTxt.text = string.Empty;
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