using System;
using UnityEngine;

public abstract class UnmovableEnemy : Enemy
{
    private Action<Vector2> ReactToNoise;

    public override void InitializeEnemy(Direction direction, bool isAvailable)
    {
        base.InitializeEnemy(direction, isAvailable);

        if (direction == Direction.Left) Flip(Vector2.left);

        ReactToNoise = noisePos =>
        {
            if (_isAvailable)
            {
                if (IsCanHearNoize(noisePos))
                {
                    var noiseDirection = (noisePos - new Vector2(transform.position.x, transform.position.y)).normalized;
                    Flip(noiseDirection);
                }
            }
        };
        _noiseEventHandler.Noise += ReactToNoise;
    }

    private bool IsCanHearNoize(Vector2 noisePos)
    {
        return Math.Abs(noisePos.x - transform.position.x) <= _enemyData.HearNoiseDistance && Math.Abs(noisePos.y - transform.position.y) < transform.localScale.y && !_trigger.PlayerInTrigger;
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