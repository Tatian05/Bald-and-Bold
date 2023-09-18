using System.Collections;
using UnityEngine;
public class PlayerHealth : MonoBehaviour, IDamageable
{
    GameManager _gameManager;

    [SerializeField] SpriteRenderer[] _allPlayerSprites;
    Vector3 _initialPos;

    bool _isDead = false;

    private void Start()
    {
        _initialPos = transform.position;
        _gameManager = Helpers.GameManager;
       // _gameManager.OnPlayerDead += Die;
    }

    public void TakeDamage(float dmg)
    {
        if (_isDead) return;
        _isDead = true;
        //_gameManager.PlayerDead();

        EventManager.TriggerEvent(Contains.PLAYER_DEAD);
        EventManager.TriggerEvent(Contains.WAIT_PLAYER_DEAD);
    }

    public void Die()
    {

    }

    public void EffectsOnDeath()
    {
        _gameManager.EffectsManager.PlayerKilled(transform.position + Vector3.up);
        Helpers.AudioManager.PlaySFX("PlayerDeath");

        _gameManager.Player.PausePlayer();
        foreach (var item in _allPlayerSprites)            
            item.gameObject.SetActive(false);
    }

    public void RestartPosition(bool pred = false)
    {
        if (pred) return;
        transform.position = _initialPos;
        _gameManager.Player.UnPausePlayer();

        foreach (var item in _allPlayerSprites)
            item.gameObject.SetActive(true);

        StartCoroutine(FixIsDead());
    }

    IEnumerator FixIsDead()
    {
        yield return new WaitForEndOfFrame();
        _isDead = false;
    }
}
