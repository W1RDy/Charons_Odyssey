using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyIdleState : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        IsStateFinished = true;

        if (_enemy as Minotaur != null) _enemy.PlaySound("MinotaurIdle");
    }

    public override void Exit()
    {
        base.Exit();
        if (_enemy as Minotaur != null) _enemy.StopSound();
    }
}
