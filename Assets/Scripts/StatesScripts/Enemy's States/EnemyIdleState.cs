using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyIdleState : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        IsStateFinished = true;
    }
}
