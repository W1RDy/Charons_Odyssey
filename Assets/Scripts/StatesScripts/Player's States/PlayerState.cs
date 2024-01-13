using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : ScriptableObject
{
    [SerializeField] private int _priorityIndex;
    [SerializeField] private PlayerStateType _stateType;
    protected Player _player;
    public bool IsStateFinished { get; protected set; }

    public virtual void Initialize(Player player)
    {
        _player = player;
    }

    public virtual void Enter() { }
    public virtual void Exit() { ResetValues(); }
    public virtual void Update() { }
    public virtual void ResetValues() { }

    public int GetPriorityIndex() => _priorityIndex;

    public PlayerStateType GetStateType() => _stateType;

    public virtual bool IsStateAvailable() => true;

    public virtual async UniTask WaitTransition(PlayerStateType newStateType)
    {
        await UniTask.Yield();
    }
}

public enum PlayerStateType
{
    Idle,
    Move,
    Climb,
    Heal,
    AttackWithPistol,
    AttackWithFist,
    AttackWithPaddle,
    IdleWithGun,
    Talk
}
