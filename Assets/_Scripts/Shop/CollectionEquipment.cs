using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CollectionEquipment : MonoBehaviour
{
    [Header("Inspector Variables")]
    [SerializeField] Button _playerButton, _presidentButton;
    [SerializeField] Button _playerNextCosmetic, _playerPreviousCosmetic;
    [SerializeField] Button _presidentNextCosmetic, _presidentrPreviousCosmetic;
    [SerializeField] Button _playerEquipButton, _presidentEquipButton;
    [SerializeField] GameObject _emptyTxt, _playerWindow, _presidentWindow;
    [SerializeField] TextMeshProUGUI _playerCosmeticName, _presidentCosmeticName;

    [Header("Player Sprites")]
    [SerializeField] Image _playerHead, _playerTorso, _playerRightLeg, _playerLeftLeg, _playerRightHand, _playerLeftHand, _playerTail;

    [Header("President Sprites")]
    [SerializeField] Image _presidentHead, _presidentTorso, _presidentRightLeg, _presidentLeftLeg, _presidentRightHand, _presidentLeftHand, _presidentTail;

    int _playerIndex, _presidentIndex;
    Color _buttonsColor;
    PersistantDataSaved _persistantDataSaved;
    List<CosmeticData> _playerCosmeticsList;
    List<CosmeticData> _presidentCosmeticsList;
    void Awake()
    {
        _persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        _playerCosmeticsList = _persistantDataSaved.playerCosmeticCollection;
        _presidentCosmeticsList = _persistantDataSaved.presidentCosmeticCollection;
    }
    void OnEnable()
    {
        foreach (var item in _playerCosmeticsList) item.OnStart();
        foreach (var item in _presidentCosmeticsList) item.OnStart();

        _playerCosmeticName.text = _playerCosmeticsList[_playerIndex].shoppableData.shoppableName;
        _presidentCosmeticName.text = _presidentCosmeticsList[_presidentIndex].shoppableData.shoppableName;
    }
    void Start()
    {
        _buttonsColor = _playerButton.image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        persistantDataSaved.AddCosmetic(CosmeticType.Player, Resources.Load<CosmeticData>("Cosmetics/Player/Player Default"));
        persistantDataSaved.AddCosmetic(CosmeticType.President, Resources.Load<CosmeticData>("Cosmetics/President/Presi Default"));

        if (persistantDataSaved.playerCosmeticEquiped) EquipPlayer(persistantDataSaved.playerCosmeticEquiped);
        if (persistantDataSaved.presidentCosmeticEquiped) EquipPresident(persistantDataSaved.playerCosmeticEquiped);

        _playerNextCosmetic.onClick.AddListener(() =>
        {
            if (!_playerCosmeticsList.Any()) return;
            _playerIndex = (_playerIndex + 1) % _playerCosmeticsList.Count;
            EquipPlayer(_playerCosmeticsList[_playerIndex]);
            _playerCosmeticName.text = _playerCosmeticsList[_playerIndex].shoppableData.shoppableName;
            _playerEquipButton.interactable = _playerCosmeticsList[_playerIndex] == persistantDataSaved.playerCosmeticEquiped ? false : true;
        });
        _playerPreviousCosmetic.onClick.AddListener(() =>
        {
            _playerIndex--;
            if (_playerIndex < 0) _playerIndex += _playerCosmeticsList.Count;
            EquipPlayer(_playerCosmeticsList[_playerIndex]);
            _playerEquipButton.interactable = _playerCosmeticsList[_playerIndex] == persistantDataSaved.playerCosmeticEquiped ? false : true;
        });

        _presidentNextCosmetic.onClick.AddListener(() =>
        {
            if (!_presidentCosmeticsList.Any()) return;
            _presidentIndex = (_presidentIndex + 1) % _presidentCosmeticsList.Count;
            EquipPresident(_presidentCosmeticsList[_presidentIndex]);
            _presidentCosmeticName.text = _presidentCosmeticsList[_presidentIndex].shoppableData.shoppableName;
            _presidentEquipButton.interactable = _presidentCosmeticsList[_presidentIndex] == persistantDataSaved.presidentCosmeticEquiped ? false : true;
        });
        _presidentrPreviousCosmetic.onClick.AddListener(() =>
        {
            _presidentIndex--;
            if (_presidentIndex < 0) _presidentIndex += _presidentCosmeticsList.Count;
            EquipPresident(_presidentCosmeticsList[_presidentIndex]);
            _presidentEquipButton.interactable = _presidentCosmeticsList[_presidentIndex] == persistantDataSaved.presidentCosmeticEquiped ? false : true;
        });

        _playerEquipButton.onClick.AddListener(() =>
        {
            persistantDataSaved.playerCosmeticEquiped = _playerCosmeticsList[_playerIndex];
            _playerEquipButton.interactable = false;
            if (Helpers.GameManager) Helpers.GameManager.SetPlayerSkin();
        });

        _presidentEquipButton.onClick.AddListener(() =>
        {
            persistantDataSaved.presidentCosmeticEquiped = _presidentCosmeticsList[_presidentIndex];
            _presidentEquipButton.interactable = false;
            if (Helpers.GameManager) Helpers.GameManager.SetPresidentSkin();
        });

        _playerButton.onClick.AddListener(() =>
        {
            _playerButton.image.color = _buttonsColor * .5f;
            _presidentButton.image.color = _buttonsColor;
            _presidentWindow.SetActive(false);
            if (!_playerCosmeticsList.Any())
            {
                _playerWindow.SetActive(false);
                _emptyTxt.SetActive(true);
                return;
            }
            else
            {
                _emptyTxt.SetActive(false);
                _playerWindow.SetActive(true);
            }
        });

        _presidentButton.onClick.AddListener(() =>
        {
            _presidentButton.image.color = _buttonsColor * .5f;
            _playerButton.image.color = _buttonsColor;
            _playerWindow.SetActive(false);
            if (!_presidentCosmeticsList.Any())
            {
                _presidentWindow.SetActive(false);
                _emptyTxt.SetActive(true);
                return;
            }
            else
            {
                _emptyTxt.SetActive(false);
                _presidentWindow.SetActive(true);
            }
        });

        _playerButton.onClick.Invoke();

        _playerNextCosmetic.onClick.Invoke();
        _presidentNextCosmetic.onClick.Invoke();
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
