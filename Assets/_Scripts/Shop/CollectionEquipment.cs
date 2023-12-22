using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class CollectionEquipment : MonoBehaviour
{
    [Header("Inspector Variables")]
    [SerializeField] GameObject[] _windowContainers;
    [SerializeField] Button[] _windowButtons;
    [SerializeField] Button _nextCosmetic, _previousCosmetic, _equipButton;
    [SerializeField] GameObject _cosmeticWindow, _playerWindow, _presidentWindow, _weaponsWindow, _bulletsWindow;
    [SerializeField] TextMeshProUGUI _cosmeticName;

    [Header("Player Sprites")]
    [SerializeField] Image _playerHead, _playerTorso, _playerRightLeg, _playerLeftLeg, _playerRightHand, _playerLeftHand, _playerTail;

    [Header("President Sprites")]
    [SerializeField] Image _presidentHead, _presidentTorso, _presidentRightLeg, _presidentLeftLeg, _presidentRightHand, _presidentLeftHand, _presidentTail;

    [Header("Weapon Sprites")]
    [SerializeField] Image _weaponPreviewImg;
    [SerializeField] TextMeshProUGUI _weaponTypeTxt;
    [SerializeField] GameObject _weaponTypeParentGO;

    [Header("Bullet Sprites")]
    [SerializeField] Image _bulletPreviewImg;
    [SerializeField] Image _grenadePreviewImg;

    [Header("Consumables")]
    [SerializeField] ConsumableUICollection _consumableCollectionPrefab;
    [SerializeField] Transform _consumableContent;

    int _index;
    Color _buttonsColor;
    PersistantData _persistantData;
    CosmeticData[] _playerCosmeticsList;
    CosmeticData[] _presidentCosmeticsList;
    WeaponSkinData[] _weaponCollectionList;
    BulletData[] _bulletCollectionList;
    BulletData[] _grenadeCollectionList;
    ConsumableData[] _consumablesCollectionList;
    Action<ShoppableSO> _equipCosmetic;
    CosmeticData _playerCosmeticEquiped, _presidentCosmeticEquiped;
    BulletData _bulletEquiped, _grenadeEquiped;
    WeaponSkinData _pistolEquiped, _rifleEquiped, _sniperEquiped, _grenadeLauncherEquiped;
    private void Awake()
    {
        _persistantData = Helpers.PersistantData;
    }
    void OnEnable()
    {
        _playerCosmeticEquiped = _persistantData.playerCosmeticEquiped;
        _presidentCosmeticEquiped = _persistantData.presidentCosmeticEquiped;
        _bulletEquiped = _persistantData.bulletEquiped;
        _grenadeEquiped = _persistantData.grenadeEquiped;
        _pistolEquiped = _persistantData.pistolEquiped;
        _rifleEquiped = _persistantData.rifleEquiped;
        _sniperEquiped = _persistantData.sniperEquiped;
        _grenadeLauncherEquiped = _persistantData.grenadeLauncherEquiped;

        //_persistantData.AddShoppableToCollection(_playerCosmeticEquiped);
        //_persistantData.AddShoppableToCollection(_presidentCosmeticEquiped);
        //_persistantData.AddShoppableToCollection(_bulletEquiped);

        _playerCosmeticsList = _persistantData.shoppablesInCollection.OfType<CosmeticData>().Where(x => x.cosmeticType == CosmeticType.Player).ToArray();
        _presidentCosmeticsList = _persistantData.shoppablesInCollection.OfType<CosmeticData>().Where(x => x.cosmeticType == CosmeticType.President).ToArray();
        _weaponCollectionList = _persistantData.shoppablesInCollection.OfType<WeaponSkinData>().ToArray();
        _consumablesCollectionList = _persistantData.shoppablesInCollection.OfType<ConsumableData>().ToArray();
        _bulletCollectionList = _persistantData.shoppablesInCollection.OfType<BulletData>().Where(x => x.bulletType == BulletType.Bullet).ToArray();
        _grenadeCollectionList = _persistantData.shoppablesInCollection.OfType<BulletData>().Where(x => x.bulletType == BulletType.Grenade).ToArray();

        foreach (var item in FList.Cast(_playerCosmeticsList.OfType<ShoppableSO>()) + _presidentCosmeticsList + _weaponCollectionList + _bulletCollectionList + _grenadeCollectionList + _consumablesCollectionList)
            item.OnStart();
    }
    void Start()
    {
        _buttonsColor = _windowButtons[0].image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        EquipPlayer(_playerCosmeticEquiped);
        EquipPresident(_presidentCosmeticEquiped);
        EquipBullet(_bulletEquiped);
        EquipGrenade(_grenadeEquiped);
        EquipWeapon(_pistolEquiped);

        List<ShoppableSO> currentCollection = null;

        bool isEquiped() => currentCollection[_index] == _playerCosmeticEquiped
        || currentCollection[_index] == _presidentCosmeticEquiped
        || currentCollection[_index] == _bulletEquiped
        || currentCollection[_index] == _grenadeEquiped
        || currentCollection[_index] == _pistolEquiped
        || currentCollection[_index] == _rifleEquiped
        || currentCollection[_index] == _sniperEquiped
        || currentCollection[_index] == _grenadeLauncherEquiped;

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
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _equipCosmetic(currentCollection[_index]);
            _equipButton.interactable = !isEquiped();
        });

        _equipButton.onClick.AddListener(() =>
        {
            EquipButton();
            _equipButton.interactable = false;
        });

        var buttonAndWindow = _windowButtons.Zip(_windowContainers, (x, y) => Tuple.Create(x, y));
        foreach (var item in buttonAndWindow)
        {
            item.Item1.onClick.AddListener(() =>
            {
                _index = 0;
                for (int i = 0; i < _windowButtons.Length; i++)
                {
                    _windowButtons[i].image.color = _buttonsColor;
                    _windowContainers[i].gameObject.SetActive(false);
                }
                item.Item1.image.color = Color.black * .5f;
                item.Item2.gameObject.SetActive(true);
            });
        }


        _windowButtons[0].onClick.AddListener(() =>
        {
            currentCollection = _playerCosmeticsList.Cast<ShoppableSO>().ToList();
            _equipCosmetic = EquipPlayer;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _cosmeticWindow.SetActive(true);
        });

        _windowButtons[1].onClick.AddListener(() =>
        {
            currentCollection = _presidentCosmeticsList.Cast<ShoppableSO>().ToList();
            _equipCosmetic = EquipPresident;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _cosmeticWindow.SetActive(true);
        });

        _windowButtons[2].onClick.AddListener(() =>
        {
            currentCollection = _weaponCollectionList.Cast<ShoppableSO>().ToList();
            _equipCosmetic = EquipWeapon;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _cosmeticWindow.SetActive(true);
        });

        _windowButtons[3].onClick.AddListener(() =>
        {
            currentCollection = _bulletCollectionList.Cast<ShoppableSO>().ToList();
            _equipCosmetic = EquipBullet;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _cosmeticWindow.SetActive(true);
        });

        _windowButtons[4].onClick.AddListener(() =>
        {
            currentCollection = _grenadeCollectionList.Cast<ShoppableSO>().ToList();
            _equipCosmetic = EquipGrenade;
            _equipCosmetic(currentCollection[_index]);
            _cosmeticName.text = currentCollection[_index].shoppableName;
            _cosmeticWindow.SetActive(true);
        });

        _windowButtons[5].onClick.AddListener(() =>
        {
            _cosmeticWindow.SetActive(false);
        });

        _windowButtons[0].onClick.Invoke();
        _cosmeticName.text = _playerCosmeticsList[_index].shoppableName;

        _nextCosmetic.onClick.Invoke();
    }
    void EquipButton()
    {
        if (_playerWindow.activeSelf)
        {
            _playerCosmeticEquiped = _playerCosmeticsList[_index];
            _persistantData.playerCosmeticEquiped = _playerCosmeticEquiped;
            if (Helpers.GameManager) Helpers.GameManager.SetPlayerSkin();
        }
        else if (_presidentWindow.activeSelf)
        {
            _presidentCosmeticEquiped = _presidentCosmeticsList[_index];
            _persistantData.presidentCosmeticEquiped = _presidentCosmeticEquiped;
            if (Helpers.GameManager) Helpers.GameManager.SetPresidentSkin();
        }
        else if (_weaponsWindow.activeSelf) EquipCurrentWeapon(_weaponCollectionList[_index])();
        else if (_bulletsWindow.activeSelf)
        {
            _bulletEquiped = _bulletCollectionList[_index];
            _persistantData.bulletEquiped = _bulletEquiped;
        }
        else
        {
            _grenadeEquiped = _grenadeCollectionList[_index];
            _persistantData.grenadeEquiped = _grenadeEquiped;
        }
    }
    public Action EquipCurrentWeapon(WeaponSkinData weapon) => weapon.fireWeaponType switch
    {
        FireWeaponType.Pistol => delegate { _pistolEquiped = weapon; _persistantData.pistolEquiped = weapon; }
        ,
        FireWeaponType.Rifle => delegate { _rifleEquiped = weapon; _persistantData.rifleEquiped = weapon; }
        ,
        FireWeaponType.Sniper => delegate { _sniperEquiped = weapon; _persistantData.sniperEquiped = weapon; }
        ,
        FireWeaponType.GrenadeLauncher => delegate { _grenadeLauncherEquiped = weapon; _persistantData.grenadeLauncherEquiped = weapon; }
        ,
        _ => throw new ArgumentOutOfRangeException()
    };
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
    void EquipGrenade(ShoppableSO shoppableSO)
    {
        var grenadeData = shoppableSO as BulletData;
        if (!grenadeData) return;

        _grenadePreviewImg.sprite = grenadeData.shopSprite;
    }
    void EquipWeapon(ShoppableSO shoppableSO)
    {
        var weaponData = shoppableSO as WeaponSkinData;
        if (!weaponData) return;

        _weaponTypeParentGO.SetActive(true);
        _weaponTypeTxt.text = weaponData.weaponTypeText;
        _weaponPreviewImg.sprite = shoppableSO.shopSprite;
    }
}