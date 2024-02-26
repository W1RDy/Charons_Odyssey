using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Heal State", menuName = "Player's State/Heal State")]
public class PlayerHealState : PlayerState
{
    private Inventory _inventory;
    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;

    public virtual void Initialize(Player player, PauseService pauseService, Inventory inventory)
    {
        base.Initialize(player, pauseService);
        _inventory = inventory;
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;
    }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
        _inventory.RemoveItem(ItemType.FirstAidKit);
        _player.TakeHeal(2);
        _player.SetAnimation("Heal", true);
        WaitWhileHeal();
    }

    public override void Exit()
    {
        base.Exit();
        _player.SetAnimation("Heal", false);
    }

    private async void WaitWhileHeal()
    {
        var token = _player.GetCancellationTokenOnDestroy();
        await UniTask.WaitUntil(() => _player.GetCurrentAnimationName().EndsWith("Heal"));
        if (token.IsCancellationRequested) return;
        await Delayer.DelayWithPause(_player.GetCurrentAnimationDuration(), token, _pauseToken);
        if (!token.IsCancellationRequested)
        {
            IsStateFinished = true;
        }
    }
}
