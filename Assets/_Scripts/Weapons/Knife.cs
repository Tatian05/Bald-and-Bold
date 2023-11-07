using UnityEngine;
using Weapons;
public class Knife : Weapon
{
    Animator _anim;
    Vector3 _initialScale;
    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<Animator>();
        _initialScale = transform.localScale;
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_MELEE, MeleeConsumable);
    }
    public override void WeaponAction()
    {
        _anim.SetTrigger("Attack");
        Helpers.AudioManager.PlaySFX(_weaponData.weaponSoundName);
    }
    void MeleeConsumable(params object[] param)
    {
        transform.localScale = (bool)param[0] ? _initialScale * (float)param[1] : _initialScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(_weaponData.damage);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_MELEE, MeleeConsumable);
    }
}
