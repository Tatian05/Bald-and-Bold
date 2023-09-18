using System.Collections;
using UnityEngine;
public class CameraAnimation : MonoBehaviour
{
    [SerializeField] Vector3 _presidentRoomPos;
    [SerializeField] int _zoomSize;

    CameraController _cameraController;
    private void Start()
    {
        _cameraController = GetComponent<CameraController>();
    }
    public void StartVictoryDefeatTransicion() => StartCoroutine(DefeatVictoryLerp());
    public void StartInitialTransicion() => StartCoroutine(InitialLerp());
    IEnumerator DefeatVictoryLerp()
    {
        _cameraController.enabled = false;
        float timer = 0;
        Vector3 initialPos = transform.position;
        while (timer <= 1f)
        {
            timer += Time.deltaTime;
            transform.position = Helpers.LevelTimerManager.Timer <= Helpers.LevelTimerManager.LevelMaxTime ? Vector3.Lerp(initialPos, _presidentRoomPos, timer / 1f) :
                                                                                                             Vector3.Lerp(initialPos, _presidentRoomPos, timer / 1f);
            Helpers.GameManager.CinematicManager.LerpSize(10, _zoomSize, timer / 1f);
            yield return null;
        }
    }
    IEnumerator InitialLerp()
    {
        _cameraController.enabled = false;
        float timer = 0;
        Vector3 initialPos = transform.position;
        transform.position = _presidentRoomPos;
        Helpers.GameManager.CinematicManager.SetSizeCinematicCamera(_zoomSize);
        yield return new WaitForSeconds(1f);
        while (timer <= 1f)
        {
            timer += Time.deltaTime;
            if (!_cameraController.enabled) transform.position = Vector3.Lerp(_presidentRoomPos, initialPos, timer / 1f);
            if (timer >= .8f && !_cameraController.enabled) _cameraController.enabled = true;
            Helpers.GameManager.CinematicManager.LerpSize(_zoomSize, 10, timer / 1f);
            yield return null;
        }
    }
}
