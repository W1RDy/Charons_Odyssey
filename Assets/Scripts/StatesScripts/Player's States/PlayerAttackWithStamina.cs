using UnityEngine;

public abstract class PlayerAttackWithStamina : PlayerAttackBaseState
{
    [SerializeField] protected float _neededStamina;
    private StaminaRefiller _staminaRefiller;

    public virtual void Initialize(Player player, Weapon weapon, PauseService pauseService, StaminaRefiller staminaRefiller)
    {
        base.Initialize(player, weapon, pauseService);
        _staminaRefiller = staminaRefiller;
    }

    public override void Attack()
    {
        _staminaRefiller.StopRefillStamine();
        base.Attack();
    }

    public override void Exit()
    {
        _staminaRefiller.StartRefillStamine();
        base.Exit();
    }

    public override bool IsStateAvailable()
    {
        return !IsCooldown && _neededStamina <= _player.GetStamina();
    }
}