using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Movable Chase State", menuName = "Enemy's State/Movable Chase State")]
public class MovableEnemyChaseState : EnemyChaseState
{
    private MovableEnemy _enemyDefault;
    protected EnemyMove _movable;

    public override void Initialize(Enemy enemy, IInstantiator instantiator, Transform target)
    {
        base.Initialize(enemy, instantiator, target);
        _movable = enemy.GetComponent<EnemyMove>();
        _enemyDefault = enemy as MovableEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
        _enemy.SetAnimation("Move", true);
        _movable.StartMove();
    }

    public override void Update()
    {
        if (!_movable.IsMoving() || Vector2.Distance(_target.transform.position, _enemy.transform.position) <= _enemyDefault.HitDistance) IsStateFinished = true;
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.SetAnimation("Move", false);
        _movable.StopMove();
    }
}