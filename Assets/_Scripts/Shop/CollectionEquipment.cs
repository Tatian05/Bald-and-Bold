using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class CollectionEquipment : MonoBehaviour
{
    [Header("Inspector Variables")]
    [SerializeField] Button[] _windowButtons;
    [SerializeField] Button _nextCosmetic, _previousCosmetic, _equipButton;
    [SerializeField] GameObject _cosmeticWindow, _consumableWindow, _playerWindow, _presidentWindow;
    [SerializeField] TextMeshProUGUI _cosmeticName;

    [Header("Player Sprites")]
    [SerializeField] Image _playerHead, _playerTorso, _playerRightLeg, _playerLeftLeg, _playerRightHand, _playerLeftHand, _playerTail;

    [Header("President Sprites")]
    [SerializeField] Image _presidentHead, _presidentTorso, _presidentRightLeg, _presidentLeftLeg, _presidentRightHand, _presidentLeftHand, _presidentTail;

    [Header("Consumables")]
    [SerializeField] ConsumableUICollection _consumableCollectionPrefab;
    [SerializeField] Transform _consumableContent;

    int _index;
    Color _buttonsColor;
    PersistantDataSaved _persistantDataSaved;
    List<CosmeticData> _playerCosmeticsList;
    List<CosmeticData> _presidentCosmeticsList;
    List<ConsumableData> _consumablesCollectionList;
    Action<CosmeticData> _equipCosmetic;
    void Awake()
    {
        _persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        _playerCosmeticsList = _persistantDataSaved.playerCosmeticCollection;
        _presidentCosmeticsList = _persistantDataSaved.presidentCosmeticCollection;
        _consumablesCollectionList = _persistantDataSaved.consumablesInCollection;
    }
    void OnEnable()
    {
        foreach (var item in _playerCosmeticsList) item.OnStart();
        foreach (var item in _presidentCosmeticsList) item.OnStart();
        foreach (var item in _consumablesCollectionList) item.OnStart();
    }
    void Start()
    {
        _buttonsColor = _windowButtons[0].image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        persistantDataSaved.AddCosmetic(CosmeticType.Player, Resources.Load<CosmeticData>("Shop/Cosmetics/Player/Player Default"));
        persistantDataSaved.AddCosmetic(CosmeticType.President, Resources.Load<CosmeticData>("Shop/Cosmetics/President/Presi Default"));

        if (persistantDataSaved.playerCosmeticEquiped) EquipPlayer(persistantDataSaved.playerCosmeticEquiped);
        if (persistantDataSaved.presidentCosmeticEquiped) EquipPresident(persistantDataSaved.playerCosmeticEquiped);

        List<CosmeticData> currentCollection = null;
        Func<bool> isEquiped = () => currentCollection[_index] == persistantDataSaved.playerCosmeticEquiped || currentCollection[_index] == persistantDataSaved.presidentCosmeticEquiped;

        foreach (var item in _consumablesCollectionList.Distinct())
            Instantiate(_consumableCollectionPrefab).SetParent(_consumableContent).SetImage(item.shoppableData.shopSprite).SetCount(_consumablesCollectionList.GroupBy(x => x).First(x => x.Key == item).Count());

        _nextCosmetic.onClick.AddListener(() =>
        {
            if (!currentCollection.Any()) return;
            _index = (_index + 1) % currentCollection.Count;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableData.shoppableName;
            _equipButton.interactable = !isEquiped();
        });
        _previousCosmetic.onClick.AddListener(() =>
        {
            _index--;
            if (_index < 0) _index += currentCollection.Count;
            _equipCosmetic(currentCollection[_index]);
            _equipButton.interactable = !isEquiped();
        });

        _equipButton.onClick.AddListener(() =>
        {
            if (_playerWindow.activeSelf)
            {
                persistantDataSaved.playerCosmeticEquiped = _playerCosmeticsList[_index];
                if (Helpers.GameManager) Helpers.GameManager.SetPlayerSkin();
            }
            else
            {
                persistantDataSaved.presidentCosmeticEquiped = _presidentCosmeticsList[_index];
                if (Helpers.GameManager) Helpers.GameManager.SetPresidentSkin();
            }
            _equipButton.interactable = false;
        });

        _windowButtons[0].onClick.AddListener(() =>
        {
            currentCollection = _playerCosmeticsList;
            _index = 0;
            _equipCosmetic = EquipPlayer;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableData.shoppableName;
            _cosmeticWindow.SetActive(true);
            _consumableWindow.SetActive(false);
            _presidentWindow.SetActive(false);
            _playerWindow.SetActive(true);
        });

        _windowButtons[1].onClick.AddListener(() =>
        {
            currentCollection = _presidentCosmeticsList;
            _index = 0;
            _equipCosmetic = EquipPresident;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableData.shoppableName;
            _cosmeticWindow.SetActive(true);
            _consumableWindow.SetActive(false);
            _playerWindow.SetActive(false);
            _presidentWindow.SetActive(true);
            _cosmeticName.text = currentCollection[_index].shoppableData.shoppableName;
        });

        _windowButtons[2].onClick.AddListener(() =>
        {
            _index = 0;
            _consumableWindow.SetActive(true);
            _cosmeticWindow.SetActive(false);
            _playerWindow.SetActive(false);
            _presidentWindow.SetActive(false);
        });

        var buttons = _windowButtons.ToList();
        foreach (var item in buttons)
        {
            item.onClick.AddListener(() =>
            {
                buttons.ForEach(x => x.image.color = _buttonsColor);
                item.image.color = Color.black * .5f;
            });
        }

        _windowButtons[0].onClick.Invoke();
        _cosmeticName.text = _playerCosmeticsList[_index].shoppableData.shoppableName;


        _nextCosmetic.onClick.Invoke();
    }

    void EquipPlayer(CosmeticData cosmeticData)
    {
        if (!cosmeticData) return;

        _playerHead.sprite = cosmeticData.headSprite;
        _playerTorso.sprite = cosmeticData.torsoSprite;
        _playerRightLeg.sprite = cosmeticData.rightLegSprite;
        _playerLeftLeg.sprite = cosmeticData.leftLegSprite;
        _playerRightHand.sprite = cosmeticData.rightHandSprite;
        _playerLeftHand.sprite = cosmeticData.leftHandSprite;
        if (cosmeticData.tailSprite)
        {
            _playerTail.gameObject.SetActive(true);
            _playerTail.sprite = cosmeticData.tailSprite;
        }
        else
        {
            _playerTail.sprite = null;
            _playerTail.gameObject.SetActive(false);
        }
    }
    void EquipPresident(CosmeticData cosmeticData)
    {
        if (!cosmeticData) return;

        _presidentHead.sprite = cosmeticData.headSprite;
        _presidentTorso.sprite = cosmeticData.torsoSprite;
        _presidentRightLeg.sprite = cosmeticData.rightLegSprite;
        _presidentLeftLeg.sprite = cosmeticData.leftLegSprite;
        _presidentRightHand.sprite = cosmeticData.rightHandSprite;
        _presidentLeftHand.sprite = cosmeticData.leftHandSprite;
        if (cosmeticData.tailSprite)
        {
            _presidentTail.gameObject.SetActive(true);
            _presidentTail.sprite = cosmeticData.tailSprite;
        }
        else
        {
            _presidentTail.sprite = null;
            _presidentTail.gameObject.SetActive(false);
        }
    }
}
