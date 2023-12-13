using UnityEngine;
public class Enemy_DamageOnTrigger : MonoBehaviour
{
    [SerializeField] float _dmg;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable player))
            player.TakeDamage(_dmg);
    }
}
