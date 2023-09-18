using UnityEngine;
public class Trap_Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<GeneralPlayer>().GetComponent<IDamageable>();
        if (player != null)
            player.TakeDamage(1);
    }
}
