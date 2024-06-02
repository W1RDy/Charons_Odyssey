using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

public abstract class EnemyAttackState : EnemyState
{
    protected Transform _target;

    private CancellationToken _token;
    protected PauseToken _pauseToken;

    public bool IsCooldown { get; private set; }

    public virtual void Initialize(Enemy enemy, IInstantiator instantiator, Transform target)
    {
        base.Initialize(enemy, instantiator);
        _token = _enemy.GetCancellationTokenOnDestroy();

        _pauseToken = _pauseHandler.GetPauseToken();
        _target = target;
    }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;

        var flipDirection = _target.position.x < _enemy.transform.position.x ? Vector2.left : Vector2.right;
        _enemy.Flip(flipDirection);

        AttackWrapper();
    }

    private async void AttackWrapper()
    {
        _enemy.SetAnimation("Attack", true);
        await WaitUntilAttack();
        _enemy.IsReadyForParrying = false;

        if (_token.IsCancellationRequested || !_enemy.gameObject.activeInHierarchy) return;

        if (!IsStateFinished)
        {
            Attack();
            await Delayer.DelayWithPause(_enemy.DamageTimeBeforeAnimationEnd, _token, _pauseToken);
            if (_token.IsCancellationRequested) return;

            IsStateFinished = true;
        }
        WaitWhileCooldown();
    }

    private async UniTask WaitUntilAttack()
    {
        await UniTask.WaitUntil(() => _enemy.GetAnimationName().EndsWith("Attack"), cancellationToken: _token).SuppressCancellationThrow();
        if (_token.IsCancellationRequested) return;

        await Delayer.DelayWithPause((_enemy.GetAnimationDuration() - _enemy.DamageTimeBeforeAnimationEnd) - _enemy.ParryingWindowDuration, _token, _pauseToken);
        if (_token.IsCancellationRequested) return;

        _enemy.IsReadyForParrying = true;
        await Delayer.DelayWithPause(_enemy.ParryingWindowDuration, _token, _pauseToken);
    }

    private async void WaitWhileCooldown()
    {
        IsCooldown = true;
        await Delayer.DelayWithPause(_enemy.AttackCooldown, _token, _pauseToken);
        if (!_token.IsCancellationRequested) IsCooldown = false;
    }

    protected abstract void Attack();

    public override void Exit()
    {
        base.Exit();
        _enemy.SetAnimation("Attack", false);
        IsStateFinished = true;
    }

    public override bool IsStateAvailable()
    {
        return !IsCooldown;
    }
}
