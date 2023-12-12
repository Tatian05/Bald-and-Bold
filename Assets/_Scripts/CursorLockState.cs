using UnityEngine;
public class CursorLockState : MonoBehaviour
{
    [SerializeField] CursorLockMode _lockMode;
    [SerializeField] bool _visible;
    private void OnEnable()
    {
        Cursor.lockState = _lockMode;
        Cursor.visible = _visible;

        NewInputManager.ActiveDeviceChangeEvent += SetCursor;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= SetCursor;
    }
    void SetCursor()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = _visible;
            Cursor.lockState = _lockMode;
        }
    }
}
