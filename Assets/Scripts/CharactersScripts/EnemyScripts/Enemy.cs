using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHasHealth, IAttackable, IAvailable
{
    [SerializeField] protected float _hp;
    [SerializeField] protected bool _isAvailable;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    #region Enemy's states

    [SerializeField] private EnemyIdleState _idleState;
    [SerializeField] private EnemyMoveState _moveState;
    [SerializeField] private EnemyAttackState _attackState;
    [SerializeField] private EnemyReclineState _reclineState;

    #endregion

    public EnemyStateMachine StateMachine { get; set; }

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        StateMachine = new EnemyStateMachine();
        InitializeStatesInstances();
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
        StateMachine.CurrentState.Update();
    }

    public void ChangeState(EnemyStateType stateType)
    {
        StateMachine.ChangeCurrentState(StateMachine.GetState(stateType));
    }

    public void TakeHit(float damage)
    {
        StartCoroutine(TakeHit());
        _hp -= damage;
        if (_hp <= 0) Death();
    }

    public void Death()
    {
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

    IEnumerator TakeHit() // переделать
    {
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.red;
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
}