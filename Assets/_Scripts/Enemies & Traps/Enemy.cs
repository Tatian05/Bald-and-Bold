using System.Collections;
using UnityEngine;
public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected GameManager gameManager;
    protected LayerMask _groundLayer;
    [SerializeField] protected EnemyData _enemyDataSO;
    [SerializeField] protected Animator anim;
    [SerializeField] protected GameObject _signGO;
    [SerializeField] protected Sprite _agroSign, _lostSign;
    [SerializeField] protected Transform sprite, _eyes;
    [SerializeField] protected bool _isRobot = false;
    private SpriteRenderer _renderer;
    private float _onHitRedTime = .2f;

    [Header("Health")]
    [SerializeField] float _maxHp = 3;
    float _currentHp;

    public virtual void Start()
    {
        gameManager = Helpers.GameManager;
        _groundLayer = gameManager.BorderLayer;
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, ActionOnPlayerDead);
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, PlayerInvisibleConsumable);
        gameManager.EnemyManager.AddEnemy(this);

        _currentHp = _maxHp;
        _renderer = sprite.GetComponent<SpriteRenderer>();
        if (!_renderer) _renderer = sprite.GetChild(0).GetComponent<SpriteRenderer>();
    }
    protected virtual void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, ActionOnPlayerDead);
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, PlayerInvisibleConsumable);
    }
    public virtual void ActionOnPlayerDead(params object[] param) { ReturnObject(); }
    protected virtual void PlayerInvisibleConsumable(params object[] param)
    {
        _enemyDataSO.playerPivot = (bool)param[0] ? null : gameManager.Player.CenterPivot;
    }
    protected void SetSign(bool enabled, Sprite sign = null)
    {
        _signGO.SetActive(enabled);
        if (enabled) _signGO.GetComponent<SpriteRenderer>().sprite = sign;
    }
    public virtual void TakeDamage(float dmg)
    {
        _currentHp -= dmg;

        if (_currentHp <= 0)
            Die();
        else
            StartCoroutine(ChangeColor());
    }

    public virtual void Die()
    {
        gameManager.EnemyManager.RemoveEnemy(this);

        ReturnObject();
        gameManager.EffectsManager.EnemyKilled(transform.position, _isRobot);
    }

    IEnumerator ChangeColor()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(_onHitRedTime);

        _renderer.color = Color.white;
    }

    protected Vector3 DistanceToPlayer() => _enemyDataSO.playerPivot.position - _eyes.position;

    public virtual bool CanSeePlayer() => !Physics2D.Raycast(_eyes.position, DistanceToPlayer().normalized, DistanceToPlayer().magnitude, _groundLayer);

    protected void ResetHp()
    {
        _currentHp = _maxHp;
    }

    public Enemy SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }
    public virtual void Reset()
    {
        transform.localScale = new Vector3(1, 1, 1);
        _currentHp = _maxHp;
        if (gameManager) gameManager.EnemyManager.AddEnemy(this);
    }

    public static void TurnOn(Enemy b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Enemy b)
    {
        b.gameObject.SetActive(false);
    }
    public virtual void ReturnObject()
    {
        _renderer.color = Color.white;
    }
}
