using UnityEngine;
public abstract class Bullet : MonoBehaviour
{
    protected float _speed;
    protected float _dmg;
    protected Vector3 _direction;
    protected TrailRenderer _trail;

    public TrailRenderer Trail { get { return _trail ? _trail : GetComponent<TrailRenderer>(); } }

    protected virtual void Start()
    {
        _trail = GetComponent<TrailRenderer>();
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, ReturnBullet);
    }
    protected virtual void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, ReturnBullet);
    }
    protected virtual void ReturnBullet(params object[] param)
    {
        _trail.sortingOrder = -1;
        _trail.Clear();
    }
}
