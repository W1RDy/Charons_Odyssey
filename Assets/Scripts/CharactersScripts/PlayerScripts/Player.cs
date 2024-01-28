using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerColliderChecker))]
[RequireComponent(typeof(PlayerHpController))]
public class Player : MonoBehaviour, IAttackableWithWeapon, IHasHealableHealth, ITalkable
{
    [SerializeField] private float _hp;
    [SerializeField] private float _speed;

    private PlayerMove _playerMove;
    private PlayerView _playerView;

    private CancellationToken _token;
    private WeaponService _weaponService;
    private BulletsCounterIndicator _bulletsCounterIndicator;
    private PlayerStatesInitializer _playerStatesInitializator;
    private PlayerColliderChecker _playerColliderChecker;
    private PlayerHpController _playerHpController;

    [SerializeField] private PistolViewConfig _pistolView;
    [SerializeField] private Transform _weaponPoint;

    public PistolViewConfig PistolView { get => _pistolView; }
    public Transform WeaponPoint { get => _weaponPoint; }

    #region Player's States

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
    private void Construct(WeaponService weaponService, BulletsCounterIndicator bulletsCounterIndicator)
    {
        _weaponService = weaponService;
        _bulletsCounterIndicator = bulletsCounterIndicator;
    }

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerMove.SetSpeed(_speed);
        _playerView = GetComponentInChildren<PlayerView>();

        StateMachine = new PlayerStateMachine();
        _playerStatesInitializator = GetComponent<PlayerStatesInitializer>();
        _playerColliderChecker = GetComponent<PlayerColliderChecker>();
        _playerHpController = GetComponent<PlayerHpController>();
        _playerHpController.SetMaxHp(_hp);

        _token = this.GetCancellationTokenOnDestroy();
        _playerStatesInitializator.InitializeStatesInstances(_stayState, _moveState, _climbState, _healState, _attackWithFistState, _attackWithPaddleState, _attackWithPistolState, _stayWithGunState, _talkState);
        _weaponService.InitializeWeapons(WeaponPoint, PistolView, _bulletsCounterIndicator); // перенести, если появится больше оружия
    }

    private void Start()
    {
        _playerStatesInitializator.InitializeStates();
        _bulletsCounterIndicator.SetCount((_weaponService.GetWeapon(WeaponType.Pistol) as Pistol).PatronsCount); // перенести, если появится больше оружия
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

    public void Flip() => _playerMove.Flip();

    public bool OnGround() => _playerColliderChecker.IsCollideWithGround();

    public void StartTalk()
    {
        ChangeState(PlayerStateType.Talk);
    }

    public async void Talk(string message)
    {
        await UniTask.WaitUntil(() => StateMachine.CurrentState.GetStateType() == PlayerStateType.Talk, cancellationToken: _token);
        if (!_token.IsCancellationRequested) (StateMachine.CurrentState as PlayerTalkState).Talk(message);
    }

    public string GetTalkableIndex() => "сharon";
}
