using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_BasicRigidBodyWaypoint : IMovement
{
    Transform[] _wayPoints;
    Transform _transform;
    float _speed;
    int _index;
    float _dir;
    Rigidbody2D _rb;

    public Movement_BasicRigidBodyWaypoint(Rigidbody2D rb, Transform transform, float speed, Transform[] wayPoints)
    {
        _transform = transform;
        _wayPoints = wayPoints;
        _speed = speed;
        _rb = rb;

        ChangeDir();
    }

    public void Move()
    {
        _rb.velocity = new Vector2(_dir * _speed * Time.fixedDeltaTime, _rb.velocity.y);

        if (Vector3.Distance(_transform.position, _wayPoints[_index].position) <= .2f) ChangeDir();
    }

    void ChangeDir()
    {
        _index++;

        if (_index > _wayPoints.Length - 1)
        {
            _index = 0;
        }

        float newDir = (_wayPoints[_index].position.x - _transform.position.x);

        if (newDir > 0) _dir = 1;
        else _dir = -1;
    }
}
