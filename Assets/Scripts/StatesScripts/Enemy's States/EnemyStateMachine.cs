using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyStateMachine
{
    public EnemyState CurrentState { get; set; }
    private Dictionary<EnemyStateType, EnemyState> _statesDictionary;

    public void InitializeStatesDictionary(List<EnemyState> states)
    {
        _statesDictionary = new Dictionary<EnemyStateType, EnemyState>();

        foreach (var state in states)
        {
            _statesDictionary[state.GetStateType()] = state;
        }
    }

    public void InitializeStates(Enemy enemy, IInstantiator instantiator, Transform target)
    {
        foreach (var state in _statesDictionary.Values)
        {
            if (state.GetStateType() == EnemyStateType.Attack) (state as EnemyAttackState).Initialize(enemy, instantiator, target);
            else if (state.GetStateType() == EnemyStateType.Chase) (state as EnemyChaseState).Initialize(enemy, instantiator, target);
            else if (state is EnemyIdleStayState idleStayState) idleStayState.Initialize(enemy, instantiator, target);
            else state.Initialize(enemy, instantiator);
        }
    }

    public EnemyState GetState(EnemyStateType stateType) => _statesDictionary[stateType];

    public void InitializeCurrentState(EnemyState defaultState)
    {
        CurrentState = defaultState;
        CurrentState.Enter();
    }

    public void ChangeCurrentState(EnemyState newState)
    {
        if (CurrentState != newState && (CurrentState.IsStateFinished || CurrentState.GetPriorityIndex() < newState.GetPriorityIndex()) && newState.IsStateAvailable())
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
