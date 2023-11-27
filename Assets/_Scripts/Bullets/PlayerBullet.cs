using UnityEngine;
public class PlayerBullet : Bullet
{
    [SerializeField] protected LayerMask _bulletLayer;

    Vector3 _lastPosition, _dir, _initialScale = Vector3.one;
    float _checkRadius;
    string _weaponName;
    protected override void Start()
    {
        base.Start();
        _checkRadius = transform.localScale.x * .12f;
    }
    private void Update()
    {
        transform.position += _direction.normalized * _speed * CustomTime.DeltaTime;

        _dir = _lastPosition - transform.position;
        var raycast = Physics2D.CircleCast(transform.position, _checkRadius, _dir, _dir.magnitude, _bulletLayer);

        if (raycast)
        {
            if (raycast.collider.TryGetComponent(out IEnemy enemy)) enemy.SetWeaponKilled(_weaponName);
            if (raycast.collider.TryGetComponent(out IDamageable damageable))damageable.TakeDamage(_dmg);

            else Helpers.AudioManager.PlaySFX("Bullet_GroundHit");
            ReturnBullet();
        }

        _lastPosition = transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
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
    public PlayerBullet SetScale(float scale)
    {
        transform.localScale = _initialScale * scale;
        _checkRadius = transform.localScale.x * .12f;
        return this;
    }
    public PlayerBullet SetWeaponName(string weaponName)
    {
        _weaponName = weaponName;
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
        transform.localScale = _initialScale;
        base.ReturnBullet();
        FRY_PlayerBullet.Instance.ReturnBullet(this);
    }
}
