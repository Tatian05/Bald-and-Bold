using System.Collections;
using UnityEngine;

public class RoomBasedCamera : MonoBehaviour
{
    public static RoomBasedCamera instance;
    Vector3 _velocity = Vector3.zero;
    GameManager _gameManager;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void MoveCamera(Transform pos)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCameraSmooth(pos));
    }

    IEnumerator MoveCameraSmooth(Transform pos)
    {
        var wait = new WaitForEndOfFrame();
        while(transform.position != pos.position)
        {
            transform.position = Vector3.SmoothDamp(transform.position, pos.position, ref _velocity, _gameManager.CameraSpeed);
            yield return wait;
        }
        yield return null;
    }
}
