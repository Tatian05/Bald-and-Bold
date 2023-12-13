using UnityEngine;
using BaldAndBold.Consumables;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class ConsumableSelector : MonoBehaviour
{
    [SerializeField] Button _leftArrow, _rightArrow;
    [SerializeField] Transform _content;
    [SerializeField] List<Consumables> _consumablesInventory;

    [Header("UI")]
    [SerializeField] string[] _submitTMP, _nextTMP, _beforeTMP;
    [SerializeField] TextMeshProUGUI _submitTMPText, _nextTMPText, _beforeTMPText;
    [SerializeField] GameObject _gamepadContainerGO;

    float _offset = 1.75f, _currentOffset;
    InputAction _gamepadTriggerConsumable, _gamepadNext, _gamepadBefore;
    private void Awake()
    {
        _gamepadTriggerConsumable = EventSystemScript.Instance.UIInputs.UI.Submit;
        _gamepadNext = NewInputManager.PlayerInputs.UI.Next;
        _gamepadBefore = NewInputManager.PlayerInputs.UI.Before;
       
    }
    private void Start()
    {
        _leftArrow.onClick.AddListener(Before);
        _rightArrow.onClick.AddListener(Next);

        GetComponent<Canvas>().worldCamera = Helpers.MainCamera;
        GamepadUI();

        var coll = Helpers.PersistantData.shoppablesInCollection.OfType<ConsumableData>().Distinct().Where(x => !Helpers.PersistantData.consumablesActivated.Contains(x));
        if (!coll.Any()) return;

        foreach (var item in coll)
            Instantiate(item.consumable).SetParent(_content);

        _consumablesInventory = _content.GetComponentsInChildren<Consumables>().ToList();
        SetPositions();
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(Contains.CONSUMABLE, UpdateList);
        NewInputManager.ActiveDeviceChangeEvent += GamepadUI;

        _gamepadTriggerConsumable.performed += TriggerSelectedConsumable;
        _gamepadTriggerConsumable.Enable();
        _gamepadNext.performed += TriggerMoveNext;
        _gamepadNext.Enable();
        _gamepadBefore.performed += TriggerMoveBefore;
        _gamepadBefore.Enable();
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE, UpdateList);
        NewInputManager.ActiveDeviceChangeEvent -= GamepadUI;

        _gamepadTriggerConsumable.performed -= TriggerSelectedConsumable;
        _gamepadTriggerConsumable.Disable();
        _gamepadNext.performed -= TriggerMoveNext;
        _gamepadNext.Disable();
        _gamepadBefore.performed -= TriggerMoveBefore;
        _gamepadBefore.Disable();
    }
    public void SetPositions()
    {
        if (!_consumablesInventory.Any()) return;

        _consumablesInventory[0].transform.DOScale(1.5f, .1f).SetUpdate(true);
        _consumablesInventory[0].transform.DOMove(transform.position + Vector3.zero, .2f).SetUpdate(true).OnComplete(() => { _leftArrow.interactable = true; _rightArrow.interactable = true; });
        _consumablesInventory[0].SetInteractableButton(true);
        _consumablesInventory.ForEach(x =>
        {
            int index = _consumablesInventory.IndexOf(x);

            if (index > 0)
            {
                if (index % 2 == 1) _currentOffset += _offset;
                _currentOffset *= -1;

                if (index >= _consumablesInventory.Count - 2)
                    x.transform.position = transform.position + new Vector3(_currentOffset, 0);
                else
                    x.transform.DOMove(transform.position + new Vector3(_currentOffset, 0), .2f).SetUpdate(true);

                if (transform.localScale.magnitude != 1) x.transform.DOScale(Vector3.one, .2f).SetUpdate(true);

                x.SetInteractableButton(false);
            }
        });
        _currentOffset = 0;
    }
    void Before()
    {
        if (_consumablesInventory.Count <= 1) return;
        var newArray = new Consumables[_consumablesInventory.Count];
        var count = _consumablesInventory.Count;

        _leftArrow.interactable = false;
        _rightArrow.interactable = false;
        _consumablesInventory.ForEach(x =>
        {
            int index = _consumablesInventory.IndexOf(x);
            if (index % 2 == 0)
                index += index == 0 ? 1 : -2;
            else
            {
                if (index >= count - 2)
                    index += index + 1 >= count ? -1 : 1;
                else index += 2;
            }

            newArray[index] = x;
        });

        _consumablesInventory = newArray.ToList();
        SetPositions();
    }
    void Next()
    {
        if (_consumablesInventory.Count <= 1) return;
        var newArray = new Consumables[_consumablesInventory.Count];
        var count = _consumablesInventory.Count;

        _rightArrow.interactable = false;
        _leftArrow.interactable = false;
        _consumablesInventory.ForEach(x =>
        {
            int index = _consumablesInventory.IndexOf(x);
            if (index % 2 == 0)
            {
                if (index >= count - 2)
                    index += index + 1 >= count ? -1 : 1;
                else index += 2;
            }
            else
                index += index - 1 == 0 ? -1 : -2;

            newArray[index] = x;
        });

        _consumablesInventory = newArray.ToList();
        SetPositions();
    }
    void TriggerSelectedConsumable(InputAction.CallbackContext obj) { _consumablesInventory[0]?.TriggerConsumable(); }
    void TriggerMoveNext(InputAction.CallbackContext obj) { _rightArrow.onClick.Invoke(); }
    void TriggerMoveBefore(InputAction.CallbackContext obj) { _leftArrow.onClick.Invoke(); }
    void UpdateList(params object[] param)
    {
        _consumablesInventory.Remove((Consumables)param[0]);
        SetPositions();
    }

    void GamepadUI()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _submitTMPText.text = _submitTMP[(int)NewInputManager.activeDevice - 1];
            _nextTMPText.text = _nextTMP[(int)NewInputManager.activeDevice - 1];
            _beforeTMPText.text = _beforeTMP[(int)NewInputManager.activeDevice - 1];
            _gamepadContainerGO.SetActive(true);
        }
        else
            _gamepadContainerGO.SetActive(false);
    }

    #region BUILDER

    public ConsumableSelector SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }

    #endregion

}