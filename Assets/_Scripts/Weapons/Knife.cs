using UnityEngine;
using Weapons;
public class Knife : Weapon
{
    Animator _anim;
    Vector3 _initialScale;
    float _knifeBoost;
    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<Animator>();
        _initialScale = transform.localScale;
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_MELEE, MeleeConsumable);

        _knifeBoost = Helpers.PersistantData.consumablesData.knifeBoost;
        transform.localScale = _initialScale * _knifeBoost;
    }
    public override void WeaponAction()
    {
        _anim.SetTrigger("Attack");
        Helpers.AudioManager.PlaySFX(_weaponData.weaponSoundName);
    }
    public override void WeaponAction(float bulletScale, float cadenceBoost, bool recoil) { }
    void MeleeConsumable(params object[] param)
    {
        Helpers.PersistantData.consumablesData.knifeBoost = _knifeBoost = (bool)param[0] ? (float)param[1] : 1;
        transform.localScale = _initialScale * _knifeBoost;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IEnemy enemy)) enemy.SetWeaponKilled(_weaponData.weaponName);
        if (collision.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(_weaponData.damage);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_MELEE, MeleeConsumable);
    }

}
