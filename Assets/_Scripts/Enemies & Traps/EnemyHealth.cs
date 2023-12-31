using System.Collections;
using UnityEngine;
public class EnemyHealth : MonoBehaviour, IDamageable, IEnemy
{
    [SerializeField] EnemyHealthData _enemyHealthData;
    [SerializeField] Enemy _enemy;
    [SerializeField] SpriteRenderer _renderer;

    float _currentHP;
    float _onHitRedTime = .2f;
    GameManager _gameManager;
    string _withWeaponKilled;
    private void Start()
    {
        _gameManager = Helpers.GameManager;
        _currentHP = _enemyHealthData.maxHP;
        _enemy.OnReturn += OnReset;
    }
    private void OnEnable()
    {
        OnReset();
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

    void OnReset()
    {
        _currentHP = _enemyHealthData.maxHP;
        _renderer.color = Color.white;
    }
    public virtual void Die()
    {
        if (_withWeaponKilled != null) _gameManager.EnemyManager.OnWeaponEnemyDeath(_withWeaponKilled);
        _gameManager.EnemyManager.RemoveEnemy(_enemy);
        _gameManager.EffectsManager.EnemyKilled(transform.position, _enemy.IsRobot);
        _renderer.color = Color.white;

        _enemy.ReturnObject();
    }
    IEnumerator ChangeColor()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(_onHitRedTime);

        _renderer.color = Color.white;
    }

    public void SetWeaponKilled(string weaponName)
    {
        _withWeaponKilled = weaponName;
    }
}
