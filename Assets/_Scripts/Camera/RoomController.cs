using UnityEngine;
public class RoomController : MonoBehaviour
{
    RoomBasedCamera _camera;
    private void Start()
    {
        _camera = RoomBasedCamera.instance;
    }
    private void OnTriggerEnter2D()
    {
        _camera.MoveCamera(transform);     
    }
}
