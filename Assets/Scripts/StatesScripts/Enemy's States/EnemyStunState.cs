using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun State", menuName = "Enemy's State/Stun State")]
public class EnemyStunState : EnemyState
{
    private PauseTokenSource _pauseTokenSource;
    private EnemyState _interruptedState;

    public override void Initialize(Enemy enemy, PauseService pauseService)
    {
        base.Initialize(enemy, pauseService);
        _pauseTokenSource = new PauseTokenSource();
    }

    public void SetInterruptedState(EnemyState interruptedState)
    {
        _interruptedState = interruptedState;
        Debug.Log(interruptedState.GetStateType());
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
        if (!token.IsCancellationRequested)
        {
            IsStateFinished = true;
            if (_interruptedState) _enemy.ChangeState(_interruptedState.GetStateType());
        }
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.SetAnimation("Stun", false);
    }
}
