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
    private bool _isPushedAndUp;

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
        }
    }

    public override void Update()
    {
        base.Update();
        if (!_isPushedAndUp)
        {
            _isPushedAndUp = !_inputService.ButtonIsPushed(InputButtonType.Shield);
        }

        if (_staminaPerSecond > _player.GetStamina() || (_inputService.ButtonIsPushed(InputButtonType.Shield) && _isPushedAndUp)) IsStateFinished = true;
        var horizontalInput = Input.GetAxis("Horizontal");
        if ((horizontalInput < 0 && _player.transform.localScale.x > 0) || (horizontalInput > 0 && _player.transform.localScale.x < 0)) _player.Flip();
    }

    public override void Exit()
    {
        base.Exit();
        _shield.DeactivateShield();
        _staminaController.StartRefillStamina();
    }

    public override void ResetValues()
    {
        base.ResetValues();
        _isPushedAndUp = false;
    }
}
