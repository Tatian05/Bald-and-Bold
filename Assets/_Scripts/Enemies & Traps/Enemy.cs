using System.Collections;
using UnityEngine;
using System;
public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected GameManager gameManager;
    protected Transform _playerCenterPivot;
    [SerializeField] protected Animator anim;

    public event Action OnUpdate;

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

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, ActionOnPlayerDead);
        gameManager.EnemyManager.AddEnemy(this);

        _currentHp = _maxHp;
        _renderer = sprite.GetComponent<SpriteRenderer>();
        if (!_renderer) _renderer = sprite.GetChild(0).GetComponent<SpriteRenderer>();

        _playerCenterPivot = gameManager.Player.CenterPivot;
    }
    protected virtual void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, ActionOnPlayerDead);
    }
    public virtual void ActionOnPlayerDead(params object[] param)
    {
        ReturnObject();
    }
    public virtual void Update()
    {
        OnUpdate?.Invoke();
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

    protected Vector3 DistanceToPlayer() => _playerCenterPivot.position - _eyes.position;

    public virtual bool CanSeePlayer() => !Physics2D.Raycast(_eyes.position, DistanceToPlayer().normalized, DistanceToPlayer().magnitude, gameManager.BorderLayer);

    protected void ResetHp()
    {
        _currentHp = _maxHp;
    }

    public Enemy SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }
    public Enemy Flip()
    {
        transform.localScale = new Vector3(1, -1, 1);
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
