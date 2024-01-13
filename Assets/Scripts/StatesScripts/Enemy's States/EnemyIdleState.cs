using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stay State", menuName = "Enemy's State/Stay State")]
public class EnemyIdleState : EnemyState
{
    public override void Enter()
    {
        IsStateFinished = true;
    }
}
