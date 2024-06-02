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

    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected NoiseEventHandler _noiseEventHandler;
    public event Action OnEnemyDisable;

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

    #endregion

    public EnemyStateMachine StateMachine { get; set; }

    #region Initialize

    [Inject]
    private void Construct(IInstantiator instantiator, NoiseEventHandler noiseEventHandler, Player player)
    {
        _instantiator = instantiator;
        _noiseEventHandler = noiseEventHandler;

        _target = player.transform;
    }

    public virtual void InitializeEnemy(Direction direction, bool isAvailable)
    {
        var pauseHandler = _instantiator.Instantiate<PauseHandler>();
        pauseHandler.SetCallbacks(Pause, Unpause);

        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
           Instantiate(_chaseState)
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
        if (!_isPaused) StateMachine.ChangeCurrentState(StateMachine.GetState(stateType));
    }

    public virtual void TakeHit(HitInfo hitInfo)
    {
        if (!_isPaused)
        {
            StartCoroutine(TakeHit());
            _hp -= hitInfo.Damage;
            if (_hp <= 0) ChangeState(EnemyStateType.Death);
            else
            {
                if (hitInfo.IsHasEffect(AdditiveHitEffect.Stun)) ApplyStun();
            }
        }
    }

    public void Death()
    {
        gameObject.SetActive(false);
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

    public void SetIdleAnimation()
    {
        foreach (var parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, parameter.defaultBool);
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

    IEnumerator TakeHit() // убрать, когда появится анимация
    {
        yield return new WaitWhile(() => _isPaused);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
        yield return new WaitWhile(() => _isPaused);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitWhile(() => _isPaused);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
        yield return new WaitWhile(() => _isPaused);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitWhile(() => _isPaused);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
        yield return new WaitWhile(() => _isPaused);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    public void ChangeAvailable(bool isAvailable) 
    {
        _isAvailable = isAvailable;
    }

    public float GetAnimationDuration()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public string GetAnimationName()
    {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
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
        _animator.speed = 0;
        _isPaused = true;
    }

    public void Unpause()
    {
        _animator.speed = 1;
        _isPaused = false;
    }

    public void OnDisable()
    {
        OnEnemyDisable?.Invoke();
    }
}