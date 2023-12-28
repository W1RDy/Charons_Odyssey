using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour, IAttackableWithWeapon, IHasHealableHealth, ITalkable // класс перегружен
{
    [SerializeField] private float _hp;
    [SerializeField] private float _speed;
    [SerializeField] private HpIndicator _hpIndicator;
    [SerializeField] private GameService _gameService;
    public BulletsCounterIndicator bulletsCounterIndicator;
    public Transform weaponPoint;
    private PlayerMove _playerMove;
    private Animator _animator;
    private float _maxHp;
    public Transform weaponView;
    public Transform weaponEnd;
    private HashSet<Collider2D> _colliders;
    private bool _onGround;
    public DialogCloudService dialogCloudService;
    private CancellationToken _token;

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

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerMove.SetSpeed(_speed);
        _animator = GetComponentInChildren<Animator>();
        _maxHp = _hp;

        StateMachine = new PlayerStateMachine();
        
        _colliders = new HashSet<Collider2D>();
        _token = this.GetCancellationTokenOnDestroy();
        InitializeStatesInstances();
    }

    private void InitializeStatesInstances()
    {
        List<PlayerState> _statesInstances = new List<PlayerState>()
        {
            Instantiate(_stayState),
            Instantiate(_moveState),
            Instantiate(_climbState),
            Instantiate(_healState),
            Instantiate(_attackWithFistState),
            Instantiate(_attackWithPaddleState),
            Instantiate(_attackWithPistolState),
            Instantiate(_stayWithGunState),
            Instantiate(_talkState)
        };

        StateMachine.InitializeStatesDictionary(_statesInstances);
    }

    private void Start()
    {
        StateMachine.InitializeStates(this);
        StateMachine.InitializeCurrentState(StateMachine.GetState(PlayerStateType.Idle));

        var _pistol = WeaponManager.Instance.GetWeapon(WeaponType.Pistol) as Pistol;
        bulletsCounterIndicator.SetCount(_pistol.PatronsCount);
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

    private void SetIdleAnimation()
    {
        foreach (var parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, parameter.defaultBool);
            else if (parameter.type == AnimatorControllerParameterType.Int)
                _animator.SetInteger(parameter.name, parameter.defaultInt);
        }
    }

    public void SetAnimation(string animationIndex, bool isActivate)
    {
        if (animationIndex == "Idle" && isActivate) SetIdleAnimation();
        else
        {
            try { _animator.SetBool(animationIndex, isActivate); }
            catch { if (isActivate) _animator.SetTrigger(animationIndex); }
        }
    }

    public float GetAnimationDuration()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public string GetAnimationName()
    {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
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
