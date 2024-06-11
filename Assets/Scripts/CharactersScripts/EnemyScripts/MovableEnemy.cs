using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public abstract class MovableEnemy : Enemy, IReclinable
{
    [SerializeField] private Trigger _trigger;

    private IMovableWithStops _movable;
    private Action<Vector2> MoveToNoise;

    [SerializeField] private EnemyMoveState _moveState;
    [SerializeField] private EnemyReclineState _reclineState;

    public override void InitializeEnemy(Direction direction, bool isAvailable)
    {
        base.InitializeEnemy(direction, isAvailable);
        _movable = GetComponent<IMovableWithStops>();

        if (direction == Direction.Left) (_movable as IMovableWithFlips).Flip(Vector2.left);
        _movable.SetSpeed((_enemyData as MovableEnemyData).Speed);

        MoveToNoise = noisePos =>
        {
            if (_isAvailable)
            {
                if (Vector2.Distance(new Vector2(noisePos.x, 0), new Vector2(transform.position.x, 0)) <= _enemyData.HearNoiseDistance && !_trigger.PlayerInTrigger)
                {
                    ChangeState(EnemyStateType.Move);
                    if (StateMachine.CurrentState is EnemyMoveState moveState) moveState.SetMovePosition(noisePos);
                }
            }
        };

        _noiseEventHandler.Noise += MoveToNoise;
    }

    protected override List<EnemyState> CreateStateInstances()
    {
        var stateInstances = base.CreateStateInstances();

        stateInstances.Add(Instantiate(_moveState));
        stateInstances.Add(Instantiate(_reclineState));

        return stateInstances;
    }

    protected override void Update()
    {
        base.Update();
        if (_isAvailable)
        {
            if (_target && Vector3.Distance(_target.position, transform.position) <= HitDistance)
            {
                if (StateMachine.GetState(EnemyStateType.Attack).IsStateAvailable()) Attack();
                else ChangeState(EnemyStateType.Cooldown);
            }
            else if (_trigger.PlayerInTrigger && Vector3.Distance(_target.position, transform.position) > HitDistance)
            {
                ChangeState(EnemyStateType.Chase);
            }
            else ChangeState(EnemyStateType.Idle);
        }
        else ChangeState(EnemyStateType.Idle);
    }

    public override void TakeHit(HitInfo hitInfo)
    {
        base.TakeHit(hitInfo);
        if (_hp > 0 && hitInfo.IsHasEffect(AdditiveHitEffect.Recline))
        {
            GetRecline(hitInfo.ReclineInfo.ReclinePoint, hitInfo.ReclineInfo.ReclineForce);
        }
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
