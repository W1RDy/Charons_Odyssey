using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStatesInitializer : MonoBehaviour
{
    private List<PlayerState> _statesInstances;

    private Player _player;
    private WeaponService _weaponService;
    private Inventory _inventory;

    private DialogLifeController _dialogLifeController;
    private DialogCloudService _dialogCloudService;

    private CustomCamera _customCamera;

    private IInputService _inputService;
    private IInstantiator _instantiator;

    private NoiseEventHandler _noiseEventHandler;
    private AudioMaster _audioMaster;

    [Inject]
    private void Construct(Inventory inventory, WeaponService weaponService, DialogLifeController dialogLifeController, DialogCloudService dialogCloudService, CustomCamera customCamera,
        IInstantiator instantiator, IInputService inputService, NoiseEventHandler noiseEventHandler, AudioMaster audioMaster)
    {
        _player = GetComponent<Player>();
        _weaponService = weaponService;
        _inventory = inventory;

        _dialogLifeController = dialogLifeController;
        _dialogCloudService = dialogCloudService;

        _customCamera = customCamera;

        _inputService = inputService;
        _instantiator = instantiator;

        _noiseEventHandler = noiseEventHandler;
        _audioMaster = audioMaster;
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

    public void InitializeStates(StaminaController staminaController, Shield shield)
    {
        foreach (var state in _statesInstances)
        {
            if (state.GetStateType() == PlayerStateType.AttackWithPistol || state.GetStateType() == PlayerStateType.AttackWithPaddle || state.GetStateType() == PlayerStateType.AttackWithFist || state.GetStateType() == PlayerStateType.IdleWithGun)
            {
                Weapon weapon;
                if (state.GetStateType() == PlayerStateType.AttackWithPistol || state.GetStateType() == PlayerStateType.IdleWithGun)
                    weapon = _weaponService.GetWeapon(WeaponType.Pistol);
                else if (state.GetStateType() == PlayerStateType.AttackWithPaddle)
                    weapon = _weaponService.GetWeapon(WeaponType.Paddle);
                else
                    weapon = _weaponService.GetWeapon(WeaponType.Fist);

                if (state.GetStateType() == PlayerStateType.IdleWithGun)
                    (state as PlayerStayWithGunState).Initialize(_player, weapon, _instantiator, _customCamera, _audioMaster);
                else if (state.GetStateType() == PlayerStateType.AttackWithPistol)
                    (state as PlayerAttackWithPistolState).Initialize(_player, weapon, _instantiator, _inventory, _customCamera, _noiseEventHandler, _audioMaster);
                else
                    (state as PlayerAttackWithStamina).Initialize(_player, weapon, _instantiator, _audioMaster);
            }
            else if (state.GetStateType() == PlayerStateType.Talk)
                (state as PlayerTalkState).Initialize(_player, _instantiator, _dialogLifeController, _dialogCloudService, _audioMaster);
            else if (state.GetStateType() == PlayerStateType.Heal)
                (state as PlayerHealState).Initialize(_player, _instantiator, _inventory, _audioMaster);
            else if (state.GetStateType() == PlayerStateType.Shield)
                (state as PlayerShieldState).Initialize(_player, _instantiator, shield, staminaController, _inputService, _audioMaster);
            else state.Initialize(_player, _instantiator, _audioMaster);
        }
    }

    private void Start()
    {
        _player.StateMachine.InitializeCurrentState(_player.StateMachine.GetState(PlayerStateType.Idle));
    }
}
