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
    private PauseService _pauseService;
    private IInputService _inputService;

    [Inject]
    private void Construct(Inventory inventory, WeaponService weaponService, DialogLifeController dialogLifeController,
        DialogCloudService dialogCloudService, CustomCamera customCamera, PauseService pauseService, IInputService inputService)
    {
        _weaponService = weaponService;
        _dialogLifeController = dialogLifeController;
        _dialogCloudService = dialogCloudService;
        _inventory = inventory;
        _customCamera = customCamera;
        _pauseService = pauseService;
        _inputService = inputService;
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

    public void InitializeStates(StaminaController staminaController, Shield shield, float parryingDistance)
    {
        foreach (var state in _statesInstances)
        {
            if (state.GetStateType() == PlayerStateType.AttackWithPistol || state.GetStateType() == PlayerStateType.AttackWithPaddle || state.GetStateType() == PlayerStateType.AttackWithFist || state.GetStateType() == PlayerStateType.IdleWithGun)
            {
                Weapon weapon;
                if (state.GetStateType() == PlayerStateType.AttackWithPistol || state.GetStateType() == PlayerStateType.IdleWithGun) weapon = _weaponService.GetWeapon(WeaponType.Pistol);
                else if (state.GetStateType() == PlayerStateType.AttackWithPaddle) weapon = _weaponService.GetWeapon(WeaponType.Paddle);
                else weapon = _weaponService.GetWeapon(WeaponType.Fist);

                if (state.GetStateType() == PlayerStateType.IdleWithGun) (state as PlayerStayWithGunState).Initialize(_player, weapon, _pauseService, _customCamera);
                else if (state.GetStateType() == PlayerStateType.AttackWithPistol) (state as PlayerAttackWithPistolState).Initialize(_player, weapon, _pauseService, _inventory, _customCamera);
                else (state as PlayerAttackWithStamina).Initialize(_player, weapon, _pauseService, staminaController);
            }
            else if (state.GetStateType() == PlayerStateType.Talk)
                (state as PlayerTalkState).Initialize(_player, _pauseService, _dialogLifeController, _dialogCloudService);
            else if (state.GetStateType() == PlayerStateType.Heal)
                (state as PlayerHealState).Initialize(_player, _pauseService, _inventory);
            else if (state.GetStateType() == PlayerStateType.Shield)
                (state as PlayerShieldState).Initialize(_player, _pauseService, shield, staminaController, _inputService);
            else if (state.GetStateType() == PlayerStateType.Parrying)
                (state as PlayerParryingState).Initialize(_player, _pauseService, parryingDistance);
            else state.Initialize(_player, _pauseService);
        }
    }

    private void Start()
    {
        _player.StateMachine.InitializeCurrentState(_player.StateMachine.GetState(PlayerStateType.Idle));
    }
}
