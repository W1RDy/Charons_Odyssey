using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun State", menuName = "Enemy's State/Stun State")]
public class EnemyStunState : EnemyState
{
    private PauseTokenSource _pauseTokenSource;

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);
        _pauseTokenSource = new PauseTokenSource();
    }

    public override void Enter()
    {
        Debug.Log("Stun");
        _enemy.SetAnimation("Stun", true);
        IsStateFinished = false;
        WaitWhileStunned();
        base.Enter();
    }

    private async void WaitWhileStunned()
    {
        var token = _enemy.GetCancellationTokenOnDestroy();
        await Delayer.DelayWithPause(1, token, _pauseTokenSource.Token);
        if (!token.IsCancellationRequested) IsStateFinished = true;
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.SetAnimation("Stun", false);
    }
}
