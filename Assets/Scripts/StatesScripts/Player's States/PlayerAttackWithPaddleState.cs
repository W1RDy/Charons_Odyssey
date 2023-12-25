using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack With Paddle State", menuName = "Player's State/Attack With Paddle State")]
public class PlayerAttackWithPaddleState : PlayerAttackBaseState
{
    private Paddle _paddle;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _weapon = WeaponManager.Instance.GetWeapon(WeaponType.Paddle);
        _paddle = _weapon as Paddle;
    }

    public override void Enter()
    {
        _player.SetAnimation("PaddleAttack", true);
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

    protected override void ApplyDamage(List<IHittable> _hittables)
    {
        foreach (var _hittable in _hittables)
        {
            var _objWithHealth = _hittable as IHasHealth;
            var _reclinable = _hittable as IReclinable;
            if (_objWithHealth != null) _objWithHealth.TakeHit(_paddle.Damage);
            if (_reclinable != null) ReclineObjs(_reclinable);
        }
    }

    private void ReclineObjs(IReclinable _reclinable)
    {
        _reclinable.GetRecline(_player.weaponPoint, _paddle.RecliningForce);
    }

    public override void Exit()
    {
        if (IsStateFinished)
        {
            base.Exit();
            _player.SetAnimation("PaddleAttack", false);
        }
    }
}
