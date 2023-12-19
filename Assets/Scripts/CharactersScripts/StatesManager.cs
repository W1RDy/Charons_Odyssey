using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatesManager : MonoBehaviour
{
    public static StatesManager Instance;

    [SerializeField] private ObjWithStatesConfig[] objStatesConfigs;
    Dictionary<string, ObjWithStatesConfig> objWithStatesDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeObjWithStatesDictionary();
        }
        else Destroy(gameObject);
    }

    private void InitializeObjWithStatesDictionary()
    {
        objWithStatesDictionary = new Dictionary<string, ObjWithStatesConfig>(); 

        foreach (var objStateConfig in objStatesConfigs)
        {
            objWithStatesDictionary[objStateConfig.objTag] = objStateConfig;
            objStateConfig.FillStatesConfigs();
        }
    }

    public State ChangeCurrentState(string tag, State currentState, List<State> activatedStates)
    {
        return objWithStatesDictionary[tag].GetMaxPriorityState(currentState, activatedStates);
    }

    public bool IsCanMakeTransition(string tag, State currentState, State state)
    {
        return objWithStatesDictionary[tag].IsCanMakeTransition(currentState, state);
    }
}



[Serializable]
public class StateConfig
{
    public int priorityIndex;
    public State state;
    public State[] connectedStates;

    public bool IsCanMakeTransition(StateConfig stateConfig)
    {
        return stateConfig.priorityIndex > priorityIndex && connectedStates.Contains(stateConfig.state);
    }
}

[Serializable]
public class ObjWithStatesConfig
{
    public string objTag;
    public StateConfig[] stateConfigs;
    private Dictionary<State, StateConfig> _statesDictionary;

    public void FillStatesConfigs()
    {
        _statesDictionary = new Dictionary<State, StateConfig>();

        foreach (var stateConfig in stateConfigs)
        {
            if (stateConfig.connectedStates.Length == 0) FillConnectedStates(stateConfig);
            _statesDictionary[stateConfig.state] = stateConfig;
        }
    }

    private void FillConnectedStates(StateConfig stateConfig)
    {
        stateConfig.connectedStates = new State[stateConfigs.Length - 1];
        var connectedStateIndex = 0;
        for (int i = 0; i < stateConfigs.Length; i++)
        {
            if (stateConfig.state != stateConfigs[i].state)
                stateConfig.connectedStates[connectedStateIndex++] = stateConfigs[i].state;
        }
    }

    public State GetMaxPriorityState(State currentState, List<State> _activatedStates)
    {
        var _state = _statesDictionary[State.Idle];
        var _currentState = _statesDictionary[currentState];

        foreach (var activatedState in _activatedStates)
        {
            var _activatedState = _statesDictionary[activatedState];
            if (_currentState.IsCanMakeTransition(_activatedState)) _state = _activatedState;
        }
        return _state.state;
    }

    public bool IsCanMakeTransition(State currentState, State state)
    {
        var _currentState = _statesDictionary[currentState];
        var _state = _statesDictionary[state];
        return _currentState.IsCanMakeTransition(_state);
    }
}

public enum State
{
    Idle,
    Recline,
    WaitCooldown,
    Move,
    Heal,
    Attack,
    Climb
}
