using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack With Fist State", menuName = "Player's State/Attack With Fist State")]
public class PlayerAttackWithFistState : PlayerAttackWithStamina
{
    public override void Enter()
    {
        _player.SetAnimation("FistAttack", true);
        base.Enter();
    }

    public override void Attack()
    {
        if (!IsCooldown && _player.GetStamina() >= _neededStamina)
        {
            base.Attack();
            _player.UseStamina(_neededStamina);
            var hittables = FinderObjects.FindHittableObjectByCircle(_weapon.Distance, _weapon.WeaponPoint.position, AttackableObjectIndex.Player);
            if (hittables != null) ApplyDamage(hittables);
        }
    }

    public override void Exit()
    {
        if (IsStateFinished)
        {
            base.Exit();
            _player.SetAnimation("FistAttack", false);
        }
    }
}
