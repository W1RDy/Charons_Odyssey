using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerShieldState", menuName = "Player's State/ShieldState")]
public class PlayerShieldState : PlayerState
{
    [SerializeField] private float _staminaPerSecond;
    private Shield _shield;
    private StaminaController _staminaController;
    private IInputService _inputService;

    public void Initialize(Player player, IInstantiator instantiator, Shield shield, StaminaController staminaController, IInputService inputService, AudioMaster audioMaster)
    {
        base.Initialize(player, instantiator, audioMaster);
        _shield = shield;
        _staminaController = staminaController;
        _inputService = inputService;
    }

    public override void Enter()
    {
        if (_player.IsEnoughStamina(_staminaPerSecond))
        {
            IsStateFinished = false;
            base.Enter();
            _staminaController.StartUsingStamina(_staminaPerSecond);
            _shield.ActivateShield();

            _player.SetAnimation("Shield", true);
            _audioMaster.PlaySound("UseShield");

            if (_player.transform.localScale.x > 0) _shield.IsTurnedRight = true;
            else _shield.IsTurnedRight = false;
        }
    }

    public override void Update()
    {
        base.Update();

        if (_staminaPerSecond > _player.GetStamina())
        {
            IsStateFinished = true;
            _player.ChangeState(PlayerStateType.Stun);
        }

        if (!_inputService.ButtonIsPushed(InputButtonType.Shield)) IsStateFinished = true;
        var horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        { 
            _player.Flip(new Vector2(horizontalInput, 0));
            if (_player.transform.localScale.x > 0) _shield.IsTurnedRight = true;
            else _shield.IsTurnedRight = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _shield.DeactivateShield();
        _staminaController.StopUsingStamina();

        _player.SetAnimation("Shield", false);
    }
}
