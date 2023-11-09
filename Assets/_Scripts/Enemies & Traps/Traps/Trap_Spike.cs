using UnityEngine;
public class Trap_Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IDamageable player))
            player.TakeDamage(1);
    }
}
