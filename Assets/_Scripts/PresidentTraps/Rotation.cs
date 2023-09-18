using System.Collections;
using UnityEngine;
public class Rotation : MonoBehaviour
{
    [SerializeField, Tooltip("Target rotation in degrees")] float _rotationAmount;
    [SerializeField, Tooltip("Rotate Direction")] Vector3 _rotateDirection;
    Quaternion _startRotation;
    Transform _myTransform;
    void Start()
    {
        _myTransform = transform;
        StartCoroutine(Rotate());
    }
    IEnumerator Rotate()
    {
        _startRotation = _myTransform.rotation;
        while (Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime <= 1)
        {
            _myTransform.rotation = _startRotation * Quaternion.AngleAxis(Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime * _rotationAmount, _rotateDirection);
            yield return null;
        }
    }
}
