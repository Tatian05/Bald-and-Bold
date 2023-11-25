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
    CosmeticData[] _playerCosmeticsList;
    CosmeticData[] _presidentCosmeticsList;
    ConsumableData[] _consumablesCollectionList;
    Action<CosmeticData> _equipCosmetic;
    void Awake()
    {
        _persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        _playerCosmeticsList = _persistantDataSaved.playerCosmeticCollection.ToArray();
        _presidentCosmeticsList = _persistantDataSaved.presidentCosmeticCollection.ToArray();
        _consumablesCollectionList = _persistantDataSaved.consumablesInCollection.ToArray();
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

        persistantDataSaved.AddCosmetic(CosmeticType.Player, Resources.Load<CosmeticData>("Player Default"));
        persistantDataSaved.AddCosmetic(CosmeticType.President, Resources.Load<CosmeticData>("Presi Default"));

        if (persistantDataSaved.playerCosmeticEquiped) EquipPlayer(persistantDataSaved.playerCosmeticEquiped);
        if (persistantDataSaved.presidentCosmeticEquiped) EquipPresident(persistantDataSaved.playerCosmeticEquiped);

        List<CosmeticData> currentCollection = null;
        Func<bool> isEquiped = () => currentCollection[_index] == persistantDataSaved.playerCosmeticEquiped || currentCollection[_index] == persistantDataSaved.presidentCosmeticEquiped;

        foreach (var item in _consumablesCollectionList.Distinct())
            Instantiate(_consumableCollectionPrefab).SetParent(_consumableContent).SetImage(item.shopSprite).SetCount(_consumablesCollectionList.GroupBy(x => x).First(x => x.Key == item).Count());

        _nextCosmetic.onClick.AddListener(() =>
        {
            if (!currentCollection.Any()) return;
            _index = (_index + 1) % currentCollection.Count;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
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
            currentCollection = _playerCosmeticsList.ToList();
            _index = 0;
            _equipCosmetic = EquipPlayer;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _cosmeticWindow.SetActive(true);
            _consumableWindow.SetActive(false);
            _presidentWindow.SetActive(false);
            _playerWindow.SetActive(true);
        });

        _windowButtons[1].onClick.AddListener(() =>
        {
            currentCollection = _presidentCosmeticsList.ToList();
            _index = 0;
            _equipCosmetic = EquipPresident;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _cosmeticWindow.SetActive(true);
            _consumableWindow.SetActive(false);
            _playerWindow.SetActive(false);
            _presidentWindow.SetActive(true);
            _cosmeticName.text = currentCollection[_index].shoppableName;
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
        _cosmeticName.text = _playerCosmeticsList[_index].shoppableName;


        _nextCosmetic.onClick.Invoke();
    }

    void EquipPlayer(CosmeticData cosmeticData)
    {
        if (!cosmeticData) return;

        _playerHead.sprite = cosmeticData.HeadSprite;
        _playerTorso.sprite = cosmeticData.TorsoSprite;
        _playerRightLeg.sprite = cosmeticData.RightLegSprite;
        _playerLeftLeg.sprite = cosmeticData.LeftLegSprite;
        _playerRightHand.sprite = cosmeticData.RightHandSprite;
        _playerLeftHand.sprite = cosmeticData.LeftHandSprite;
        if (cosmeticData.TailSprite)
        {
            _playerTail.gameObject.SetActive(true);
            _playerTail.sprite = cosmeticData.TailSprite;
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

        _presidentHead.sprite = cosmeticData.HeadSprite;
        _presidentTorso.sprite = cosmeticData.TorsoSprite;
        _presidentRightLeg.sprite = cosmeticData.RightLegSprite;
        _presidentLeftLeg.sprite = cosmeticData.LeftLegSprite;
        _presidentRightHand.sprite = cosmeticData.RightHandSprite;
        _presidentLeftHand.sprite = cosmeticData.LeftHandSprite;
        if (cosmeticData.TailSprite)
        {
            _presidentTail.gameObject.SetActive(true);
            _presidentTail.sprite = cosmeticData.TailSprite;
        }
        else
        {
            _presidentTail.sprite = null;
            _presidentTail.gameObject.SetActive(false);
        }
    }
}
