using System.Collections.Generic;
using UnityEngine;
using PickUps;
using Weapons;
public class DropManager : MonoBehaviour
{
    [Header("PickUps")]
    [SerializeField] PickUp[] _pickUpsCollection;
    [SerializeField] Weapon[] _weaponsCollection;
    Dictionary<PickUp, int> _pickUps = new Dictionary<PickUp, int>();
    Dictionary<Weapon, int> _weapons = new Dictionary<Weapon, int>();
    void Start()
    {
        _pickUps.Add(_pickUpsCollection[0], 50);

        _weapons.Add(_weaponsCollection[0], 25);
        _weapons.Add(_weaponsCollection[1], 25);
        _weapons.Add(_weaponsCollection[2], 25);
        _weapons.Add(_weaponsCollection[3], 25);
    }
    public PickUp GetPickUpDrop() => RWS(_pickUps);
    public Weapon GetWeaponDrop() => RWS(_weapons);
    T RWS<T>(Dictionary<T, int> values)
    {
        float sum = 0;
        foreach (var item in values)
            sum += item.Value;

        float random = Random.Range(0f, 1f);
        float count = 0;
        foreach (var item in values)
        {
            count += item.Value / sum;
            if (count >= random)
                return item.Key;
        }

        return default;
    }
}
