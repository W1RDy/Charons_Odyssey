using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class PlayerState : ScriptableObject, IPause
{
    [SerializeField] private int _priorityIndex;
    [SerializeField] private PlayerStateType _stateType;

    protected PauseHandler _pauseHandler;
    protected bool _isPaused;

    protected Player _player;

    protected AudioMaster _audioMaster;
    public bool IsStateFinished { get; protected set; }

    public virtual void Initialize(Player player, IInstantiator instantiator, AudioMaster audioMaster)
    {
        _player = player;
        _player.OnPlayerDisable += OnPlayerDisable;

        _pauseHandler = instantiator.Instantiate<PauseHandler>();
        _pauseHandler.SetCallbacks(Pause, Unpause);

        _audioMaster = audioMaster;
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

    public virtual void Pause()
    { 
        if (!_isPaused) _isPaused = true;
    }

    public virtual void Unpause()
    {
        if (_isPaused) _isPaused = false;
    }

    public virtual void OnPlayerDisable()
    {
        _player.OnPlayerDisable -= OnPlayerDisable;
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
    Talk,
    Shield,
    Parrying,
    Dodge,
    Stun
}
