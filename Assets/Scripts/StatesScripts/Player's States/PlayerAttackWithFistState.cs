using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack With Fist State", menuName = "Player's State/Attack With Fist State")]
public class PlayerAttackWithFistState : PlayerAttackBaseState
{
    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _weapon = WeaponManager.Instance.GetWeapon(WeaponType.Fist);
    }

    public override void Enter()
    {
        _player.SetAnimation("FistAttack", true);
        base.Enter();
    }

    public override void Attack()
    {
        if (!IsCooldown)
        {
            base.Attack();
            var _hittables = FinderObjects.FindHittableObjectByCircle(_weapon.Distance, _player.weaponPoint.position, AttackableObjectIndex.Player);
            if (_hittables != null) ApplyDamage(_hittables);
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
