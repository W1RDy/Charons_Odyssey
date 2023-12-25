using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; set; }
    private Dictionary<PlayerStateType, PlayerState> _statesDictionary;
    private bool _isChanging;

    public void InitializeStatesDictionary(List<PlayerState> states)
    {
        _statesDictionary = new Dictionary<PlayerStateType, PlayerState>();

        foreach (var state in states)
        {
            _statesDictionary[state.GetStateType()] = state;
        }
    }

    public void InitializeStates(Player player)
    {
        foreach (var state in _statesDictionary.Values) state.Initialize(player);
    }

    public PlayerState GetState(PlayerStateType stateType) => _statesDictionary[stateType];

    public void InitializeCurrentState(PlayerState defaultState)
    {
        CurrentState = defaultState;
        CurrentState.Enter();
    }

    public async void ChangeCurrentState(PlayerState newState) 
    {
        if (CurrentState != newState && (CurrentState.IsStateFinished || CurrentState.GetPriorityIndex() < newState.GetPriorityIndex()) && newState.IsStateAvailable() && !_isChanging)
        {
            _isChanging = true;
            await CurrentState.WaitTransition(newState.GetStateType());
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
            _isChanging = false;
        }
    }
}
