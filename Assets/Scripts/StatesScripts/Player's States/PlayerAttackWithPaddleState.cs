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
            var hittables = FinderObjects.FindHittableObjectByCircle(_weapon.Distance, _player.weaponPoint.position, AttackableObjectIndex.Player);
            if (hittables != null) ApplyDamage(hittables);
        }
    }

    protected override void ApplyDamage(List<IHittable> hittables)
    {
        foreach (var hittable in hittables)
        {
            var objWithHealth = hittable as IHasHealth;
            var reclinable = hittable as IReclinable;
            if (objWithHealth != null) objWithHealth.TakeHit(_paddle.Damage);
            if (reclinable != null) ReclineObjs(reclinable);
        }
    }

    private void ReclineObjs(IReclinable reclinable)
    {
        reclinable.GetRecline(_player.weaponPoint, _paddle.RecliningForce);
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
