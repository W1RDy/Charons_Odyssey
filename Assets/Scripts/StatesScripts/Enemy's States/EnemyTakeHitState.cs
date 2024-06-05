using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Take Hit State", menuName = "Enemy's State/Take Hit State")]
public class EnemyTakeHitState : EnemyState
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
        IsStateFinished = false;
        _enemy.SetAnimation("TakeHit", true);
        _ = WaitWhileAnimationFinished();
    }

    private async UniTask WaitWhileAnimationFinished()
    {
        await UniTask.WaitUntil(() => _enemy.GetAnimationName().EndsWith("TakeHit"), cancellationToken: _cancellationToken).SuppressCancellationThrow();
        if (_cancellationToken.IsCancellationRequested || IsStateFinished) return;

        await Delayer.DelayWithPause(_enemy.GetAnimationDuration(), _cancellationToken, _pauseToken);
        if (_cancellationToken.IsCancellationRequested || IsStateFinished) return;
        IsStateFinished = true;
    }
}