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
    public GameObject BulletPrefab => (_weaponData as GunsData).Bullet.gameObject;
    protected BulletsCounterIndicator _bulletsCounterIndicator;
    public int PatronsCount
    {
        get
        {
            return _patronsCount;
        }
        set
        {
            if (value.GetType() == typeof(int) && value > 100)
            {
                _patronsCount = (_weaponData as GunsData).PatronsCount;
                _bulletsCounterIndicator.SetCount(_patronsCount);

            }
            else if (value.GetType() == typeof(int) && value >= 0)
            {
                _patronsCount = value;
                _bulletsCounterIndicator.SetCount(_patronsCount);
            }
        }
    }
}

public abstract class ColdWeapon : Weapon
{

}
