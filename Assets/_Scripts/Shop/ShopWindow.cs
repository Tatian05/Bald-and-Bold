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
    [SerializeField] TextMeshProUGUI _consumableDescription;

    [Space(20)]
    [SerializeField] Color _selectedColor;
    [SerializeField] TextMeshProUGUI _coins;
    [SerializeField] CosmeticData[] _playerCosmetics;
    [SerializeField] CosmeticData[] _presidentCosmetics;
    [SerializeField] ConsumableData[] _consumables;
    ShoppableSO _shoppableSelected;
    ShopItem _itemSelected;

    Color _buttonsColor;
    void Awake()
    {
        _playerCosmetics = Resources.LoadAll<CosmeticData>("Shop/Cosmetics/Player").Where(x => x.shoppableData.cost > 0).OrderBy(x => x.shoppableData.shoppableQuality).ToArray();
        _presidentCosmetics = Resources.LoadAll<CosmeticData>("Shop/Cosmetics/President").Where(x => x.shoppableData.cost > 0).OrderBy(x => x.shoppableData.shoppableQuality).ToArray();
        _consumables = Resources.LoadAll<ConsumableData>("Shop/Consumables").Where(x => x.shoppableData.cost > 0).OrderBy(x => x.shoppableData.shoppableQuality).ToArray();
    }
    void Start()
    {
        _buttonsColor = _windowsButtons[0].image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        _coins.text = persistantDataSaved.presiCoins.ToString();

        List<Button> allButtons = new List<Button>();

        #region Instanceo de Cosmetics

        for (int i = 0; i < _playerCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_playerGridParent).
                               SetCosmeticData(_playerCosmetics[i]);

            var button = cosmeticItem.GetComponent<Button>();
            allButtons.Add(button);
            if (persistantDataSaved.playerCosmeticCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection();
                continue;
            }

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = cosmeticItem.ShoppableSO;
                _itemSelected = cosmeticItem;
                ShowSelectedPlayerCosmetic();
            });
        }

        for (int i = 0; i < _presidentCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab).
                               SetParent(_presidentGridParent).
                               SetCosmeticData(_presidentCosmetics[i]);

            var button = cosmeticItem.GetComponent<Button>();
            allButtons.Add(button);
            if (persistantDataSaved.presidentCosmeticCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection();
                continue;
            }

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = cosmeticItem.ShoppableSO;
                ShowSelectedPresidentCosmetic();
                _itemSelected = cosmeticItem;
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
                ShowConsumableSelected();
            });
        }

        #endregion

        foreach (var item in allButtons)
        {
            item.onClick.AddListener(() =>
            {
                allButtons.ForEach(x => x.image.color = Color.clear);
                item.image.color = _selectedColor;
                _shoppableSelectedTxt.text = _shoppableSelected.shoppableData.shoppableName;
                _coinsTextBuyButton.text = _shoppableSelected.shoppableData.cost.ToString();
                _buyButton.interactable = persistantDataSaved.presiCoins >= _shoppableSelected.shoppableData.cost ? true : false;
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
            _itemSelected.SetInCollection();
            _coins.text = persistantDataSaved.presiCoins.ToString();
            _shoppableSelected = null;
            _itemSelected = null;
            _buyButton.interactable = false;
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

        consumable.SetConsumable(_consumableDescription);
    }
}
