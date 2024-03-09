using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "Enemy's State/Attack State")]
public class EnemyAttackState : EnemyState
{
    protected EnemyDefault _enemyDefault;
    private Transform _target;

    private CancellationToken _token;
    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;

    public bool IsCooldown { get; private set; }

    public void Initialize(Enemy enemy, PauseService pauseService, Transform target)
    {
        base.Initialize(enemy, pauseService);
        _enemyDefault = enemy as EnemyDefault;
        _token = _enemy.GetCancellationTokenOnDestroy();
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;
        _target = target;
    }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
        var flipDirection = _target.position.x < _enemyDefault.transform.position.x ? Vector2.left : Vector2.right;
        _enemy.Flip(flipDirection);
        _enemy.SetAnimation("Attack", true);
        Attack();
    }

    private async void Attack()
    {
        await WaitUntilAttack();
        _enemy.IsReadyForParrying = false;

        if (_token.IsCancellationRequested || !_enemy.gameObject.activeInHierarchy) return;

        if (!IsStateFinished)
        {
            var player = FinderObjects.FindHittableObjectByCircle(_enemyDefault.HitDistance, _enemyDefault.transform.position, AttackableObjectIndex.Enemy);

            if (player != null && player[0] is IHittableWithShield playerWithShield)
                playerWithShield.TakeHit(_enemyDefault.Damage, new Vector2(_enemyDefault.transform.localScale.x, 0).normalized);
            else if (player != null)
                player[0].TakeHit(_enemyDefault.Damage);
            await Delayer.DelayWithPause(_enemy.DamageTimeBeforeAnimationEnd, _token, _pauseToken);
            if (_token.IsCancellationRequested) return;
            IsStateFinished = true;
        }

        WaitWhileCooldown();
    }

    private async UniTask WaitUntilAttack()
    {
        await UniTask.WaitUntil(() => _enemy.GetAnimationName().EndsWith("Attack"));
        if (_token.IsCancellationRequested) return;
        await Delayer.DelayWithPause((_enemy.GetAnimationDuration() - _enemy.DamageTimeBeforeAnimationEnd) - _enemy.ParryingWindowDuration, _token, _pauseToken);
        _enemy.IsReadyForParrying = true;
        if (_token.IsCancellationRequested) return;
        await Delayer.DelayWithPause(_enemy.ParryingWindowDuration, _token, _pauseToken);
    }

    private async void WaitWhileCooldown()
    {
        IsCooldown = true;
        await Delayer.DelayWithPause(_enemyDefault.AttackCooldown, _token, _pauseToken);
        if (!_token.IsCancellationRequested) IsCooldown = false;
    }

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

    public override void Pause()
    {
        _pauseTokenSource.Pause();
        base.Pause();
    }

    public override void Unpause()
    {
        _pauseTokenSource.Unpause();
        base.Unpause();
    }
}
