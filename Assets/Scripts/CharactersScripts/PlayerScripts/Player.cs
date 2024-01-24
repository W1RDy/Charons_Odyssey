using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour, IAttackableWithWeapon, IHasHealableHealth, ITalkable // класс перегружен
{
    [SerializeField] private float _hp;
    [SerializeField] private float _speed;
    private float _maxHp;
    private bool _onGround;

    private HpIndicator _hpIndicator;

    private GameService _gameService;
    private PlayerMove _playerMove;
    private PlayerView _playerView;
    private HashSet<Collider2D> _colliders;

    private CancellationToken _token;
    private WeaponService _weaponService;
    private BulletsCounterIndicator _bulletsCounterIndicator;
    private PlayerStatesInitializer _playerStatesInitializator;

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
    private void Construct(HpIndicator hpIndicator, GameService gameService, WeaponService weaponService, BulletsCounterIndicator bulletsCounterIndicator)
    {
        _hpIndicator = hpIndicator;
        _gameService = gameService;
        _weaponService = weaponService;
        _bulletsCounterIndicator = bulletsCounterIndicator;
    }

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerMove.SetSpeed(_speed);
        _playerView = GetComponentInChildren<PlayerView>();
        _maxHp = _hp;

        StateMachine = new PlayerStateMachine();
        _playerStatesInitializator = GetComponent<PlayerStatesInitializer>();

        _colliders = new HashSet<Collider2D>();
        _token = this.GetCancellationTokenOnDestroy();
        _playerStatesInitializator.InitializeStatesInstances(_stayState, _moveState, _climbState, _healState, _attackWithFistState, _attackWithPaddleState, _attackWithPistolState, _stayWithGunState, _talkState);
        _weaponService.InitializeWeapons(WeaponPoint, PistolView, _bulletsCounterIndicator);
    }

    private void Start()
    {
        _playerStatesInitializator.InitializeStates();
        _bulletsCounterIndicator.SetCount((_weaponService.GetWeapon(WeaponType.Pistol) as Pistol).PatronsCount);
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
        _hp += healValue;
        if (_hp > _maxHp) _hp = _maxHp;
        _hpIndicator.SetHp(_hp);
    }

    public void TakeHit(float damage)
    {
        _hp -= damage;
        _hpIndicator.SetHp(_hp);
        if (_hp <= 0) Death();
    }

    public void Death()
    {
        Destroy(gameObject);
        _gameService.LoseGame();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && collision.GetContact(collision.contacts.Length - 1).point.y > collision.collider.bounds.center.y)
        {
            _colliders.Add(collision.collider);
            _onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            RemoveGround(collision.collider);
        }
    }

    public void RemoveGround(Collider2D collider)
    {
        _colliders.Remove(collider);
        if (_colliders.Count == 0) _onGround = false;
    }

    public bool OnGround() => _onGround;

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
