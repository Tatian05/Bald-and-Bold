using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OnBackJoystick : MonoBehaviour
{
    [SerializeField] Button _onBackButton;
    InputAction _cancelUI;
    private void Awake()
    {
        _cancelUI = EventSystemScript.Instance.UIInputs.UI.Cancel;     
    }
    private void OnEnable()
    {
       EventSystemScript.Instance.UIInputs.UI.Cancel.performed += CancelBinding;
        _cancelUI.Enable();
    }
    private void OnDisable()
    {
        EventSystemScript.Instance.UIInputs.UI.Cancel.performed -= CancelBinding;
        _cancelUI?.Disable();
    }
    private void CancelBinding(InputAction.CallbackContext obj)
    {
        EventSystemScript.Instance.SetCurrentGameObjectSelected(_onBackButton.gameObject);
    }
}
