using UnityEngine;
public class Dummy : MonoBehaviour, IDamageable
{
    public void Die()
    {
        EventManager.TriggerEvent(Contains.DUMMY_SPAWN);
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        Die();
    }
}
