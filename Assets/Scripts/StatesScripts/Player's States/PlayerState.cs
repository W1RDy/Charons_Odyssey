using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : ScriptableObject, IPause
{
    [SerializeField] private int _priorityIndex;
    [SerializeField] private PlayerStateType _stateType;
    private PauseService _pauseService;
    protected bool _isPaused;
    protected Player _player;
    public bool IsStateFinished { get; protected set; }

    public virtual void Initialize(Player player, PauseService pauseService)
    {
        _player = player;
        _player.OnDeath += OnPlayerDeath;

        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
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

    public virtual void OnPlayerDeath()
    {
        _pauseService.RemovePauseObj(this);
        _player.OnDeath -= OnPlayerDeath;
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
