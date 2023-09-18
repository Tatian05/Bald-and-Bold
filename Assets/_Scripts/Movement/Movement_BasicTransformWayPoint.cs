using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_BasicTransformWayPoint : IMovement
{
    Transform[] _wayPoints;
    Transform _transform;
    float _speed;
    int _index;
    Vector3 _dir;

    bool _bothAxis;

    public Movement_BasicTransformWayPoint(Transform transform, float speed, Transform[] wayPoints, bool bothAxis = false)
    {
        _transform = transform;
        _wayPoints = wayPoints;
        _speed = speed;
        _bothAxis = bothAxis;

        ChangeDir();
    }
    public void Move()
    {
        _transform.position += _dir.normalized * _speed * Time.deltaTime;
        _transform.localRotation *= Quaternion.Euler(0,0, 180);
        if (Vector3.Distance(_transform.position, _wayPoints[_index].position) <= .2f) ChangeDir();
    }

    void ChangeDir()
    {
        _index++;

        if (_index > _wayPoints.Length - 1)
        {
            _index = 0;
        }

        _dir = (_wayPoints[_index].position - _transform.position);
        if(!_bothAxis) _dir.y = 0;
        _dir.z = 0;

        _dir.Normalize();
    }
}
