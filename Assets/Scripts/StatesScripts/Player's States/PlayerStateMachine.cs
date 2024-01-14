using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; set; }
    private Dictionary<PlayerStateType, PlayerState> _statesDictionary;
    private bool _isChanging;

    public void InitializeStatesDictionary(List<PlayerState> states)
    {
        _statesDictionary = new Dictionary<PlayerStateType, PlayerState>();

        foreach (var state in states)
        {
            _statesDictionary[state.GetStateType()] = state;
        }
    }

    public void InitializeStates(Player player, WeaponService weaponService, DialogManager dialogManager, DialogCloudService dialogCloudService, Inventory inventory)
    {
        foreach (var state in _statesDictionary.Values)
        {
            if (state.GetStateType() == PlayerStateType.AttackWithPistol || state.GetStateType() == PlayerStateType.AttackWithPaddle || state.GetStateType() == PlayerStateType.AttackWithFist || state.GetStateType() == PlayerStateType.IdleWithGun)
            {
                Weapon weapon;
                if (state.GetStateType() == PlayerStateType.AttackWithPistol || state.GetStateType() == PlayerStateType.IdleWithGun) weapon = weaponService.GetWeapon(WeaponType.Pistol);
                else if (state.GetStateType() == PlayerStateType.AttackWithPaddle) weapon = weaponService.GetWeapon(WeaponType.Paddle);
                else weapon = weaponService.GetWeapon(WeaponType.Fist);

                if (state.GetStateType() == PlayerStateType.IdleWithGun) (state as PlayerStayWithGunState).Initialize(player, weapon);
                else (state as PlayerAttackBaseState).Initialize(player, weapon);
            }
            else if (state.GetStateType() == PlayerStateType.Talk)
                (state as PlayerTalkState).Initialize(player, dialogManager, dialogCloudService);
            else if (state.GetStateType() == PlayerStateType.Heal)
                (state as PlayerHealState).Initialize(player, inventory);
            else state.Initialize(player);
        }
    }

    public PlayerState GetState(PlayerStateType stateType) => _statesDictionary[stateType];

    public void InitializeCurrentState(PlayerState defaultState)
    {
        CurrentState = defaultState;
        CurrentState.Enter();
    }

    public async void ChangeCurrentState(PlayerState newState) 
    {
        if (CurrentState != newState && (CurrentState.IsStateFinished || CurrentState.GetPriorityIndex() < newState.GetPriorityIndex()) && newState.IsStateAvailable() && !_isChanging)
        {
            _isChanging = true;
            await CurrentState.WaitTransition(newState.GetStateType());
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
            _isChanging = false;
        }
    }
}
