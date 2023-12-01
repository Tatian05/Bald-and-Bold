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
    [SerializeField] GameObject _cosmeticWindow, _consumableWindow, _playerWindow, _presidentWindow, _bulletsWindow;
    [SerializeField] TextMeshProUGUI _cosmeticName;

    [Header("Player Sprites")]
    [SerializeField] Image _playerHead, _playerTorso, _playerRightLeg, _playerLeftLeg, _playerRightHand, _playerLeftHand, _playerTail;

    [Header("President Sprites")]
    [SerializeField] Image _presidentHead, _presidentTorso, _presidentRightLeg, _presidentLeftLeg, _presidentRightHand, _presidentLeftHand, _presidentTail;

    [Header("Bullet Sprite")]
    [SerializeField] Image _bulletPreviewImg;

    [Header("Consumables")]
    [SerializeField] ConsumableUICollection _consumableCollectionPrefab;
    [SerializeField] Transform _consumableContent;

    int _index;
    Color _buttonsColor;
    PersistantData _persistantData;
    CosmeticData[] _playerCosmeticsList;
    CosmeticData[] _presidentCosmeticsList;
    BulletData[] _bulletCollectionList;
    ConsumableData[] _consumablesCollectionList;
    Action<ShoppableSO> _equipCosmetic;
    CosmeticData _playerCosmeticEquiped, _presidentCosmeticEquiped;
    BulletData _bulletEquiped;
    private void Awake()
    {
        _persistantData = Helpers.PersistantData;
    }
    void OnEnable()
    {
        _playerCosmeticEquiped = _persistantData.playerCosmeticEquiped;
        _presidentCosmeticEquiped = _persistantData.presidentCosmeticEquiped;
        _bulletEquiped = _persistantData.bulletEquiped;

        //_persistantData.AddShoppableToCollection(_playerCosmeticEquiped);
        //_persistantData.AddShoppableToCollection(_presidentCosmeticEquiped);
        //_persistantData.AddShoppableToCollection(_bulletEquiped);

        _playerCosmeticsList = _persistantData.shoppablesInCollection.OfType<CosmeticData>().Where(x => x.cosmeticType == CosmeticType.Player).ToArray();
        _presidentCosmeticsList = _persistantData.shoppablesInCollection.OfType<CosmeticData>().Where(x => x.cosmeticType == CosmeticType.President).ToArray();
        _consumablesCollectionList = _persistantData.shoppablesInCollection.OfType<ConsumableData>().ToArray();
        _bulletCollectionList = _persistantData.shoppablesInCollection.OfType<BulletData>().ToArray();

        foreach (var item in _playerCosmeticsList.OfType<ShoppableSO>().Concat(_presidentCosmeticsList).Concat(_consumablesCollectionList).Concat(_bulletCollectionList))
            item.OnStart();
    }
    void Start()
    {
        _buttonsColor = _windowButtons[0].image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        EquipPlayer(_playerCosmeticEquiped);
        EquipPresident(_presidentCosmeticEquiped);
        EquipBullet(_bulletEquiped);

        List<ShoppableSO> currentCollection = null;
        Func<bool> isEquiped = () => currentCollection[_index] == _playerCosmeticEquiped || currentCollection[_index] == _presidentCosmeticEquiped || currentCollection[_index] == _bulletEquiped;

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
                _persistantData.playerCosmeticEquiped = _playerCosmeticsList[_index];
                if (Helpers.GameManager) Helpers.GameManager.SetPlayerSkin();
            }
            else if (_presidentWindow.activeSelf)
            {
                _persistantData.presidentCosmeticEquiped = _presidentCosmeticsList[_index];
                if (Helpers.GameManager) Helpers.GameManager.SetPresidentSkin();
            }
            else _persistantData.bulletEquiped = _bulletCollectionList[_index];
            _equipButton.interactable = false;
        });

        var buttons = _windowButtons.ToList();
        foreach (var item in buttons)
        {
            item.onClick.AddListener(() =>
            {
                buttons.ForEach(x => x.image.color = _buttonsColor);
                item.image.color = Color.black * .5f;
                _index = 0;
            });
        }
        _windowButtons[0].onClick.AddListener(() =>
        {
            currentCollection = _playerCosmeticsList.OfType<ShoppableSO>().ToList();
            _equipCosmetic = EquipPlayer;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _consumableWindow.SetActive(false);
            _presidentWindow.SetActive(false);
            _bulletsWindow.SetActive(false);
            _cosmeticWindow.SetActive(true);
            _playerWindow.SetActive(true);
        });

        _windowButtons[1].onClick.AddListener(() =>
        {
            currentCollection = _presidentCosmeticsList.OfType<ShoppableSO>().ToList();
            _equipCosmetic = EquipPresident;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _consumableWindow.SetActive(false);
            _playerWindow.SetActive(false);
            _bulletsWindow.SetActive(false);
            _cosmeticWindow.SetActive(true);
            _presidentWindow.SetActive(true);
            _cosmeticName.text = currentCollection[_index].shoppableName;
        });

        _windowButtons[2].onClick.AddListener(() =>
        {
            _cosmeticWindow.SetActive(false);
            _playerWindow.SetActive(false);
            _presidentWindow.SetActive(false);
            _bulletsWindow.SetActive(false);
            _consumableWindow.SetActive(true);
        });

        _windowButtons[3].onClick.AddListener(() =>
        {
            currentCollection = _bulletCollectionList.OfType<ShoppableSO>().ToList();
            _equipCosmetic = EquipBullet;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _consumableWindow.SetActive(false);
            _playerWindow.SetActive(false);
            _presidentWindow.SetActive(false);
            _cosmeticWindow.SetActive(true);
            _bulletsWindow.SetActive(true);
        });

        _windowButtons[0].onClick.Invoke();
        _cosmeticName.text = _playerCosmeticsList[_index].shoppableName;

        _nextCosmetic.onClick.Invoke();
    }

    void EquipPlayer(ShoppableSO shoppableSO)
    {
        var cosmeticData = shoppableSO as CosmeticData;
        if (!cosmeticData) return;

        _playerHead.sprite = cosmeticData.shopSprite;
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
    void EquipPresident(ShoppableSO shoppable)
    {
        var cosmeticData = shoppable as CosmeticData;
        if (!cosmeticData) return;

        _presidentHead.sprite = cosmeticData.shopSprite;
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
    void EquipBullet(ShoppableSO shoppableSO)
    {
        var bulletData = shoppableSO as BulletData;
        if (!bulletData) return;

        _bulletPreviewImg.sprite = bulletData.shopSprite;
    }
}
