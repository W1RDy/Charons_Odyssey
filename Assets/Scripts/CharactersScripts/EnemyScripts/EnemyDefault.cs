using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyDefault : Enemy, IReclinable
{
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _speed;
    [SerializeField] private float _hitDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    private Transform _target;
    private IMovableWithStops _movable;
    public float HitDistance { get => _hitDistance; }
    public float Damage { get => _damage; }
    public float AttackCooldown { get => _cooldown; }

    protected override void Awake()
    {
        base.Awake();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _movable = GetComponent<IMovableWithStops>();
        _movable.SetSpeed(_speed);
        _trigger.TriggerWorked += StartMove;
        _trigger.TriggerTurnedOff += _movable.StopMove;
    }

    protected override void Update()
    {
        base.Update();
        if (_isAvailable)
        {
            if (_target && Vector3.Distance(_target.position, transform.position) < _hitDistance)
            {
                if (StateMachine.GetState(EnemyStateType.Attack).IsStateAvailable()) Attack();
                else ChangeState(EnemyStateType.Idle);
            }
            else if (_trigger.playerInTrigger && Vector3.Distance(_target.position, transform.position) > _hitDistance)
            {
                ChangeState(EnemyStateType.Move);
            }

            if (StateMachine.CurrentState.IsStateFinished) ChangeState(EnemyStateType.Idle);
        }
        else ChangeState(EnemyStateType.Idle);
    }

    private void StartMove()
    {
        if (_isAvailable) _movable.StartMove();
    }

    public override void Attack()
    {
        ChangeState(EnemyStateType.Attack);
    }

    public void GetRecline(Transform _recliner, float _reclineForce)
    {
        if (_hp > 0)
        {
            (StateMachine.GetState(EnemyStateType.Recline) as EnemyReclineState).SetReclineParameters(_recliner, _reclineForce);
            ChangeState(EnemyStateType.Recline);
        }
    }

    private void OnDestroy()
    {
        _trigger.TriggerWorked -= StartMove;
        _trigger.TriggerTurnedOff -= _movable.StopMove;
    }
}
