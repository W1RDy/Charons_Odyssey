using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData _weaponData;
    public Player player;
    public float Cooldown => _weaponData.Cooldown;
    public float Damage => _weaponData.Damage;
    public WeaponType GetWeaponType() => _weaponData.Type;
    public float Distance => _weaponData.Distance;
}

public abstract class Guns : Weapon
{
    private int _patronsCount;
    public int PatronsCount { 
        get => _patronsCount;
        set { if (value.GetType() == typeof(int)) _patronsCount = value; }
        } 
    [SerializeField] protected Transform shootPoint;
    public GameObject BulletPrefab => (_weaponData as GunsData).Bullet.gameObject;

    private void Awake()
    {
        _patronsCount = (_weaponData as GunsData).PatronsCount;
    }
}

public abstract class ColdWeapon : Weapon
{

}
