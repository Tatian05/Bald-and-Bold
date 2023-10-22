using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public class ShowKeyUI : MonoBehaviour
{
    [SerializeField] KeysUI _keyUI;

    [SerializeField] ActionBinding[] _actions;
    ActionBinding _currentActionBinding;
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(Contains.ON_CONTROLS_CHANGED, SetActionBinding);
        SetActionBinding(OnControlsChange.Instance.CurrentControl);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_CONTROLS_CHANGED, SetActionBinding);
    }
    void SetActionBinding(params object[] param)
    {
        _currentActionBinding = _actions.Single(x => x.inputBinding.groups == (string)param[0]);
        _keyUI?.SetImage(_currentActionBinding.inputBinding.path).
                SetText(NewInputManager.GetBindingName(_currentActionBinding.action.name, _currentActionBinding.selectedBinding));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() || collision.GetComponent<PlayerShop>()) _keyUI = FRY_KeysUI.Instance.pool.GetObject().
                SetPosition(transform.position + Vector3.up).
                SetImage(_currentActionBinding.inputBinding.path).
                SetText(NewInputManager.GetBindingName(_currentActionBinding.action.name, _currentActionBinding.selectedBinding)); ;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() || collision.GetComponent<PlayerShop>()) _keyUI.SetPosition(transform.position + Vector3.up);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() && _keyUI != null || collision.GetComponent<PlayerShop>() && _keyUI != null) _keyUI.ReturnObject();
    }
    private void OnValidate()
    {
        foreach (var item in _actions) item.Set();
    }
    private void OnDestroy()
    {
        if (_keyUI) _keyUI.ReturnObject();
    }
}

[System.Serializable]
public class ActionBinding
{
    [SerializeField, Range(0, 10)] public int selectedBinding;
    [SerializeField] public InputActionReference action;
    [SerializeField] public InputBinding inputBinding;

    public void Set()
    {
        if (action == null) return;

        if (action.action.bindings.Count > selectedBinding)
            inputBinding = action.action.bindings[selectedBinding];
    }
}
