using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpRotation : MonoBehaviour
{
    Quaternion _initialQuaterion;
    [SerializeField] Quaternion _targetQuaternion;
    void Start()
    {
        _initialQuaterion = transform.rotation;
    }
    void Update()
    {
        transform.rotation = Quaternion.Lerp(_initialQuaterion, _targetQuaternion, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
    }
}
