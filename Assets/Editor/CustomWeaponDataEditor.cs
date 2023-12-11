using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(WeaponData))]
public class CustomWeaponDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        WeaponData weaponData = (WeaponData)target;

        EditorGUILayout.LabelField("Fire Rate");
        weaponData.fireRate = EditorGUILayout.FloatField(weaponData.fireRate, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Damage");
        weaponData.damage = EditorGUILayout.FloatField(weaponData.damage, GUILayout.MaxWidth(50));

        weaponData.bulletExplosion = (GameObject)EditorGUILayout.ObjectField("Bullet Explosion", weaponData.bulletExplosion, typeof(GameObject), false);

        EditorGUILayout.LabelField("Weapon Sound Name");
        weaponData.weaponSoundName = EditorGUILayout.TextField(weaponData.weaponSoundName, GUILayout.MaxWidth(100));

        if (weaponData.weaponType == WeaponType.MainWeapon) FireWeapons(weaponData);
        else Knifes(weaponData);

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(weaponData);
    }

    void FireWeapons(WeaponData weaponData)
    {
        EditorGUILayout.LabelField("Bullet Speed");
        weaponData.bulletSpeed = EditorGUILayout.FloatField(weaponData.bulletSpeed, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Max Ammo");
        weaponData.initialAmmo = EditorGUILayout.IntField(weaponData.initialAmmo, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Recoil Duration");
        weaponData.recoilDuration = EditorGUILayout.FloatField(weaponData.recoilDuration, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Recoil Force");
        weaponData.recoilForce = EditorGUILayout.FloatField(weaponData.recoilForce, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Recoil Weapon Rotation");
        weaponData.recoilWeaponRot = EditorGUILayout.FloatField(weaponData.recoilWeaponRot, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Recoil Weapon Rotation Duration");
        weaponData.recoilWeaponRotDuration = EditorGUILayout.FloatField(weaponData.recoilWeaponRotDuration, GUILayout.MaxWidth(50));
    }
    void Knifes(WeaponData weaponData)
    {
        EditorGUILayout.LabelField("Attack Range");
        weaponData.attackRange = EditorGUILayout.FloatField(weaponData.attackRange, GUILayout.MaxWidth(50));

        EditorGUILayout.LabelField("Knife Speed");
        weaponData.knifeSpeed = EditorGUILayout.FloatField(weaponData.knifeSpeed, GUILayout.MaxWidth(50));
    }
}
