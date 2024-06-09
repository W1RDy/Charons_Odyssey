using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Enemy : MonoBehaviour, IHasHealth, IParryingHittable, IStunable, IAttackable, IAvailable, IPause
{
    public EnemyType EnemyType { get; protected set; }
    [SerializeField] protected float _hp;
    [SerializeField] protected float _hitDistance;
    [SerializeField] protected float _hearNoiseDistance;

    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _damage;

    public float HitDistance => _hitDistance;
    public float AttackCooldown => _attackCooldown;
    public float Damage => _damage;

    [SerializeField] protected float _parryingWindowDuration;
    [SerializeField] protected float _damageTimeBeforeAnimationEnd = 0.1f;
    [SerializeField] protected float _stunningTime;

    [SerializeField] protected bool _isAvailable;

    private EnemyView _view;
    private HPBarInitializer _hpBarInitializer;

    [SerializeField] private StunAnimation _stunAnimation;
    [SerializeField] private TakeHitAnimation _takeHitAnimation;

    protected NoiseEventHandler _noiseEventHandler;
    public event Action OnEnemyDisable;

    private BattleActivator _battleActivator;

    private bool _isAgressive;
    public event Action OnEnemyAgressive;
    public event Action OnEnemyCalm;

    protected Transform _target;
    private IInstantiator _instantiator;

    private bool _isPaused;
    public bool IsReadyForParrying { get; set; }
    public float ParryingWindowDuration => _parryingWindowDuration;
    public float DamageTimeBeforeAnimationEnd => _damageTimeBeforeAnimationEnd;

    #region Enemy's states

    [SerializeField] private EnemyIdleState _idleState;
    [SerializeField] private EnemyIdleState _cooldownState;
    [SerializeField] private EnemyAttackState _attackState;
    [SerializeField] private EnemyStunState _stunState;
    [SerializeField] private EnemyDeathState _deathState;
    [SerializeField] private EnemyChaseState _chaseState;
    [SerializeField] private EnemyTakeHitState _takeHitState;

    #endregion

    public EnemyStateMachine StateMachine { get; set; }

    #region Initialize

    [Inject]
    private void Construct(IInstantiator instantiator, NoiseEventHandler noiseEventHandler, Player player, HPBarInitializer hpBarInitializer, BattleActivator battleActivator)
    {
        _instantiator = instantiator;
        _noiseEventHandler = noiseEventHandler;

        _target = player.transform;
        _hpBarInitializer = hpBarInitializer;

        _battleActivator = battleActivator;
        _battleActivator.AddEnemyToSubscribe(this);
    }

    public virtual void InitializeEnemy(Direction direction, bool isAvailable)
    {
        var pauseHandler = _instantiator.Instantiate<PauseHandler>();
        pauseHandler.SetCallbacks(Pause, Unpause);

        var animator = GetComponentInChildren<Animator>();
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var hpBar = _hpBarInitializer.GetHpBar(spriteRenderer, _hp, transform);
        _view = new EnemyView(spriteRenderer, animator, _stunAnimation, _takeHitAnimation, hpBar);

        StateMachine = new EnemyStateMachine();
        InitializeStatesInstances();
        ChangeAvailable(isAvailable);
    }

    protected virtual List<EnemyState> CreateStateInstances()
    {
        return new List<EnemyState>()
        {
           Instantiate(_idleState),
           Instantiate(_attackState),
           Instantiate(_stunState),
           Instantiate(_cooldownState),
           Instantiate(_deathState),
           Instantiate(_chaseState),
           Instantiate(_takeHitState),
        };
    }

    private void InitializeStatesInstances()
    {
        var stateInstances = CreateStateInstances();
        StateMachine.InitializeStatesDictionary(stateInstances);
    }

    private void Start()
    {
        StateMachine.InitializeStates(this, _instantiator, _target);
        StateMachine.InitializeCurrentState(StateMachine.GetState(EnemyStateType.Idle));
    }

    #endregion

    protected virtual void Update()
    {
        if (!_isPaused) StateMachine.CurrentState.Update();
    }

    protected virtual void FixedUpdate()
    {
        if (!_isPaused) StateMachine.CurrentState.FixedUpdate();
    }

    public void ChangeState(EnemyStateType stateType)
    {
        if (!_isPaused)
        {
            if (stateType == EnemyStateType.Idle || stateType == EnemyStateType.Move || stateType == EnemyStateType.Death)
            {
                ChangeAgressiveState(false);
            }
            else ChangeAgressiveState(true);

            StateMachine.ChangeCurrentState(StateMachine.GetState(stateType));
        }
    }

    private void ChangeAgressiveState(bool isChangeToAgressive)
    {
        if (!_isAgressive && isChangeToAgressive)
        {
            _isAgressive = true;
            OnEnemyAgressive?.Invoke();
        }
        else if (_isAgressive && !isChangeToAgressive)
        {
            _isAgressive = false;
            OnEnemyCalm?.Invoke();
        }
    }

    public virtual void TakeHit(HitInfo hitInfo)
    {
        if (!_isPaused)
        {
            _hp -= hitInfo.Damage;
            _view.ChangeHPView(_hp);

            if (_hp <= 0)
            {
                ChangeState(EnemyStateType.Death);
            }
            else
            {
                StateMachine.GetState(EnemyStateType.TakeHit).Enter();
                if (hitInfo.IsHasEffect(AdditiveHitEffect.Stun)) ApplyStun();
            }
        }
    }

    public void Death()
    {
        gameObject.SetActive(false);
        ChangeAgressiveState(false);
        _battleActivator.UnsubscribeToEnemy(this);
    }

    public void ApplyParrying()
    {
        ApplyStun();
    }

    public void ApplyStun()
    {
        var interruptedState = StateMachine.CurrentState;
        ChangeState(EnemyStateType.Stun);
        if (StateMachine.CurrentState is EnemyStunState enemyStunState) enemyStunState.SetInterruptedState(interruptedState);
    }

    public virtual void Attack()
    {
        ChangeState(EnemyStateType.Attack);
    }

    public void ChangeAvailable(bool isAvailable) 
    {
        _isAvailable = isAvailable;
    }

    public float GetAnimationDuration()
    {
        return _view.GetAnimationDuration();
    }

    public string GetAnimationName()
    {
        return _view.GetAnimationName();
    }

    public void SetAnimation(string animationIndex, bool isActivate)
    {
        _view.SetAnimation(animationIndex, isActivate);
    }

    public void Flip(Vector2 direction)
    {
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    public void Pause()
    {
        _view.Pause();
        _isPaused = true;
    }

    public void Unpause()
    {
        _view.Unpause();
        _isPaused = false;
    }

    public void OnDisable()
    {
        OnEnemyDisable?.Invoke();
    }
}