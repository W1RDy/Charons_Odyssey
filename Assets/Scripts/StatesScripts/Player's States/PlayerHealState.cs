using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Heal State", menuName = "Player's State/Heal State")]
public class PlayerHealState : PlayerState
{
    private Inventory _inventory;

    public virtual void Initialize(Player player, Inventory inventory)
    {
        base.Initialize(player);
        _inventory = inventory;
    }

    public override void Enter()
    {
        IsStateFinished = false;
        _inventory.RemoveItem(ItemType.FirstAidKit);
        _player.TakeHeal(2);
        _player.SetAnimation("Heal", true);
        WaitWhileHeal();
    }

    public override void Exit()
    {
        _player.SetAnimation("Heal", false);
    }

    private async void WaitWhileHeal()
    {
        var token = _player.GetCancellationTokenOnDestroy();
        await UniTask.WaitUntil(() => _player.GetCurrentAnimationName().EndsWith("Heal"));
        if (token.IsCancellationRequested) return;
        await Delayer.Delay(_player.GetCurrentAnimationDuration(), token);
        if (!token.IsCancellationRequested)
        {
            IsStateFinished = true;
        }
    }
}
