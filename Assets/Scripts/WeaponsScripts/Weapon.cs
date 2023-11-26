using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData _weaponData;
    public Transform weaponPoint;
    public float Cooldown => _weaponData.Cooldown;
    public float Damage => _weaponData.Damage;
    public WeaponType GetWeaponType() => _weaponData.Type;
    public float Distance => _weaponData.Distance;
    protected bool _isCooldown;

    public virtual void Attack()
    {
        WaitCooldown();
    }

    protected virtual void ApplyDamage(List<IHittable> _hittables)
    {
        foreach (var _hittable in _hittables)
        {
            var objWithHealth = _hittable as IHasHealth;
            if (objWithHealth != null) objWithHealth.TakeHit(Damage); 
        }
    }

    private void WaitCooldown()
    {
        _isCooldown = true;
        StartCoroutine(Delayer.DelayCoroutine(Cooldown, () => _isCooldown = false));
    }
}

public abstract class Guns : Weapon
{
    public float PatronsCount => (_weaponData as GunsData).PatronsCount;

    public override void Attack()
    {
        if (!_isCooldown)
        {
            base.Attack();
            Debug.Log("Gun");
            var _hittables = FinderHittableObjects.FindHittableObjectByRay(Distance, weaponPoint.position, AttackableObjectIndex.Player);
            if (_hittables != null) ApplyDamage(_hittables);
        }
    }
}

public abstract class ColdWeapon : Weapon
{
    public override void Attack()
    {
        if (!_isCooldown)
        {
            base.Attack();
            var _hittables = FinderHittableObjects.FindHittableObjectByCircle(Distance, weaponPoint.position, AttackableObjectIndex.Player);
            if (_hittables != null) ApplyDamage(_hittables);
        }
    }
}
