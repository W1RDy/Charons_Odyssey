using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Heal State", menuName = "Player's State/Heal State")]
public class PlayerHealState : PlayerState
{
    [SerializeField] private int _healValue;
    private Inventory _inventory;

    private PauseToken _pauseToken;

    public virtual void Initialize(Player player, IInstantiator instantiator, Inventory inventory, AudioMaster audioMaster)
    {
        base.Initialize(player, instantiator, audioMaster);
        _inventory = inventory;

        _pauseToken = _pauseHandler.GetPauseToken();
    }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
        _inventory.RemoveItem(ItemType.FirstAidKit);
        _player.TakeHeal(_healValue);

        _player.SetAnimation("Heal", true);
        _audioMaster.PlaySound("Heal");

        WaitWhileHeal();
    }

    public override void Exit()
    {
        base.Exit();
        _player.SetAnimation("Heal", false);
        _audioMaster.StopSound("Heal");
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
