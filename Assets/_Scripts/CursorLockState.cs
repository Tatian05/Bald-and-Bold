using UnityEngine;
public class CursorLockState : MonoBehaviour
{
    [SerializeField] CursorLockMode _lockMode;
    [SerializeField] bool _visible;
    void Start()
    {
        Cursor.lockState = _lockMode;
        Cursor.visible = _visible;
    }
}
