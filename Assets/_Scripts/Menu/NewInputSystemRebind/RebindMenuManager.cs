using UnityEngine;
using UnityEngine.InputSystem;
public class RebindMenuManager : MonoBehaviour
{
    [SerializeField] InputActionReference[] _action;
    private void OnEnable()
    {
        foreach (var item in _action) item.action.Disable();
    }
    private void OnDisable()
    {
        foreach (var item in _action) item.action.Enable();
    }
}
