using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    Vector3 _offset = new Vector3(0, 3, -10);
    void LateUpdate()
    {
        transform.position = _target.position + _offset;
    }
}
