using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : ScriptableObject, IPause
{
    [SerializeField] private int _priorityIndex;
    [SerializeField] private EnemyStateType _stateType;
    protected Enemy _enemy;

    private PauseService _pauseService;
    protected bool _isPaused;

    public bool IsStateFinished { get; protected set; }

    public virtual void Initialize(Enemy enemy, PauseService pauseService)
    {
        _enemy = enemy;
        _enemy.OnEnemyDisable += OnEnemyDisable;

        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    public virtual void Enter() { }
    public virtual void Exit() { ResetValues(); }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void ResetValues() { }
    public int GetPriorityIndex() => _priorityIndex;
    public EnemyStateType GetStateType() => _stateType;
    public virtual bool IsStateAvailable() => true;

    public virtual void Pause()
    {
        if (!_isPaused) _isPaused = true;
    }

    public virtual void Unpause()
    {
        if (_isPaused) _isPaused = false;
    }

    public virtual void OnEnemyDisable()
    {
        _pauseService.RemovePauseObj(this);
        _enemy.OnEnemyDisable -= OnEnemyDisable;
    }
}

public enum EnemyStateType
{
    Idle,
    Chase,
    Recline,
    Attack,
    Stun,
    Move,
    Cooldown
}
