using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
    [SerializeField] private int _priorityIndex;
    [SerializeField] EnemyStateType _stateType;
    protected Enemy _enemy;
    public bool IsStateFinished { get; protected set; }

    public virtual void Initialize(Enemy enemy)
    {
        _enemy = enemy;
    }

    public virtual void Enter() { }
    public virtual void Exit() { ResetValues(); }
    public virtual void Update() { }
    public virtual void ResetValues() { }
    public int GetPriorityIndex() => _priorityIndex;
    public EnemyStateType GetStateType() => _stateType;
    public virtual bool IsStateAvailable() => true;
}

public enum EnemyStateType
{
    Idle,
    Move,
    Recline,
    Attack
}
