using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Stun State", menuName = "Player's State/Stun State")]
public class PlayerStunState : PlayerState
{
    private PauseTokenSource _pauseTokenSource;

    public override void Initialize(Player player, IInstantiator instantiator, AudioMaster audioMaster)
    {
        base.Initialize(player, instantiator, audioMaster);
        _pauseTokenSource = new PauseTokenSource();
    }

    public override void Enter()
    {
        Debug.Log("Stun");
        //_player.SetAnimation("Stun", true);
        IsStateFinished = false;
        WaitWhileStunned();
        base.Enter();
    }

    private async void WaitWhileStunned()
    {
        var token = _player.GetCancellationTokenOnDestroy();
        await Delayer.DelayWithPause(2, token, _pauseTokenSource.Token);
        if (!token.IsCancellationRequested) IsStateFinished = true;
    }

    public override void Exit()
    {
        base.Exit();
        //_player.SetAnimation("Stun", false);
    }
}
