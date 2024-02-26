using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Zenject;

public abstract class Enemy : MonoBehaviour, IHasHealth, IAttackable, IAvailable, IPause
{
    public EnemyType EnemyType { get; protected set; }
    [SerializeField] protected float _hp;
    [SerializeField] protected bool _isAvailable;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    private PauseService _pauseService;
    private bool _isPaused;

    #region Enemy's states

    [SerializeField] private EnemyIdleState _idleState;
    [SerializeField] private EnemyMoveState _moveState;
    [SerializeField] private EnemyAttackState _attackState;
    [SerializeField] private EnemyReclineState _reclineState;

    #endregion

    public EnemyStateMachine StateMachine { get; set; }

    [Inject]
    private void Construct(PauseService pauseService)
    {
        _pauseService = pauseService;
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
           Instantiate(_moveState),
           Instantiate(_attackState),
           Instantiate(_reclineState)
        };

        StateMachine.InitializeStatesDictionary(stateInstances);
    }

    private void Start()
    {
        StateMachine.InitializeStates(this);
        StateMachine.InitializeCurrentState(StateMachine.GetState(EnemyStateType.Idle));
    }

    protected virtual void Update()
    {
        if (!_isPaused) StateMachine.CurrentState.Update();
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
        gameObject.SetActive(false);
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