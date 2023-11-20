using System.Collections;
using UnityEngine;
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyHealthData _enemyHealthData;
    [SerializeField] Enemy _enemy;
    [SerializeField] SpriteRenderer _renderer;
    float _currentHP;
    float _onHitRedTime = .2f;
    GameManager _gameManager;
    private void Start()
    {
        _gameManager = Helpers.GameManager;
        _currentHP = _enemyHealthData.maxHP;
        _enemy.OnReturn += OnReset;
    }
    private void OnDestroy()
    {
        _enemy.OnReturn -= OnReset;
    }
    public virtual void TakeDamage(float dmg)
    {
        _currentHP -= dmg;

        if (_currentHP <= 0)
            Die();
        else
            StartCoroutine(ChangeColor());
    }

    public virtual void Die()
    {
        _gameManager.EnemyManager.RemoveEnemy(_enemy);
        _enemy.ReturnObject();

        _gameManager.EffectsManager.EnemyKilled(transform.position, _enemy.IsRobot);
        _renderer.color = Color.white;
    }
    void OnReset() { _currentHP = _enemyHealthData.maxHP; }
    IEnumerator ChangeColor()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(_onHitRedTime);

        _renderer.color = Color.white;
    }
}
