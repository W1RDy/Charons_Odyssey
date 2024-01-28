using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStatesInitializer : MonoBehaviour
{
    private Player _player;
    private WeaponService _weaponService;
    private DialogLifeController _dialogLifeController;
    private DialogCloudService _dialogCloudService;
    private Inventory _inventory;
    private List<PlayerState> _statesInstances;
    private CustomCamera _customCamera;

    [Inject]
    private void Construct(Inventory inventory, WeaponService weaponService, DialogLifeController dialogLifeController, DialogCloudService dialogCloudService, CustomCamera customCamera)
    {
        _weaponService = weaponService;
        _dialogLifeController = dialogLifeController;
        _dialogCloudService = dialogCloudService;
        _inventory = inventory;
        _customCamera = customCamera;
        _player = GetComponent<Player>();
    }

    public void InitializeStatesInstances(params PlayerState[] playerStates)
    {
        _statesInstances = new List<PlayerState>();
        foreach (var playerState in playerStates)
        {
            _statesInstances.Add(Instantiate(playerState));
        }
        _player.StateMachine.InitializeStatesDictionary(_statesInstances);
    }

    public void InitializeStates()
    {
        foreach (var state in _statesInstances)
        {
            if (state.GetStateType() == PlayerStateType.AttackWithPistol || state.GetStateType() == PlayerStateType.AttackWithPaddle || state.GetStateType() == PlayerStateType.AttackWithFist || state.GetStateType() == PlayerStateType.IdleWithGun)
            {
                Weapon weapon;
                if (state.GetStateType() == PlayerStateType.AttackWithPistol || state.GetStateType() == PlayerStateType.IdleWithGun) weapon = _weaponService.GetWeapon(WeaponType.Pistol);
                else if (state.GetStateType() == PlayerStateType.AttackWithPaddle) weapon = _weaponService.GetWeapon(WeaponType.Paddle);
                else weapon = _weaponService.GetWeapon(WeaponType.Fist);

                if (state.GetStateType() == PlayerStateType.IdleWithGun) (state as PlayerStayWithGunState).Initialize(_player, weapon, _customCamera);
                else if (state.GetStateType() == PlayerStateType.AttackWithPistol) (state as PlayerAttackWithPistolState).Initialize(_player, weapon, _customCamera);
                else (state as PlayerAttackBaseState).Initialize(_player, weapon);
            }
            else if (state.GetStateType() == PlayerStateType.Talk)
                (state as PlayerTalkState).Initialize(_player, _dialogLifeController, _dialogCloudService);
            else if (state.GetStateType() == PlayerStateType.Heal)
                (state as PlayerHealState).Initialize(_player, _inventory);
            else state.Initialize(_player);
        }
    }

    private void Start()
    {
        _player.StateMachine.InitializeCurrentState(_player.StateMachine.GetState(PlayerStateType.Idle));
    }
}
