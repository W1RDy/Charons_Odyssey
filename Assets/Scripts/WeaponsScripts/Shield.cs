﻿using UnityEngine;

public class Shield : ArmorItem
{
    [SerializeField] private ShieldData _shieldData;
    public bool IsActivated { get; private set; }

    public Shield()
    {
        IsActivated = false;
    }

    public void ActivateShield()
    {
        IsActivated = true;
    }

    public void DeactivateShield()
    {
        IsActivated = false;
    }

    public override void AbsorbDamage(ref float damage)
    {
        if (IsActivated)
        {
            damage -= damage * (_shieldData.AbsorbedDamage / 100);
        }
    }
}