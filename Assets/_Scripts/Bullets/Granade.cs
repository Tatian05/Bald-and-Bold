using UnityEngine;
using System.Linq;
public class Granade : MonoBehaviour
{
    [SerializeField] float _throwForce;
    [SerializeField] float _explosionRadius;
    [SerializeField] float _explosionForce;

    float _gravityScale = 1.5f;
    float _fallGravityMultiplier = 3;
    float _startMass, _startGravityScale;
    public float ThrowForce { get { return _throwForce; } }

    TrailRenderer _trail;
    Rigidbody2D _rb;
    Vector2 _direction;
    float _damage;
    LayerMask _groundLayer;
    Vector3 _initialScale = Vector3.one;
    string _weaponName;
    bool _falling => _rb.velocity.y < 0;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, ReturnGrenade);
        _trail = GetComponent<TrailRenderer>();
        _groundLayer = Helpers.GameManager.GroundLayer;
        _startGravityScale = _rb.gravityScale;
        _startMass = _rb.mass;
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, ReturnGrenade);
    }
    private void Update()
    {
        transform.right = _rb.velocity;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
    private void FixedUpdate()
    {
        _rb.AddForce(_falling ? Vector2.down * (_gravityScale * _fallGravityMultiplier) : Vector2.down * _gravityScale);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            Explosion();
            Helpers.AudioManager.PlaySFX("Grenade_Destroy");
            FRY_GrenadeExplosion.Instance.pool.GetObject().SetPosition(transform.position).SetScale(transform.localScale.x);
            ReturnGrenade();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            Explosion();
            Helpers.AudioManager.PlaySFX("Grenade_Destroy");
            FRY_GrenadeExplosion.Instance.pool.GetObject().SetPosition(transform.position).SetScale(transform.localScale.x);
            ReturnGrenade();
        }
    }

    public void ThrowGranade()
    {
        _rb.AddForce(_direction * _throwForce, ForceMode2D.Impulse);
    }
    void Explosion()
    {
        var collisions = Physics2D.OverlapCircleAll(transform.position, _explosionRadius * transform.localScale.x, Helpers.GameManager.DynamicBodiesLayer).
                                                                                                                                   Where(x => x.GetComponent<IDamageable>() != null && x.GetComponent<Rigidbody2D>() != null && !x.GetComponent<Player>()).
                                                                                                                                   Select(x => x.GetComponent<Rigidbody2D>());
        if (collisions.Count() <= 0) return;
        foreach (var item in collisions)
        {
            if (IsBlocked(item.position, _groundLayer)) continue;
            item.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            if(item.TryGetComponent(out IEnemy enemy)) enemy.SetWeaponKilled(_weaponName);
            if(item.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(_damage);
        }
    }

    bool IsBlocked(Vector3 other, LayerMask ground)
    {
        return Physics2D.Raycast(transform.position, other - transform.position, (other - transform.position).magnitude, ground);
    }

    void ReturnGrenade(params object[] param)
    {
        _trail.Clear();
        FRY_Granades.Instance.ReturnBullet(this);
    }

    #region BUILDER 

    public Granade SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public Granade SetDirection(Vector2 direction)
    {
        _direction = direction;
        return this;
    }
    public Granade SetDamage(float damage)
    {
        _damage = damage;
        return this;
    }

    public Granade SetScale(float scale)
    {
        transform.localScale = _initialScale * scale;
        return this;
    }
    public Granade SetWeaponName(string weaponName)
    {
        _weaponName = weaponName;
        return this;
    }

    #endregion

    #region FACTORY
    private void Reset()
    {
        _rb.velocity = Vector2.zero;
        transform.localScale = _initialScale;
    }
    public static void TurnOn(Granade g)
    {
        g.gameObject.SetActive(true);
    }
    public static void TurnOff(Granade g)
    {
        g.Reset();
        g.gameObject.SetActive(false);
    }

    #endregion
}
