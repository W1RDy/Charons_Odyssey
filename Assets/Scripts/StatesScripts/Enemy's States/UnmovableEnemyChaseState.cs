using UnityEngine;

[CreateAssetMenu(fileName = "Unmovable Chase State", menuName = "Enemy's State/Unmovable Chase State")]
public class UnmovableEnemyChaseState : EnemyChaseState
{
    public override void Enter()
    {
        Debug.Log("EnterChaseState");
        base.Enter();
        IsStateFinished = false;
    }

    public override void Update()
    {
        var vectorToTarget = (_target.transform.position - _enemy.transform.position);

        if (vectorToTarget.magnitude > _enemy.HitDistance || !_enemy.IsPlayerOnView)
        {
            IsStateFinished = true;

        }
        else
        {
            _enemy.Flip(vectorToTarget.normalized);
        }
    }
}