using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerShieldState", menuName = "Player's State/ShieldState")]
public class PlayerShieldState : PlayerState
{
    [SerializeField] private float _staminaPerSecond;
    private Shield _shield;
    private StaminaController _staminaController;
    private IInputService _inputService;

    public void Initialize(Player player, PauseService pauseService, Shield shield, StaminaController staminaController, IInputService inputService)
    {
        base.Initialize(player, pauseService);
        _shield = shield;
        _staminaController = staminaController;
        _inputService = inputService;
    }

    public override void Enter()
    {
        if (_staminaPerSecond <= _player.GetStamina())
        {
            IsStateFinished = false;
            base.Enter();
            _staminaController.StartUsingStamina(_staminaPerSecond);
            _shield.ActivateShield();

            if (_player.transform.localScale.x > 0) _shield.IsTurnedRight = true;
            else _shield.IsTurnedRight = false;
        }
    }

    public override void Update()
    {
        base.Update();

        if (_staminaPerSecond > _player.GetStamina() || !_inputService.ButtonIsPushed(InputButtonType.Shield)) IsStateFinished = true;
        var horizontalInput = Input.GetAxis("Horizontal");
        if ((horizontalInput < 0 && _player.transform.localScale.x > 0) || (horizontalInput > 0 && _player.transform.localScale.x < 0))
        { 
            _player.Flip();
            if (_player.transform.localScale.x > 0) _shield.IsTurnedRight = true;
            else _shield.IsTurnedRight = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _shield.DeactivateShield();
        _staminaController.StartRefillStamina();
    }
}
