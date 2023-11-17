using UnityEngine;
using UnityEngine.InputSystem;
public class ConsumablesManager : MonoBehaviour
{
    [SerializeField] GameObject _consumableCollectionGO;
    [SerializeField] ParticleSystem _buffParticle, _debuffParticle;
    InputAction _openConsumablesWindow;
    private void Start()
    {
        _openConsumablesWindow = NewInputManager.PlayerInputs.Player.OpenConsumableCollection;
        _openConsumablesWindow.performed += OpenConsumableWindow;
        _openConsumablesWindow.Enable();
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_VISUAL_EFFECTS, ConsumableEffect);
    }

    void OpenConsumableWindow(InputAction.CallbackContext obj)
    {
        bool active = _consumableCollectionGO.activeSelf;
        _consumableCollectionGO.SetActive(active = !active);
        CustomTime.SetSlowDown(active ? .25f : 1f);
        EventManager.TriggerEvent(Contains.CONSUMABLE_PAUSE, active);
    }
    void ConsumableEffect(params object[] param)
    {
        var particle = Instantiate((bool)param[0] ? _buffParticle : _debuffParticle);
        particle.transform.position = transform.position + Vector3.up;
    }
    private void OnDestroy()
    {
        _openConsumablesWindow.performed -= OpenConsumableWindow;
        _openConsumablesWindow.Disable();
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_VISUAL_EFFECTS, ConsumableEffect);
        EventManager.TriggerEvent(Contains.SAVE_GAME);
        Helpers.PersistantData.SaveConsumablesData();
    }
}
