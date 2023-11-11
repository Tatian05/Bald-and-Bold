using UnityEngine;
using UnityEngine.InputSystem;
public class ConsumablesManager : MonoBehaviour
{
    [SerializeField] GameObject _consumableCollectionGO;
    InputAction _openConsumablesWindow;
    private void Start()
    {
        _openConsumablesWindow = NewInputManager.PlayerInputs.Player.OpenConsumableCollection;
        _openConsumablesWindow.performed += OpenConsumableWindow;
        _openConsumablesWindow.Enable();
    }

    void OpenConsumableWindow(InputAction.CallbackContext obj)
    {
        bool active = _consumableCollectionGO.activeSelf;
        _consumableCollectionGO.SetActive(active = !active);
        CustomTime.SetTimeScale(_consumableCollectionGO ? 1f : .1f);
        EventManager.TriggerEvent(Contains.CONSUMABLE_PAUSE, active);
    }

    private void OnDestroy()
    {
        _openConsumablesWindow.performed -= OpenConsumableWindow;
        _openConsumablesWindow.Disable();
    }
}
