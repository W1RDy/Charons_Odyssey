using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerColliderChecker))]
[RequireComponent(typeof(PlayerHpController))]
public class Player : MonoBehaviour, IAttackableWithWeapon, IHasHealableHealth, ITalkable, IPause, IHasStamina
{
    [SerializeField] private float _hp;
    [SerializeField] private float _speed;
    [SerializeField] private float _staminaValue = 100f;

    private PlayerMove _playerMove;
    private PlayerView _playerView;

    private CancellationToken _token;
    private WeaponService _weaponService;
    private BulletsCounterIndicator _bulletsCounterIndicator;
    private PlayerStatesInitializer _playerStatesInitializator;
    private PlayerColliderChecker _playerColliderChecker;
    private PlayerHpController _playerHpController;
    private PlayerStaminaController _playerStamineController;
    private PauseService _pauseService;
    private StaminaRefiller _playerStaminaRefiller;
    private StaminaIndicator _playerStaminaIndicator;

    [SerializeField] private PistolViewConfig _pistolView;
    [SerializeField] private Transform _weaponPoint;
    private bool _isPaused;

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

    #endregion

    public PlayerStateMachine StateMachine { get; set; }

    [Inject]
    private void Construct(WeaponService weaponService, BulletsCounterIndicator bulletsCounterIndicator, PauseService pauseService, StaminaIndicator staminaIndicator)
    {
        _playerStaminaIndicator = staminaIndicator;
        _weaponService = weaponService;
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
        _playerStaminaRefiller = new StaminaRefiller(_staminaValue, this.GetCancellationTokenOnDestroy(), this, _pauseService);

        InitializeControllers();

        _token = this.GetCancellationTokenOnDestroy();
        _playerStatesInitializator.InitializeStatesInstances(_stayState, _moveState, _climbState, _healState, _attackWithFistState, _attackWithPaddleState, _attackWithPistolState, _stayWithGunState, _talkState);
        _weaponService.InitializeWeapons(WeaponPoint, PistolView, _bulletsCounterIndicator); // ���������, ���� �������� ������ ������
    }

    private void InitializeControllers()
    {
        _playerHpController = GetComponent<PlayerHpController>();
        _playerStamineController = new PlayerStaminaController(_staminaValue, _playerStaminaIndicator);
        _playerHpController.SetMaxHp(_hp);
    }

    private void Start()
    {
        _playerStatesInitializator.InitializeStates(_playerStaminaRefiller);
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
        _playerHpController.TakeHeal(healValue, ref _hp);
    }

    public void TakeHit(float damage)
    {
        _playerHpController.TakeHit(damage, ref _hp);
        if (_hp <= 0) Death();
    }

    public void Death()
    {
        _playerHpController.Death();
        _playerStaminaRefiller.Unsubscribe();
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

    public string GetTalkableIndex() => "�haron";

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
        _playerStamineController.UseStamine(value, ref _staminaValue);
    }

    public void RefillStamina(float value)
    {
        _playerStamineController.RefillStamine(value, ref _staminaValue);
    }

    public float GetStamina()
    {
        return _staminaValue;
    }
}
