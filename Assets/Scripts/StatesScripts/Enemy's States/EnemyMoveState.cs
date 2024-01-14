using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move State", menuName = "Enemy's State/Move State")]
public class EnemyMoveState : EnemyState
{
    protected EnemyMove _enemyMove;
    protected EnemyMove _movable;

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);
        _movable = enemy.GetComponent<EnemyMove>();
    }

    public override void Enter()
    {
        IsStateFinished = false;
        _enemy.SetAnimation("Move", true);
        _movable.StartMove();
    }

    public override void Update()
    {
        if (!_movable.IsMoving()) IsStateFinished = true;
    }

    public override void Exit()
    {
        _enemy.SetAnimation("Move", false);
        _movable.StopMove();
    }

    public override bool IsStateAvailable()
    {
        return base.IsStateAvailable();
    }
}
