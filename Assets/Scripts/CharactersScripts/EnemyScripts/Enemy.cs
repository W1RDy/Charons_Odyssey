using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Zenject;
using Cysharp.Threading.Tasks;

public abstract class Enemy : MonoBehaviour, IHasHealth, IParryingHittable, IStunable, IAttackable, IAvailable, IPause
{
    public EnemyType EnemyType { get; protected set; }
    [SerializeField] protected float _hp;
    [SerializeField] protected bool _isAvailable;
    [SerializeField] protected float _parryingWindowDuration;
    [SerializeField] protected float _damageTimeBeforeAnimationEnd = 0.1f;
    [SerializeField] protected float _stunningTime;
    [SerializeField] protected float _hearNoiseDistance;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    private PauseService _pauseService;

    protected NoiseEventHandler _noiseEventHandler;
    public event Action OnDeath;

    protected Transform _target;

    private bool _isPaused;
    public bool IsReadyForParrying { get; set; }
    public float ParryingWindowDuration => _parryingWindowDuration;
    public float DamageTimeBeforeAnimationEnd => _damageTimeBeforeAnimationEnd;

    #region Enemy's states

    [SerializeField] private EnemyIdleState _idleState;
    [SerializeField] private EnemyChaseState _chaseState;
    [SerializeField] private EnemyAttackState _attackState;
    [SerializeField] private EnemyReclineState _reclineState;
    [SerializeField] private EnemyStunState _stunState;
    [SerializeField] private EnemyMoveState _moveState;

    #endregion

    public EnemyStateMachine StateMachine { get; set; }

    [Inject]
    private void Construct(PauseService pauseService, NoiseEventHandler noiseEventHandler, Player player)
    {
        _pauseService = pauseService;
        _noiseEventHandler = noiseEventHandler;

        _target = player.transform;
    }

    public virtual void InitializeEnemy(Direction direction, bool isAvailable)
    {
        _pauseService.AddPauseObj(this);
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        StateMachine = new EnemyStateMachine();
        InitializeStatesInstances();
        ChangeAvailable(isAvailable);
    }

    private void InitializeStatesInstances()
    {
        var stateInstances = new List<EnemyState>()
        {
           Instantiate(_idleState),
           Instantiate(_chaseState),
           Instantiate(_attackState),
           Instantiate(_reclineState),
           Instantiate(_stunState),
           Instantiate(_moveState)
        };

        StateMachine.InitializeStatesDictionary(stateInstances);
    }

    private void Start()
    {
        StateMachine.InitializeStates(this, _pauseService, _target);
        StateMachine.InitializeCurrentState(StateMachine.GetState(EnemyStateType.Idle));
    }

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

    public void TakeHit(float damage)
    {
        if (!_isPaused)
        {
            StartCoroutine(TakeHit());
            _hp -= damage;
            if (_hp <= 0) Death();
        }
    }

    public void Death()
    {
        _pauseService.RemovePauseObj(this);
        OnDeath?.Invoke();
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
        (StateMachine.CurrentState as EnemyStunState).SetInterruptedState(interruptedState);
    }

    public virtual void Attack()
    {

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
}