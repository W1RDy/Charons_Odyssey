using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public abstract class PlayerAttackBaseState : PlayerState
{
    protected Weapon _weapon;
    public bool IsCooldown { get; protected set; }
    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
        Attack();
        WaitWhileAttack();
    }

    public virtual void Initialize(Player player, Weapon weapon, IInstantiator instantiator, AudioMaster audioMaster)
    {
        base.Initialize(player, instantiator, audioMaster);
        _weapon = weapon;
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;
    }

    public virtual void Attack()
    {
        
    }

    protected virtual void ApplyDamage(List<IHittable> hittables)
    {
        foreach (var hittable in hittables)
        {
            var objWithHealth = hittable as IHasHealth;
            objWithHealth?.TakeHit(_weapon.Damage);
        }
    }

    private async void WaitCooldown(CancellationToken token)
    {
        IsCooldown = true;
        await Delayer.DelayWithPause(_weapon.Cooldown, token, _pauseToken);
        if (!token.IsCancellationRequested) IsCooldown = false;
    }

    private async void WaitWhileAttack()
    {
        var token = _player.GetCancellationTokenOnDestroy();
        await UniTask.WaitUntil(() => _player.GetCurrentAnimationName().EndsWith("Hit") || _player.GetCurrentAnimationName().EndsWith("Shot"));
        if (token.IsCancellationRequested) return;
        await Delayer.DelayWithPause(_player.GetCurrentAnimationDuration(), token, _pauseToken);
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

    public override void Pause()
    {
        base.Pause();
        _pauseTokenSource.Pause();
    }

    public override void Unpause()
    {
        base.Unpause();
        _pauseTokenSource.Unpause();
    }
}
