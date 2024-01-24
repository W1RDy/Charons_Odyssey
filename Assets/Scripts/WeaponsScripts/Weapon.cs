using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData _weaponData;
    public float Cooldown => _weaponData.Cooldown;
    public float Damage => _weaponData.Damage;
    public WeaponType GetWeaponType() => _weaponData.Type;
    public float Distance => _weaponData.Distance;
    public Transform WeaponPoint { get; private set; }

    public virtual void Initialize(Transform weaponPoint)
    {
        WeaponPoint = weaponPoint;
    }
}

public abstract class Guns : Weapon
{
    private int _patronsCount;
    public int PatronsCount { get => _patronsCount; }
    public GameObject BulletPrefab => (_weaponData as GunsData).Bullet.gameObject;
    protected BulletsCounterIndicator _bulletsCounterIndicator;

    private void Start()
    {
        _patronsCount = (_weaponData as GunsData).PatronsCount;
    }

    public void DisablePatrons(int patrons)
    {
        _patronsCount -= patrons;
        if (_patronsCount < 0) _patronsCount = 0;
        _bulletsCounterIndicator.SetCount(_patronsCount);
    }

    public void AddPatrons(int patrons)
    {
        Debug.Log("Add patrons");
        _patronsCount += patrons;
        _bulletsCounterIndicator.SetCount(_patronsCount);
    }
}

public abstract class ColdWeapon : Weapon
{

}
