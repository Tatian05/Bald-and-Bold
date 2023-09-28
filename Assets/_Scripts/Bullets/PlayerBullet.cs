using UnityEngine;
public class PlayerBullet : Bullet
{
    [SerializeField] protected LayerMask _bulletLayer;

    Vector3 _lastPosition, _dir;

    private void Update()
    {
        transform.position += _direction.normalized * _speed * Time.deltaTime;

        _dir = _lastPosition - transform.position;
        var raycast = Physics2D.Raycast(transform.position, _dir, _dir.magnitude, _bulletLayer);

        if (raycast)
        {
            Debug.Log(raycast.collider.name);
            if (raycast.collider.TryGetComponent(out IDamageable enemy)) enemy.TakeDamage(_dmg);
            else Helpers.AudioManager.PlaySFX("Bullet_GroundHit");
            ReturnBullet();
        }

        _lastPosition = transform.position;
    }

    #region BUILDER
    public PlayerBullet SetDirection(Vector2 direction)
    {
        _direction = direction;
        transform.right = _direction;
        return this;
    }
    public PlayerBullet SetPosition(Vector2 position)
    {
        transform.position = position;
        _lastPosition = position;
        return this;
    }
    public PlayerBullet SetDmg(float dmg)
    {
        _dmg = dmg;
        return this;
    }
    public PlayerBullet SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    #endregion
    public static void TurnOn(PlayerBullet b)
    {
        b.Trail.sortingOrder = 1;
        b.gameObject.SetActive(true);
        b._lastPosition = Vector3.zero;
    }

    public static void TurnOff(PlayerBullet b)
    {
        b.gameObject.SetActive(false);
    }
    protected override void ReturnBullet(params object[] param)
    {
        base.ReturnBullet();
        FRY_PlayerBullet.Instance.ReturnBullet(this);
    }
}
