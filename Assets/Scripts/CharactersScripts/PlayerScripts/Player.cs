using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerColliderChecker))]
[RequireComponent(typeof(PlayerHpHandler))]
public class Player : MonoBehaviour, IAttackableWithWeapon, IHasHealableHealth, IHittableWithShield, ITalkable, IPause, IHasStamina
{
    [SerializeField] private float _hp;
    [SerializeField] private float _speed;
    [SerializeField] private float _staminaValue = 100f;

    private bool _isImmortal = false;
    public bool IsImmortal { get => _isImmortal; set => _isImmortal = value; }

    private PlayerMove _playerMove;
    private PlayerView _playerView;

    private CancellationToken _token;
    private WeaponService _weaponService;
    private ArmorItemsService _armorItemsService;
    private BulletsCounterIndicator _bulletsCounterIndicator;

    private PlayerStatesInitializer _playerStatesInitializator;
    private PlayerColliderChecker _playerColliderChecker;
    private PlayerHpHandler _playerHpHandler;
    private PlayerStaminaHandler _playerStaminaHandler;

    private PauseService _pauseService;

    private StaminaController _playerStaminaController;
    private StaminaIndicator _playerStaminaIndicator;

    [SerializeField] private PistolViewConfig _pistolView;
    [SerializeField] private Transform _weaponPoint;
    private bool _isPaused;

    private Shield _shield;
    public PistolViewConfig PistolView { get => _pistolView; }
    public Transform WeaponPoint { get => _weaponPoint; }

    #region Player's States

    [Header("Playr's States")]

    [SerializeField] private PlayerStayState _stayState;
    [SerializeField] private PlayerMoveState _moveState;
    [SerializeField] private PlayerClimbState _climbState;
    [SerializeField] private PlayerHealState _healState;
    [SerializeField] private PlayerStayWithGunState _stayWithGunState;
    [SerializeField] private PlayerAttackWithFistState _attackWithFistState;
    [SerializeField] private PlayerAttackWithPaddleState _attackWithPaddleState;
    [SerializeField] private PlayerAttackWithPistolState _attackWithPistolState;
    [SerializeField] private PlayerTalkState _talkState;
    [SerializeField] private PlayerShieldState _shieldState;
    [SerializeField] private PlayerParryingState _parryingState;
    [SerializeField] private PlayerDodgeState _dodgeState;
    [SerializeField] private PlayerStunState _stunState;

    #endregion

    public PlayerStateMachine StateMachine { get; set; }

    [Inject]
    private void Construct(WeaponService weaponService, ArmorItemsService armorItemsService, BulletsCounterIndicator bulletsCounterIndicator, PauseService pauseService, StaminaIndicator staminaIndicator)
    {
        _playerStaminaIndicator = staminaIndicator;
        _weaponService = weaponService;
        _armorItemsService = armorItemsService;
        _bulletsCounterIndicator = bulletsCounterIndicator;
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerMove.SetSpeed(_speed);
        _playerView = GetComponentInChildren<PlayerView>();

        StateMachine = new PlayerStateMachine();
        _playerStatesInitializator = GetComponent<PlayerStatesInitializer>();
        _playerColliderChecker = GetComponent<PlayerColliderChecker>();
        _playerStaminaController = new StaminaController(_staminaValue, this.GetCancellationTokenOnDestroy(), this, _pauseService);

        _shield = _armorItemsService.GetShield();

        InitializeHandlers();

        _token = this.GetCancellationTokenOnDestroy();
        _playerStatesInitializator.InitializeStatesInstances(_stayState, _moveState, _climbState, _healState, _attackWithFistState, _attackWithPaddleState,
            _attackWithPistolState, _stayWithGunState, _talkState, _shieldState, _parryingState, _dodgeState, _stunState);
        
        _weaponService.InitializeWeapons(WeaponPoint, PistolView, _bulletsCounterIndicator); // перенести, если появится больше оружия
    }

    private void InitializeHandlers()
    {
        _playerHpHandler = GetComponent<PlayerHpHandler>();
        _playerStaminaHandler = new PlayerStaminaHandler(_staminaValue, _playerStaminaIndicator);
        _playerHpHandler.Initialize(_hp, _shield);
    }

    private void Start()
    {
        _playerStatesInitializator.InitializeStates(_playerStaminaController, _shield);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
    }

    public void ChangeState(PlayerStateType stateType)
    {
        StateMachine.ChangeCurrentState(StateMachine.GetState(stateType));
    }

    public void Attack(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Fist) ChangeState(PlayerStateType.AttackWithFist);
        else if (weaponType == WeaponType.Pistol) ChangeState(PlayerStateType.AttackWithPistol);
        else if (weaponType == WeaponType.Paddle) ChangeState(PlayerStateType.AttackWithPaddle);
        else throw new TypeAccessException(weaponType + "is incorrect WeaponType!");
    }

    public void TakeHeal(float healValue)
    {
        if (!_isImmortal)
        {
            _playerHpHandler.TakeHeal(healValue, ref _hp);
        }
    }

    public void TakeHit(float damage)
    {
        if (!_isImmortal)
        {
            _playerHpHandler.TakeHit(damage, ref _hp);
        }
        if (_hp <= 0) Death();
    }

    public void TakeHit(float damage, Vector2 damageDirection)
    {
        if (!_isImmortal)
        {
            _playerHpHandler.TakeHit(damage, damageDirection, ref _hp);
        }
        if (_hp <= 0) Death();
    }

    public void Death()
    {
        _playerHpHandler.Death();
        _playerStaminaController.Unsubscribe();
    }

    public void SetAnimation(string animationIndex, bool isActivate)
    {
        _playerView.SetAnimation(animationIndex, isActivate);
    }

    public string GetCurrentAnimationName()
    {
        return _playerView.GetAnimationName();
    }

    public float GetCurrentAnimationDuration()
    {
        return _playerView.GetAnimationDuration();
    }

    public void SetAnimationSpeed(float speed)
    {
        _playerView.SetAnimatorSpeed(speed);
    }

    public void Flip() => _playerMove.Flip();

    public bool OnGround() => _playerColliderChecker.IsCollideWithGround();

    public void StartTalk()
    {
        ChangeState(PlayerStateType.Talk);
    }

    public async void Talk(string message)
    {
        if (!_isPaused)
        {
            await UniTask.WaitUntil(() => StateMachine.CurrentState.GetStateType() == PlayerStateType.Talk, cancellationToken: _token);
            await UniTask.WaitWhile(() => _isPaused, cancellationToken: _token); 
            if (!_token.IsCancellationRequested) (StateMachine.CurrentState as PlayerTalkState).Talk(message);
        }
    }

    public string GetTalkableIndex() => "сharon";

    public void Pause()
    {
        if (!_isPaused) _isPaused = true;
    }

    public void Unpause()
    {
        if (_isPaused) _isPaused = false;
    }

    public void UseStamina(float value)
    {
        _playerStaminaHandler.UseStamina(value, ref _staminaValue);
    }

    public void ChangeStaminaTo(float value)
    {
        _playerStaminaHandler.ChangeStaminaTo(value, ref _staminaValue);
    }

    public void RefillStamina(float value)
    {
        _playerStaminaHandler.RefillStamina(value, ref _staminaValue);
    }

    public float GetStamina()
    {
        return _staminaValue;
    }
}
