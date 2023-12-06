using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class OnBackJoystick : MonoBehaviour
{
    [SerializeField] UnityEvent _onBackEvent;
    InputAction _cancelUI;
    bool _binding;
    private void Awake()
    {
        _cancelUI = EventSystemScript.Instance.UIInputs.UI.Cancel;
    }
    private void OnEnable()
    {
        NewInputManager.rebindStarted += OnRebindStarted;
        NewInputManager.RebindComplete += OnRebindFinish;
        NewInputManager.RebindCanceled += OnRebindFinish;

        _cancelUI.performed += CancelBinding;
        _cancelUI.Enable();
    }
    private void OnDisable()
    {
        NewInputManager.rebindStarted -= OnRebindStarted;
        NewInputManager.RebindComplete -= OnRebindFinish;
        NewInputManager.RebindCanceled -= OnRebindFinish;

        _cancelUI.performed -= CancelBinding;
        _cancelUI?.Disable();
    }
    void OnRebindStarted(InputAction a, int b) { DisableAction(); }
    void OnRebindFinish() { Invoke(nameof(EnableAction), .05f); }
    void EnableAction() { _cancelUI?.Enable(); }
    void DisableAction() { _cancelUI?.Disable(); }
    private void CancelBinding(InputAction.CallbackContext obj)
    {
        _onBackEvent.Invoke();
    }
}
