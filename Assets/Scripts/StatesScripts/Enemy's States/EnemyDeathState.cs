using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Death State", menuName = "Enemy's State/Death State")]
public class EnemyDeathState : EnemyState
{
    private PauseToken _pauseToken;
    private CancellationToken _cancellationToken;

    public override void Initialize(Enemy enemy, IInstantiator instantiator)
    {
        base.Initialize(enemy, instantiator);

        _pauseToken = _pauseHandler.GetPauseToken();
        _cancellationToken = _enemy.GetCancellationTokenOnDestroy();
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.SetAnimation("Death", true);
        _ = WaitWhileAnimationFinished();
    }

    private async UniTask WaitWhileAnimationFinished()
    {
        await UniTask.WaitUntil(() => _enemy.GetAnimationName().EndsWith("Death"), cancellationToken: _cancellationToken).SuppressCancellationThrow();
        if (_cancellationToken.IsCancellationRequested) return;
        await Delayer.DelayWithPause(_enemy.GetAnimationDuration(), _cancellationToken, _pauseToken);
        if (_cancellationToken.IsCancellationRequested) return;
        Exit();
    }

    public override void Exit() 
    {
        base.Exit();
        _enemy.Death();
    }
}