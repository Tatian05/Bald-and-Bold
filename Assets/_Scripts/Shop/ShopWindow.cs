using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class ShopWindow : MonoBehaviour
{
    [Header("Inspector Variables")]
    [SerializeField, Tooltip("PlayerCosmetic, PresidentCosmetic, Consumables")] Button[] _windowsButtons;
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

    [Space(20)]
    [SerializeField] Color _selectedColor;
    [SerializeField] TextMeshProUGUI _coins;
    [SerializeField] ShoppableSO[] _allShopables;
    [SerializeField] CosmeticData[] _playerCosmetics;
    [SerializeField] CosmeticData[] _presidentCosmetics;
    [SerializeField] ConsumableData[] _consumables;

    ShoppableSO _shoppableSelected;
    ShopItem _itemSelected;
    Color _buttonsColor;
    PersistantDataSaved _persistantDataSaved;
    List<ShopItem> _allShopItems = new List<ShopItem>(), _inCollectionShopItems = new List<ShopItem>();
    Button _lastPlayerCosmeticSelected, _lastPresidentCosmeticSelected, _lastConsumableSelected;
    bool _start = true;

    public static event System.Action UpdateCollList;
    void Awake()
    {
        _persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        _playerCosmetics = _allShopables.OfType<CosmeticData>().Where(x => x.cosmeticType == CosmeticType.Player).OrderBy(x => x.shoppableQuality).ToArray();
        _presidentCosmetics = _allShopables.OfType<CosmeticData>().Where(x => x.cosmeticType == CosmeticType.President).OrderBy(x => x.shoppableQuality).ToArray();
        _consumables = _allShopables.OfType<ConsumableData>().OrderBy(x => x.shoppableQuality).ToArray();
    }
    private void OnEnable()
    {
        if (_start) return;

        foreach (var item in _allShopItems.Except(_inCollectionShopItems))
            if (_persistantDataSaved.cosmeticsInCollection.Contains(item.ShoppableSO)) item.SetInCollection(true);
    }
    void Start()
    {
        _start = false;
        _buttonsColor = _windowsButtons[0].image.color;
        _coins.text = _persistantDataSaved.presiCoins.ToString();
        _consumableImage.enabled = false;

        List<Button> allButtons = new List<Button>();

        #region Instanceo de Cosmetics

        for (int i = 0; i < _playerCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_playerGridParent).
                               SetCosmeticData(_playerCosmetics[i]);
            _allShopItems.Add(cosmeticItem);

            var button = cosmeticItem.GetComponent<Button>();
            allButtons.Add(button);
            if (_persistantDataSaved.playerCosmeticCollection.Contains(cosmeticItem.ShoppableSO))
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
            allButtons.Add(button);
            if (_persistantDataSaved.presidentCosmeticCollection.Contains(cosmeticItem.ShoppableSO))
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
            var consumable = Instantiate(_shopItemPrefab).
                               SetParent(_consumablesGridParent).
                               SetCosmeticData(_consumables[i]);

            var button = consumable.GetComponent<Button>();
            allButtons.Add(button);

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = consumable.ShoppableSO;
                _itemSelected = consumable;
                _consumableDuration.gameObject.SetActive(true);
                ShowConsumableSelected();
                _lastConsumableSelected = button;
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

        var buttons = _windowsButtons.ToList();
        foreach (var item in buttons)
        {
            item.onClick.AddListener(() =>
            {
                buttons.ForEach(x => x.image.color = _buttonsColor);
                item.image.color = Color.black * .5f;
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
}
