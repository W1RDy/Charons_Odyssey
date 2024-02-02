using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponService : MonoBehaviour, IService
{
    [SerializeField] Weapon[] weapons;
    private Dictionary<WeaponType, Weapon> _weaponsDict;

    public void InitializeService()
    {
        InitializeWeaponsDictionary();
    }

    private void InitializeWeaponsDictionary()
    {
        _weaponsDict = new Dictionary<WeaponType, Weapon>();

        foreach (var weapon in weapons)
        {
            _weaponsDict[weapon.GetWeaponType()] = weapon;
        }
    }

    public Weapon GetWeapon(WeaponType type) => _weaponsDict[type];

    public void InitializeWeapons(Transform weaponPoint, PistolViewConfig pistolView, BulletsCounterIndicator bulletsCounterIndicator)
    {
        foreach (var weapon in _weaponsDict.Values)
        {
            if (weapon.GetWeaponType() == WeaponType.Pistol) (weapon as Pistol).Initialize(weaponPoint, pistolView, bulletsCounterIndicator);
            else weapon.Initialize(weaponPoint);
        }
    }
}
