using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon[] _weapons;
    private Dictionary<WeaponType, Weapon> _weaponsDict;

    public static WeaponManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeWeaponsDictionary();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    private void InitializeWeaponsDictionary()
    {
        _weaponsDict = new Dictionary<WeaponType, Weapon>();

        foreach(var _weapon in _weapons) _weaponsDict[_weapon.GetWeaponType()] = _weapon;
    }

    public Weapon GetWeapon(WeaponType type) => _weaponsDict[type];
}
