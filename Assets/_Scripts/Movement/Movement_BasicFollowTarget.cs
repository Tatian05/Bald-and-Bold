using UnityEngine;
public class Movement_BasicFollowTarget : IMovement
{
    Transform _transform;
    Transform _target;

    Vector3 _offSet = new Vector3(0, .7f);
    float _speed;
    public Movement_BasicFollowTarget(Transform transform, Transform target, float speed)
    {
        _transform = transform;
        _target = target;
        _speed = speed;
    }

    public void Move()
    {
        Vector3 dir = ((_target.position + _offSet) - _transform.position).normalized;

        _transform.position += dir * _speed * Time.deltaTime;
    }
}