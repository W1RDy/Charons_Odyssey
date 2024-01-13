using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class PlayerAttackBaseState : PlayerState
{
    protected Weapon _weapon;
    public bool IsCooldown { get; protected set; }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
        Attack();
        WaitWhileAttack();
    }

    public virtual void Attack()
    {
        
    }

    protected virtual void ApplyDamage(List<IHittable> hittables)
    {
        foreach (var hittable in hittables)
        {
            var objWithHealth = hittable as IHasHealth;
            if (objWithHealth != null) objWithHealth.TakeHit(_weapon.Damage);
        }
    }

    private async void WaitCooldown(CancellationToken token)
    {
        IsCooldown = true;
        await Delayer.Delay(_weapon.Cooldown, token);
        if (!token.IsCancellationRequested) IsCooldown = false;
    }

    private async void WaitWhileAttack()
    {
        var token = _player.GetCancellationTokenOnDestroy();
        await UniTask.WaitUntil(() => _player.GetAnimationName().EndsWith("Hit") || _player.GetAnimationName().EndsWith("Shot"));
        if (token.IsCancellationRequested) return;
        await Delayer.Delay(_player.GetAnimationDuration(), token);
        if (!token.IsCancellationRequested)
        {
            IsStateFinished = true;
            WaitCooldown(token);
        }
    }

    public override bool IsStateAvailable()
    {
        return !IsCooldown;
    }
}
