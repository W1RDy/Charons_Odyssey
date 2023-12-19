﻿using UnityEngine;
using System.Collections.Generic;

public class Paddle : ColdWeapon
{
    public float RecliningForce => (_weaponData as PaddleData).RecliningForce;

    public override void Attack()
    {
        if (!_isCooldown)
        {
            Debug.Log("Paddle");
            player.SetAttackAnimation(WeaponType.Paddle);
        }
        base.Attack();
    }

    protected override void ApplyDamage(List<IHittable> _hittables)
    {
        foreach (var _hittable in _hittables)
        {
            var _objWithHealth = _hittable as IHasHealth;
            var _reclinable = _hittable as IReclinable;
            if (_objWithHealth != null) _objWithHealth.TakeHit(Damage);
            if (_reclinable != null) ReclineObjs(_reclinable);
        }
    }

    private void ReclineObjs(IReclinable _reclinable)
    {
        _reclinable.GetRecline(player.weaponPoint, RecliningForce);
    }
}
