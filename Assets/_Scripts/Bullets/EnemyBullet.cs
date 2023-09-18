using UnityEngine;
public class EnemyBullet : Bullet
{
    private void Update()
    {
        transform.position += _direction.normalized * _speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IDamageable>();
        if (collision)
        {
            if (player != null) player.TakeDamage(_dmg);
            ReturnBullet();
        }
    }

    #region BUILDER
    public EnemyBullet SetDirection(Vector2 direction)
    {
        _direction = direction;
        transform.right = _direction;
        return this;
    }
    public EnemyBullet SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public EnemyBullet SetDmg(float dmg)
    {
        _dmg = dmg;
        return this;
    }
    public EnemyBullet SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    #endregion

    public static void TurnOn(EnemyBullet b)
    {
        b.Trail.sortingOrder = 1;
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(EnemyBullet b)
    {
        b.gameObject.SetActive(false);
    }
    protected override void ReturnBullet(params object[] param)
    {
        base.ReturnBullet();
        FRY_EnemyBullet.Instance.ReturnBullet(this);
    }
}
