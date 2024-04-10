using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

public class EnemyDefault : Enemy, IReclinable
{
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _speed;
    [SerializeField] protected float _hitDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;

    private IMovableWithStops _movable;
    public float HitDistance { get => _hitDistance; }
    public float Damage { get => _damage; }
    public float AttackCooldown { get => _cooldown; }

    private Action<Vector2> MoveToNoise;


    public override void InitializeEnemy(Direction direction, bool isAvailable)
    {
        base.InitializeEnemy(direction, isAvailable);
        _movable = GetComponent<IMovableWithStops>();

        if (direction == Direction.Left) (_movable as IMovableWithFlips).Flip(Vector2.left);
        _movable.SetSpeed(_speed);

        EnemyType = EnemyType.Default;

        MoveToNoise = noisePos =>
        {
            if (_isAvailable)
            {
                if (Vector2.Distance(new Vector2(noisePos.x, 0), new Vector2(transform.position.x, 0)) <= _hearNoiseDistance && !_trigger.PlayerInTrigger)
                {
                    ChangeState(EnemyStateType.Move);
                    if (StateMachine.CurrentState is EnemyMoveState moveState) moveState.SetMovePosition(noisePos);
                }
            }
        };

        _noiseEventHandler.Noise += MoveToNoise;
    }

    protected override void Update()
    {
        base.Update();
        if (_isAvailable)
        {
            if (_target && Vector3.Distance(_target.position, transform.position) < _hitDistance)
            {
                if (StateMachine.GetState(EnemyStateType.Attack).IsStateAvailable()) Attack();
                else ChangeState(EnemyStateType.Cooldown);
            }
            else if (_trigger.PlayerInTrigger && Vector3.Distance(_target.position, transform.position) > _hitDistance)
            {
                ChangeState(EnemyStateType.Chase);
            }
            else ChangeState(EnemyStateType.Idle);
        }
        else ChangeState(EnemyStateType.Idle);
    }

    public override void Attack()
    {
        ChangeState(EnemyStateType.Attack);
    }

    public void GetRecline(Transform recliner, float reclineForce)
    {
        if (_hp > 0)
        {
            (StateMachine.GetState(EnemyStateType.Recline) as EnemyReclineState).SetReclineParameters(recliner, reclineForce);
            ChangeState(EnemyStateType.Recline);
        }
    }

    public void OnDestroy()
    {
        _noiseEventHandler.Noise -= MoveToNoise;
    }
}