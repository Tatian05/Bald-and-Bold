using UnityEngine;
public class CursorLockState : MonoBehaviour
{
    [SerializeField] CursorLockMode _lockMode;
    [SerializeField] bool _visible;
    private void OnEnable()
    {
        Cursor.lockState = _lockMode;
        Cursor.visible = _visible;

        EventManager.SubscribeToEvent(Contains.ON_CONTROLS_CHANGED, SetCursor);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_CONTROLS_CHANGED, SetCursor);     
    }
    void SetCursor(params object[] param)
    {
        if ((string)param[0] == "Gamepad")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
