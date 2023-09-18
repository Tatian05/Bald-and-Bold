using UnityEngine;
public class PlayerBullet : Bullet
{
    [SerializeField] protected LayerMask _bulletLayer;

    Vector3 _lastPosition;
    private void Update()
    {
        transform.position += _direction.normalized * _speed * Time.deltaTime;

        Vector3 direction = _lastPosition - transform.position;
        var raycast = Physics2D.Raycast(transform.position, direction, direction.magnitude, _bulletLayer);

        if (raycast)
        {
            var enemy = raycast.collider.GetComponent<IDamageable>();
            if (enemy != null) enemy.TakeDamage(_dmg);
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
