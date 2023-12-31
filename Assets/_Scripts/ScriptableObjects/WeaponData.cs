using UnityEngine;
public enum WeaponType { MainWeapon, SecundaryWeapon }
[CreateAssetMenu(menuName = "Weapon Data/New Weapon Data", fileName = "New Weapon Data")]
public class WeaponData : ScriptableObject
{
    //All Weapons
    public WeaponType weaponType;
    public string weaponName;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float damage;
    [HideInInspector] public int initialAmmo;
    [HideInInspector] public string weaponSoundName;
    [HideInInspector] public GameObject bulletExplosion;


    //FireWeapons
    [HideInInspector] public float bulletSpeed;
    [HideInInspector] public float recoilDuration;
    [HideInInspector] public float recoilForce;
    [HideInInspector] public float recoilWeaponRot;
    [HideInInspector] public float recoilWeaponRotDuration;
    [HideInInspector] public float currentCadence;

    //Knifes
    [HideInInspector] public float attackRange;
    [HideInInspector] public float knifeSpeed;

    private void OnEnable()
    {
        currentCadence = fireRate;
    }
}