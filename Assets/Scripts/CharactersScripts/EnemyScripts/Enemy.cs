using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHasHealth, IAttackable, IHasStates, IAvailable
{
    [SerializeField] protected float _hp;
    [SerializeField] protected bool _isAvailable;
    public State State { get; private set; }
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    public List<State> _activatedStates;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _activatedStates = new List<State>() { State.Idle };
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

    public void EnableState(State state)
    {
        if (_isAvailable || (!_isAvailable && State != State.Idle))
        {
            if (state == State.Idle) _activatedStates.Clear();
            if (!_activatedStates.Contains(state))
            {
                _activatedStates.Add(state);
                State = StatesManager.Instance.ChangeCurrentState(tag, State, _activatedStates);
                if (state == State.Idle || state == State.WaitCooldown || state == State.Recline) SetIdleAnimation();
                else _animator.SetBool(state.ToString(), true);
                Debug.Log("state: " + State);
            }
        }
    }

    public void DisableState(State state)
    {
        if (!_isAvailable) EnableState(State.Idle);
        else
        {
            if (_activatedStates.Contains(state))
            {
                _activatedStates.Remove(state);
                State = StatesManager.Instance.ChangeCurrentState(tag, State, _activatedStates);
                Debug.Log("enemy current state = " + State);
                if (state == State.Idle) throw new ArgumentException("Can't disable default state, set state instead!");
                else _animator.SetBool(state.ToString(), false);
            }
        }
    }

    public State GetState()
    {
        return State;
    }

    public void SetIdleAnimation()
    {
        foreach (var parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, parameter.defaultBool);
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
        Debug.Log(isAvailable);
        _isAvailable = isAvailable;
    }
}