using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CollectionEquipment : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button _playerButton, _presidentButton;
    [SerializeField] Button _playerNextCosmetic, _playerPreviousCosmetic;
    [SerializeField] Button _presidentNextCosmetic, _presidentrPreviousCosmetic;
    [SerializeField] Button _playerEquipButton, _presidentEquipButton;
    [SerializeField] GameObject _emptyTxt, _playerWindow, _presidentWindow;

    [Header("Player Sprites")]
    [SerializeField] Image _playerHead, _playerTorso, _playerRightLeg, _playerLeftLeg, _playerRightHand, _playerLeftHand, _playerTail;

    [Header("President Sprites")]
    [SerializeField] Image _presidentHead, _presidentTorso, _presidentRightLeg, _presidentLeftLeg, _presidentRightHand, _presidentLeftHand, _presidentTail;

    int _playerIndex, _presidentIndex;
    Color _buttonsColor;
    void Start()
    {
        _buttonsColor = _playerButton.image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        if (persistantDataSaved.playerCosmeticEquiped) EquipPlayer(persistantDataSaved.playerCosmeticEquiped);
        if (persistantDataSaved.presidentCosmeticEquiped) EquipPresident(persistantDataSaved.playerCosmeticEquiped);

        _playerNextCosmetic.onClick.AddListener(() =>
        {
            if (!persistantDataSaved.playerCosmeticCollection.Any()) return;
            _playerIndex = (_playerIndex + 1) % persistantDataSaved.playerCosmeticCollection.Count;
            EquipPlayer(persistantDataSaved.playerCosmeticCollection[_playerIndex]);
            _playerEquipButton.interactable = persistantDataSaved.playerCosmeticCollection[_playerIndex] == persistantDataSaved.playerCosmeticEquiped ? false : true;
        });
        _playerPreviousCosmetic.onClick.AddListener(() =>
        {
            _playerIndex--;
            if (_playerIndex < 0) _playerIndex += persistantDataSaved.playerCosmeticCollection.Count;
            EquipPlayer(persistantDataSaved.playerCosmeticCollection[_playerIndex]);
            _playerEquipButton.interactable = persistantDataSaved.playerCosmeticCollection[_playerIndex] == persistantDataSaved.playerCosmeticEquiped ? false : true;
        });

        _presidentNextCosmetic.onClick.AddListener(() =>
        {
            if (!persistantDataSaved.presidentCosmeticCollection.Any()) return;
            _presidentIndex = (_presidentIndex + 1) % persistantDataSaved.presidentCosmeticCollection.Count;
            EquipPresident(persistantDataSaved.presidentCosmeticCollection[_presidentIndex]);
            _presidentEquipButton.interactable = persistantDataSaved.presidentCosmeticCollection[_presidentIndex] == persistantDataSaved.presidentCosmeticEquiped ? false : true;
        });
        _presidentrPreviousCosmetic.onClick.AddListener(() =>
        {
            _presidentIndex--;
            if (_presidentIndex < 0) _presidentIndex += persistantDataSaved.presidentCosmeticCollection.Count;
            EquipPresident(persistantDataSaved.presidentCosmeticCollection[_presidentIndex]);
            _presidentEquipButton.interactable = persistantDataSaved.presidentCosmeticCollection[_presidentIndex] == persistantDataSaved.presidentCosmeticEquiped ? false : true;
        });

        _playerEquipButton.onClick.AddListener(() =>
        {
            persistantDataSaved.playerCosmeticEquiped = persistantDataSaved.playerCosmeticCollection[_playerIndex];
            _playerEquipButton.interactable = false;
            if (Helpers.GameManager) Helpers.GameManager.SetPlayerSkin();
        });

        _presidentEquipButton.onClick.AddListener(() =>
        {
            persistantDataSaved.presidentCosmeticEquiped = persistantDataSaved.presidentCosmeticCollection[_presidentIndex];
            _presidentEquipButton.interactable = false;
            if (Helpers.GameManager) Helpers.GameManager.SetPresidentSkin();
        });

        _playerButton.onClick.AddListener(() =>
        {
            _playerButton.image.color = _buttonsColor * .5f;
            _presidentButton.image.color = _buttonsColor;
            _presidentWindow.SetActive(false);
            if (!persistantDataSaved.playerCosmeticCollection.Any())
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
            if (!persistantDataSaved.presidentCosmeticCollection.Any())
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
