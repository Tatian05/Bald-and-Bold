using UnityEngine;
using System;
public class Enemy_DamageOnTrigger : MonoBehaviour
{
    IDamageable _player;
    [SerializeField] float _dmg;

    Collider2D _collider;
    Action _OnUpdate;

    float _colliderTimer;
    float _currentColliderTimer;

    private void Start()
    {
        _player = Helpers.GameManager.Player.GetComponent<IDamageable>();
        _collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        _OnUpdate?.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _player.TakeDamage(_dmg);

        _collider.enabled = false;
        _OnUpdate += ActivateCollider;
    }

    void ActivateCollider()
    {
        _currentColliderTimer += Time.deltaTime;

        if (_currentColliderTimer < _colliderTimer) return;

        _collider.enabled = true;
        _currentColliderTimer = 0;
        _OnUpdate -= ActivateCollider;
    }
}
