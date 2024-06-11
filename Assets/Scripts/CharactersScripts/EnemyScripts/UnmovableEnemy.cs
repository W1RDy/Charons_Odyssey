using System;
using UnityEngine;

public abstract class UnmovableEnemy : Enemy
{
    [SerializeField] private Trigger _trigger;

    private Action<Vector2> ReactToNoise;

    public override void InitializeEnemy(Direction direction, bool isAvailable)
    {
        base.InitializeEnemy(direction, isAvailable);

        if (direction == Direction.Left) Flip(Vector2.left);

        ReactToNoise = noisePos =>
        {
            if (_isAvailable)
            {
                if (Vector2.Distance(new Vector2(noisePos.x, 0), new Vector2(transform.position.x, 0)) <= _enemyData.HearNoiseDistance && !_trigger.PlayerInTrigger)
                {
                    var noiseDirection = (noisePos - new Vector2(transform.position.x, transform.position.y)).normalized;
                    Flip(noiseDirection);
                }
            }
        };
        _noiseEventHandler.Noise += ReactToNoise;
    }

    protected override void Update()
    {
        base.Update();
        if (_isAvailable)
        {
            if (_target && _trigger.PlayerInTrigger && Vector3.Distance(_target.position, transform.position) <= HitDistance)
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

    public void OnDestroy()
    {
        _noiseEventHandler.Noise -= ReactToNoise;
    }
}