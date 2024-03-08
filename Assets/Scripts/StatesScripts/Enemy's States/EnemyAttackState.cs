using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "Enemy's State/Attack State")]
public class EnemyAttackState : EnemyState
{
    protected EnemyDefault _enemyDefault;
    private CancellationToken _token;
    public bool IsCooldown { get; private set; }

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);
        _enemyDefault = enemy as EnemyDefault;
        _token = _enemy.GetCancellationTokenOnDestroy();
    }

    public override void Enter()
    {
        IsStateFinished = false;
        _enemy.SetAnimation("Attack", true);
        Attack();
    }

    private async void Attack()
    {
        await WaitUntilAttack();
        _enemy.IsReadyForParrying = false;
        if (_token.IsCancellationRequested) return;

        if (!IsStateFinished)
        {
            var player = FinderObjects.FindHittableObjectByCircle(_enemyDefault.HitDistance, _enemyDefault.transform.position, AttackableObjectIndex.Enemy);

            if (player != null && player[0] is IHittableWithShield playerWithShield)
                playerWithShield.TakeHit(_enemyDefault.Damage, new Vector2(_enemyDefault.transform.localScale.x, 0).normalized);
            else if (player != null)
                player[0].TakeHit(_enemyDefault.Damage);
            IsStateFinished = true;
        }

        WaitWhileCooldown();
    }

    private async UniTask WaitUntilAttack()
    {
        await UniTask.WaitUntil(() => _enemy.GetAnimationName().EndsWith("Attack"));
        if (_token.IsCancellationRequested) return;
        Debug.Log(_enemy.GetAnimationDuration());
        Debug.Log((_enemy.GetAnimationDuration() - _enemy.DamageTimeBeforeAnimationEnd) - _enemy.ParryingWindowDuration);
        await Delayer.Delay((_enemy.GetAnimationDuration() - _enemy.DamageTimeBeforeAnimationEnd) - _enemy.ParryingWindowDuration, _token);
        _enemy.IsReadyForParrying = true;
        if (_token.IsCancellationRequested) return;
        await Delayer.Delay(_enemy.ParryingWindowDuration, _token);
    }

    private async void WaitWhileCooldown()
    {
        IsCooldown = true;
        await Delayer.Delay(_enemyDefault.AttackCooldown, _token);
        if (!_token.IsCancellationRequested) IsCooldown = false;
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("FinishAttack");
        _enemy.SetAnimation("Attack", false);
        IsStateFinished = true;
    }

    public override bool IsStateAvailable()
    {
        return !IsCooldown;
    }

    public override void ResetValues()
    {
        IsStateFinished = false;
    }
}
