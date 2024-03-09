﻿using UnityEngine;

public abstract class PlayerAttackWithStamina : PlayerAttackBaseState
{
    [SerializeField] protected float _neededStamina;

    public override bool IsStateAvailable()
    {
        return !IsCooldown && _neededStamina <= _player.GetStamina();
    }
}