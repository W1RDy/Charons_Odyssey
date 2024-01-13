using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Heal State", menuName = "Player's State/Heal State")]
public class PlayerHealState : PlayerState
{
    private Inventory _inventory;

    [Inject]
    private void Construct(Inventory inventory)
    {
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
        await UniTask.WaitUntil(() => _player.GetAnimationName().EndsWith("Heal"));
        if (token.IsCancellationRequested) return;
        await Delayer.Delay(_player.GetAnimationDuration(), token);
        if (!token.IsCancellationRequested)
        {
            IsStateFinished = true;
        }
    }
}
