using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class ShopWindow : MonoBehaviour
{
    [Header("Inspector Variables")]
    [SerializeField] Button _playerButton, _presidentButton;
    [SerializeField] Button _buyButton;
    [SerializeField] TextMeshProUGUI _coinsTextBuyButton;
    [SerializeField] ShopItem _shopItemPrefab;

    [Header("Player Settings")]
    [SerializeField] Transform _playerGridParent;
    [SerializeField] Image _playerHeadSprite, _playerTorsoSprite, _playerRightLegSprite, _playerLeftLegSprite, _playerRightHandSprite, _playerLeftHandSprite, _playerTailSprite;

    [Header("President Settings")]
    [SerializeField] Transform _presidentGridParent;
    [SerializeField] Image _presidentHeadSprite, _presidentTorsoSprite, _presidentRightLegSprite, _presidentLeftLegSprite, _presidentRightHandSprite, _presidentLeftHandSprite, _presidentTailSprite;

    [Space(20)]
    [SerializeField] Color _selectedColor;
    [SerializeField] TextMeshProUGUI _coins;
    [SerializeField] CosmeticData[] _playerCosmetics;
    [SerializeField] CosmeticData[] _presidentCosmetics;
    ShoppableSO _shoppableSelected;
    ShopItem _itemSelected;

    Color _buttonsColor;
    void Awake()
    {
        _playerCosmetics = Resources.LoadAll<CosmeticData>("Shop/Cosmetics/Player").Where(x => x.shoppableData.cost > 0).OrderBy(x => x.shoppableData.shoppableQuality).ToArray();
        _presidentCosmetics = Resources.LoadAll<CosmeticData>("Shop/Cosmetics/President").Where(x => x.shoppableData.cost > 0).OrderBy(x => x.shoppableData.shoppableQuality).ToArray();
    }
    void Start()
    {
        _buttonsColor = _playerButton.image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        _coins.text = persistantDataSaved.presiCoins.ToString();

        List<Button> playerCosmeticsButton = new List<Button>();
        List<Button> presidentCosmeticsButton = new List<Button>();

        #region Instanceo de Cosmetics

        for (int i = 0; i < _playerCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab);
            cosmeticItem.transform.SetParent(_playerGridParent, true);
            cosmeticItem.SetCosmeticData(_playerCosmetics[i]);
            var button = cosmeticItem.GetComponent<Button>();
            playerCosmeticsButton.Add(button);
            if (persistantDataSaved.playerCosmeticCollection.Contains(cosmeticItem.ShoppableSO))
            {
                cosmeticItem.SetInCollection();
                continue;
            }

            button.onClick.AddListener(() =>
            {
                _shoppableSelected = cosmeticItem.ShoppableSO;
                ShowSelectedPlayerCosmetic();
                _itemSelected = cosmeticItem;
                for (int i = 0; i < playerCosmeticsButton.Count; i++)
                    playerCosmeticsButton[i].image.color = Color.clear;

                button.image.color = _selectedColor;
                _coinsTextBuyButton.text = _shoppableSelected.shoppableData.cost.ToString();
                _buyButton.interactable = persistantDataSaved.presiCoins >= _shoppableSelected.shoppableData.cost ? true : false;
            });
        }

        for (int i = 0; i < _presidentCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_shopItemPrefab);
            cosmeticItem.transform.SetParent(_presidentGridParent, true);
            cosmeticItem.SetCosmeticData(_presidentCosmetics[i]);
            var button = cosmeticItem.GetComponent<Button>();
            presidentCosmeticsButton.Add(button);
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
                for (int i = 0; i < presidentCosmeticsButton.Count; i++)
                    presidentCosmeticsButton[i].image.color = Color.clear;

                button.image.color = _selectedColor;
                _coinsTextBuyButton.text = _shoppableSelected.shoppableData.cost.ToString();
                _buyButton.interactable = persistantDataSaved.presiCoins >= _shoppableSelected.shoppableData.cost ? true : false;
            });
        }

        #endregion

        #region botones

        _playerButton.onClick.AddListener(() =>
        {
            _playerButton.image.color = _buttonsColor * .5f;
            _presidentButton.image.color = _buttonsColor;
        });

        _presidentButton.onClick.AddListener(() =>
        {
            _presidentButton.image.color = _buttonsColor * .5f;
            _playerButton.image.color = _buttonsColor;
        });

        #endregion

        _buyButton.onClick.AddListener(() =>
        {
            _itemSelected.SetInCollection();
            _coins.text = persistantDataSaved.presiCoins.ToString();
            _shoppableSelected = null;
            _itemSelected = null;
            _buyButton.interactable = false;
        });

        _playerButton.onClick.Invoke();
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
}
