using UnityEngine;
public abstract class Enemy : MonoBehaviour
{
    protected GameManager gameManager;
    protected LayerMask _groundLayer;
    [SerializeField] protected EnemyData _enemyDataSO;
    [SerializeField] protected Animator anim;
    [SerializeField] protected GameObject _signGO;
    [SerializeField] protected Sprite _agroSign, _lostSign;
    [SerializeField] protected Transform _eyes;
    [SerializeField] protected bool _isRobot = false;
    public bool IsRobot { get { return _isRobot; } }
    public event System.Action OnReturn;
    public virtual void Start()
    {
        gameManager = Helpers.GameManager;
        _groundLayer = gameManager.BorderLayer;
        OnReturn += OnReset;
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, ActionOnPlayerDead);
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, PlayerInvisibleConsumable);

        gameManager.EnemyManager.AddEnemy(this);

        if (Helpers.PersistantData.consumablesData.invisible) _enemyDataSO.playerPivot = null;
    }
    protected virtual void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, ActionOnPlayerDead);
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, PlayerInvisibleConsumable);
        OnReturn -= OnReset;
    }
    public virtual void ActionOnPlayerDead(params object[] param) { ReturnObject(); }
    protected abstract void PlayerInvisibleConsumable(params object[] param);
    protected void SetSign(bool enabled, Sprite sign = null)
    {
        if (!_signGO) return;

        _signGO.SetActive(enabled);
        if (enabled) _signGO.GetComponent<SpriteRenderer>().sprite = sign;
    }

    protected Vector3 DistanceToPlayer() => _enemyDataSO.playerPivot.position - _eyes.position;

    public virtual bool CanSeePlayer() => !Physics2D.Raycast(_eyes.position, DistanceToPlayer().normalized, DistanceToPlayer().magnitude, _groundLayer);

    public Enemy SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }
    void OnReset()
    {
        transform.localScale = new Vector3(1, 1, 1);
        if (gameManager) gameManager.EnemyManager.AddEnemy(this);
    }
    public virtual void Reset()
    {
        OnReturn?.Invoke();
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
    }
}
