using UnityEngine;
using UnityEngine.InputSystem;
public class ConsumablesManager : MonoBehaviour
{
    [SerializeField] ParticleSystem _buffParticle, _debuffParticle;
    [SerializeField] ConsumableSelector _consumablesWindowPrefab;

    ConsumableSelector _consumablesWindow;
    InputAction _openConsumablesWindow;
    bool _active;
    private void Start()
    {
        _openConsumablesWindow = NewInputManager.PlayerInputs.Player.OpenConsumableCollection;
        _openConsumablesWindow.performed += OpenConsumableWindow;
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_VISUAL_EFFECTS, ConsumableEffect);
        Helpers.LevelTimerManager.OnLevelDefeat += OnLevelEnd;
    }

    private void Update()
    {
        _consumablesWindow?.SetPosition(transform.position + (Vector3.up * 3));
    }

    void OpenConsumableWindow(InputAction.CallbackContext obj)
    {
        if (!_consumablesWindow) _consumablesWindow = Instantiate(_consumablesWindowPrefab);

        _active = _consumablesWindow.gameObject.activeSelf;
        _consumablesWindow.gameObject.SetActive(_active = !_active);
        CustomTime.SetSlowDown(_active ? .25f : 1f);
        if (_active)
        {
            NewInputManager.DisablePlayer();
            _openConsumablesWindow.Enable();
        }
        else
        {
            _openConsumablesWindow.Disable();
            NewInputManager.EnablePlayer();
        }
    }
    void ConsumableEffect(params object[] param)
    {
        var particle = Instantiate((bool)param[0] ? _buffParticle : _debuffParticle);
        particle.transform.position = transform.position + Vector3.up;
    }
    void OnLevelEnd()
    {
        if (!_active) return;

        _consumablesWindow.gameObject.SetActive(false);
        CustomTime.SetSlowDown(1f);
    }
    private void OnDestroy()
    {
        _openConsumablesWindow.performed -= OpenConsumableWindow;
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_VISUAL_EFFECTS, ConsumableEffect);
    }
}
