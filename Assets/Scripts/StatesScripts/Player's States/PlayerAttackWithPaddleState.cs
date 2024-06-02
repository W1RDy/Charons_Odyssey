using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Attack With Paddle State", menuName = "Player's State/Attack With Paddle State")]
public class PlayerAttackWithPaddleState : PlayerAttackWithStamina
{
    private Paddle _paddle;

    public override void Initialize(Player player, Weapon weapon, IInstantiator instantiator, AudioMaster audioMaster)
    {
        base.Initialize(player, weapon, instantiator, audioMaster);
        _paddle = _weapon as Paddle;
    }

    public override void Enter()
    {
        _player.SetAnimation("PaddleAttack", true);
        _audioMaster.PlaySound("PaddleHit");

        base.Enter();
    }

    public override void Attack()
    {
        if (!IsCooldown && _player.IsEnoughStamina(_neededStamina))
        {
            base.Attack();
            _player.UseStamina(_neededStamina);
            var hittables = FinderObjects.FindHittableObjectByCircle(_weapon.Distance, _weapon.WeaponPoint.position, AttackableObjectIndex.Player);
            if (hittables != null) ApplyDamage(hittables);
        }
    }

    protected override void ApplyDamage(List<IHittable> hittables)
    {
        foreach (var hittable in hittables)
        {
            var objWithHealth = hittable as IHasHealth;

            var hitInfo = new HitInfo(_weapon.Damage, Vector2.zero, AdditiveHitEffect.Recline);
            hitInfo.SetReclineInfo(new ReclineInfo(_weapon.WeaponPoint, _paddle.RecliningForce));

            objWithHealth?.TakeHit(hitInfo);
        }
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
