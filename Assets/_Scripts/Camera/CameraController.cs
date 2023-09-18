using System.Collections;
using UnityEngine;
using System;
public class CameraController : MonoBehaviour
{
    GameManager _gameManager;
    Transform _myTransform, _player;
    Action _cameraBehaviour;
    Camera _camera;

    [SerializeField] AnimationCurve _curve;
    [SerializeField] float[] _clampX = new float[2];
    [SerializeField] float[] _clampY = new float[2];
    [SerializeField] float _smooth;
    [SerializeField] bool _static;

    private void Start()
    {
        _myTransform = transform;
        _gameManager = Helpers.GameManager;
        _camera = Helpers.MainCamera;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _gameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(Shaking());
        _gameManager.EnemyManager.OnHeavyAttack += () => StartCoroutine(Shaking());

        _cameraBehaviour = _static ? delegate { } : (Action)CameraClamped;
    }
    private void LateUpdate()
    {
        _cameraBehaviour();
    }

    float _xClamp, _yClamp;
    Vector3 _targetPosition;
    void CameraClamped()
    {
        _xClamp = Mathf.Clamp(_player.position.x, _clampX[0], _clampX[1]);
        _yClamp = Mathf.Clamp(_player.position.y, _clampY[0], _clampY[1]);
        _targetPosition = new Vector3(_xClamp, _yClamp, _myTransform.position.z);
        _myTransform.position = Vector3.Lerp(_myTransform.position, _targetPosition, _smooth * Time.deltaTime);
    }
    public void StartCameraShake(float duration) => StartCoroutine(CameraShake(duration));
    public void StartShaking() => StartCoroutine(Shaking());

    public void DisableCamera(bool enabled) => _camera.enabled = enabled;
    IEnumerator CameraShake(float duration)
    {
        Vector3 startPosition = _myTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * .015f;
            float y = UnityEngine.Random.Range(-1f, 1f) * .015f;

            _myTransform.localPosition = new Vector3(startPosition.x + x, startPosition.y + y, startPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _myTransform.localPosition = startPosition;
    }
    IEnumerator Shaking()
    {
        Vector3 startPosition = _myTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < _gameManager.CameraShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strenght = _curve.Evaluate(elapsedTime / _gameManager.CameraShakeDuration);
            _myTransform.localPosition = startPosition + UnityEngine.Random.insideUnitSphere * strenght;
            yield return null;
        }

        _myTransform.localPosition = startPosition;
    }
}
