using UnityEngine;
public enum WeaponType { MainWeapon, SecundaryWeapon }
[CreateAssetMenu(menuName = "Weapon Data/New Weapon Data", fileName = "New Weapon Data")]
public class WeaponData : ScriptableObject
{
    //All Weapons
    public WeaponType weaponType;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float recoilDuration;
    [HideInInspector] public float recoilForce;
    [HideInInspector] public float recoilWeaponRot;
    [HideInInspector] public float recoilWeaponRotDuration;
    [HideInInspector] public float damage;
    [HideInInspector] public int initialAmmo;
    [HideInInspector] public Sprite mainSprite;
    [HideInInspector] public Sprite selectedSprite;
    [HideInInspector] public Sprite handWeapon;
    [HideInInspector] public string weaponSoundName;
    [HideInInspector] public GameObject bulletExplosion;


    //FireWeapons
    [HideInInspector] public float bulletSpeed;

    //Knifes
    [HideInInspector] public float attackRange;
    [HideInInspector] public float knifeSpeed;
}