using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class ShopWindow : MonoBehaviour
{
    [SerializeField] Button _playerButton, _presidentButton;
    [SerializeField] Button _playerBuyButton, _presidentBuyButton;
    [SerializeField] CosmeticShopItem _cosmeticShopItemPrefab;

    [Header("Player Settings")]
    [SerializeField] Transform _playerGridParent;
    [SerializeField] Image _playerHeadSprite, _playerTorsoSprite, _playerRightLegSprite, _playerLeftLegSprite, _playerRightHandSprite, _playerLeftHandSprite, _playerTailSprite;

    [Header("President Settings")]
    [SerializeField] Transform _presidentGridParent;
    [SerializeField] Image _presidentHeadSprite, _presidentTorsoSprite, _presidentRightLegSprite, _presidentLeftLegSprite, _presidentRightHandSprite, _presidentLeftHandSprite, _presidentTailSprite;

    [SerializeField] Color _selectedColor;
    [SerializeField] TextMeshProUGUI _coins;

    [SerializeField] CosmeticData[] _playerCosmetics;
    [SerializeField] CosmeticData[] _presidentCosmetics;
    CosmeticData _cosmeticSelected;
    CosmeticShopItem _itemSelected;

    Color _buttonsColor;

    void Awake()
    {
        _playerCosmetics = Resources.LoadAll<CosmeticData>("Cosmetics/Player").Where(x => x.cost != 0).ToArray();
        _presidentCosmetics = Resources.LoadAll<CosmeticData>("Cosmetics/President").Where(x => x.cost != 0).ToArray();
    }
    void OnEnable()
    {
        foreach (var item in _playerCosmetics) item.SetName();
        foreach (var item in _presidentCosmetics) item.SetName();
    }
    void Start()
    {
        _buttonsColor = _playerButton.image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        _coins.text = persistantDataSaved.coins.ToString();

        List<Button> playerCosmeticsButton = new List<Button>();
        List<Button> presidentCosmeticsButton = new List<Button>();

        #region Instanceo de Cosmetics

        for (int i = 0; i < _playerCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_cosmeticShopItemPrefab);
            cosmeticItem.transform.SetParent(_playerGridParent, true);
            cosmeticItem.SetCosmeticData(_playerCosmetics[i], _playerCosmetics[i].headSprite);
            var button = cosmeticItem.GetComponent<Button>();
            playerCosmeticsButton.Add(button);
            if (persistantDataSaved.playerCosmeticCollection.Any() && persistantDataSaved.playerCosmeticCollection.Contains(cosmeticItem.CosmeticData))
            {
                cosmeticItem.SetInCollection();
                continue;
            }

            button.onClick.AddListener(() =>
            {
                ShowSelectedPlayerCosmetic(cosmeticItem);
                _cosmeticSelected = cosmeticItem.CosmeticData;
                _itemSelected = cosmeticItem;
                for (int i = 0; i < playerCosmeticsButton.Count; i++)
                    playerCosmeticsButton[i].image.color = Color.clear;

                button.image.color = _selectedColor;
                _playerBuyButton.interactable = true;
                _playerBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = _cosmeticSelected.cost.ToString();
                _playerBuyButton.interactable = persistantDataSaved.coins >= _cosmeticSelected.cost ? true : false;
            });
        }

        for (int i = 0; i < _presidentCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_cosmeticShopItemPrefab);
            cosmeticItem.transform.SetParent(_presidentGridParent, true);
            cosmeticItem.SetCosmeticData(_presidentCosmetics[i], _presidentCosmetics[i].headSprite);
            var button = cosmeticItem.GetComponent<Button>();
            presidentCosmeticsButton.Add(button);
            if (persistantDataSaved.presidentCosmeticCollection.Any() && persistantDataSaved.presidentCosmeticCollection.Contains(cosmeticItem.CosmeticData))
            {
                cosmeticItem.SetInCollection();
                continue;
            }

            button.onClick.AddListener(() =>
            {
                ShowSelectedPresidentCosmetic(cosmeticItem);
                _cosmeticSelected = cosmeticItem.CosmeticData;
                _itemSelected = cosmeticItem;
                for (int i = 0; i < presidentCosmeticsButton.Count; i++)
                    presidentCosmeticsButton[i].image.color = Color.clear;

                button.image.color = _selectedColor;
                _presidentBuyButton.interactable = true;
                _presidentBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = _cosmeticSelected.cost.ToString();
                _presidentBuyButton.interactable = persistantDataSaved.coins >= _cosmeticSelected.cost ? true : false;
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

        _playerBuyButton.onClick.AddListener(() =>
        {
            _itemSelected.SetInCollection();
            persistantDataSaved.AddPlayerCosmetic(_cosmeticSelected);
            persistantDataSaved.Buy(_cosmeticSelected.cost);
            _coins.text = persistantDataSaved.coins.ToString();
            _cosmeticSelected = null;
            _itemSelected = null;
            _playerBuyButton.interactable = false;
        });
        _presidentBuyButton.onClick.AddListener(() =>
        {
            _itemSelected.SetInCollection();
            persistantDataSaved.AddPresidentCosmetic(_cosmeticSelected);
            persistantDataSaved.Buy(_cosmeticSelected.cost);
            _coins.text = persistantDataSaved.coins.ToString();
            _cosmeticSelected = null;
            _itemSelected = null;
            _presidentBuyButton.interactable = false;
        });

        _playerButton.onClick.Invoke();
    }

    void ShowSelectedPlayerCosmetic(CosmeticShopItem cosmeticItem)
    {
        cosmeticItem.SetCosmetics(ref _playerHeadSprite, ref _playerTorsoSprite, ref _playerRightLegSprite,
                ref _playerLeftLegSprite, ref _playerRightHandSprite, ref _playerLeftHandSprite, ref _playerTailSprite);
    }
    void ShowSelectedPresidentCosmetic(CosmeticShopItem cosmeticItem)
    {
        cosmeticItem.SetCosmetics(ref _presidentHeadSprite, ref _presidentTorsoSprite, ref _presidentRightLegSprite,
                ref _presidentLeftLegSprite, ref _presidentRightHandSprite, ref _presidentLeftHandSprite, ref _presidentTailSprite);
    }
}
