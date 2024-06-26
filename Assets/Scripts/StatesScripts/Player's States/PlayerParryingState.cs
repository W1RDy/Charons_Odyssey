﻿using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerParryingState", menuName = "Player's State/ParryingState")]
public class PlayerParryingState : PlayerState
{
    [SerializeField] private float _parryingDistance;
    [SerializeField] private int _neededStamina;
    [SerializeField] private float _cooldown;
    private IParryingHittable _parryingHittable;

    private CancellationToken _token;
    private PauseTokenSource _pauseTokenSource;

    private bool _isCooldown;

    public override void Initialize(Player player, IInstantiator instantiator, AudioMaster audioMaster)
    {
        base.Initialize(player, instantiator, audioMaster);
        _token = player.GetCancellationTokenOnDestroy();
        _pauseTokenSource = new PauseTokenSource();
    }

    public override void Enter()
    {
        IsStateFinished = false;
        _isCooldown = true;
        _player.UseStamina(_neededStamina);

        _parryingHittable.IsReadyForParrying = false;
        _parryingHittable.ApplyParrying();
        IsStateFinished = true;

        _player.SetAnimation("Parrying", true);
        _audioMaster.PlaySound("UseShield");

        WaitCooldown();
        base.Enter();
    }

    private async void WaitCooldown()
    {
        await Delayer.DelayWithPause(_cooldown, _token, _pauseTokenSource.Token);
        if (!_token.IsCancellationRequested) _isCooldown = false;
    }

    public override bool IsStateAvailable()
    {
        _parryingHittable = FinderObjects.FindParryingHittableByCircle(_parryingDistance, _player.transform.position);
        return _parryingHittable != null && !_isCooldown && _player.IsEnoughStamina(_neededStamina);
    }
}