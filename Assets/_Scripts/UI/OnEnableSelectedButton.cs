using UnityEngine;
using UnityEngine.InputSystem;
public class OnEnableSelectedButton : MonoBehaviour
{
    [SerializeField] GameObject _onEnableSelectedButton;
    private void OnEnable()
    {
        if (Gamepad.current == null) return;
        EventSystemScript.Instance.AddToStack(_onEnableSelectedButton);
    }
    private void OnDisable()
    {
        EventSystemScript.Instance.RemoveToStack();
    }
}
