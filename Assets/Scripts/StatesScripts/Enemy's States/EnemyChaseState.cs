using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase State", menuName = "Enemy's State/Chase State")]
public class EnemyChaseState : EnemyState
{
    private EnemyDefault _enemyDefault;
    protected EnemyMove _movable;
    private Transform _target;

    public void Initialize(Enemy enemy, PauseService pauseService, Transform target)
    {
        base.Initialize(enemy, pauseService);
        _movable = enemy.GetComponent<EnemyMove>();
        _target = target;
        _enemyDefault = enemy as EnemyDefault;
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

    public override bool IsStateAvailable()
    {
        return base.IsStateAvailable();
    }
}
